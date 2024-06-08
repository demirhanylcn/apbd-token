using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using token.Context;
using token.Contracts;
using token.Exception;
using token.Helpers;
using token.Models;
using token.RepositoryInterfaces;
using token.ServiceInterfaces;
using LoginRequest = token.Contracts.LoginRequest;


namespace token.Service;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;
    private readonly AppDbContext _appDbContext;

    public AuthService(IConfiguration configuration,IUserRepository userRepository,AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public void RegisterUser(RegisterUserRequest request)
    {
        
        _userRepository.CheckUserWithMailExists(request);
        _userRepository.CheckPasswordStrongEnough(request);
        
        var (hashedPassword, salt) = SecurityHelpers.GetHashedPasswordAndSalt(request.Password);
        
             var customerToAdd = new Customer
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Salt = salt,
                Password = hashedPassword
            };
        
        
        

        _appDbContext.Customers.Add(customerToAdd);
        _appDbContext.SaveChanges();
    }

    public (string accessToken, string refreshToken) LoginUser(LoginRequest request)
    {

        _userRepository.CheckLoginDetails(request);
        var user = _appDbContext.Customers.FirstOrDefault(e => e.Email == request.Email);
        
        var hashedPassword = SecurityHelpers.GetHashedPasswordWithSalt(request.Password, user.Salt);
        
        
        if (hashedPassword != user.Password) throw new ThiefExeption();

        var userClaims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.Id)),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, "Customer")
        };

        var accessToken = GenerateAccessToken(userClaims);
        var refreshToken = GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExp = DateTime.UtcNow.AddDays(30);
        _appDbContext.SaveChanges();
        return (accessToken, refreshToken);
    }

    public async Task<string> GetNewAccessToken(string refleshToken)
    {
        var customer = await _userRepository.GetCustomer(refleshToken);
        if (customer == null) throw new UserDoesntExistsException();
        var claim =  _userRepository.GetCustomerClaim(customer);
        var newToken =  GenerateAccessToken(claim);
        return newToken;
    }

    private string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var sskey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Auth:SecretKey"]));
        var credentials = new SigningCredentials(sskey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _configuration["Auth:ValidIssuer"],
            _configuration["Auth:ValidAudience"],
            claims,
            expires: DateTime.Now.AddMinutes(5),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
    
}