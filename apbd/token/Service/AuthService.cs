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
        
        User userToAdd;
        var (hashedPassword, salt) = SecurityHelpers.GetHashedPasswordAndSalt(request.Password);
        if (_appDbContext.Users.Any())
        {
             userToAdd = new User
            {
                Id = _appDbContext.Users.Max(e => e.Id) + 1,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Salt = salt,
                Password = hashedPassword
            };
        }
        else
        {
             userToAdd = new User
            {
                Id = 1,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Salt = salt,
                Password = hashedPassword
            };
        }
        
        

        _appDbContext.Users.Add(userToAdd);
    }

    public (string accessToken, string refreshToken) LoginUser(LoginRequest request)
    {
        var user = _appDbContext.Users.FirstOrDefault(e =>
            string.Equals(request.Email, e.Email, StringComparison.OrdinalIgnoreCase));

        if (user == null) throw new UserDoesntExistsException();
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

        return (accessToken, refreshToken);
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