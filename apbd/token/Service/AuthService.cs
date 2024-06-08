using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using solution.Models;
using token.Contracts;
using token.Helpers;
using token.ServiceInterfaces;
using LoginRequest = token.Contracts.LoginRequest;


namespace solution.Service;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;

    public AuthService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void RegisterUser(RegisterUserRequest request)
    {
        // check if user with email exists etc...
        var (hashedPassword, salt) = SecurityHelpers.GetHashedPasswordAndSalt(request.Password);
        var userToAdd = new User
        {
            Id = AppDbContext.Users.Max(e => e.Id) + 1,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Salt = salt,
            Password = hashedPassword
        };
        
        AppDbContext.Users.Add(userToAdd);
    }

    public (string accessToken, string refreshToken) LoginUser(LoginRequest request)
    {
        var user = AppDbContext.Users.FirstOrDefault(e =>
            string.Equals(request.Email, e.Email, StringComparison.OrdinalIgnoreCase));

        // handle if user not found
        var hashedPassword = SecurityHelpers.GetHashedPasswordWithSalt(request.Password, user.Salt);
        
        //handle invalid password provided
        if (hashedPassword != user.Password) throw new System.Exception("Incorrect password bla bla bla");
        
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
            issuer: _configuration["Auth:ValidIssuer"],
            audience: _configuration["Auth:ValidAudience"],
            claims: claims,
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