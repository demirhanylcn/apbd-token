
using token.Contracts;
using LoginRequest = token.Contracts.LoginRequest;

namespace token.ServiceInterfaces;

public interface IAuthService
{
    public void RegisterUser(RegisterUserRequest request);
    public (string accessToken, string refreshToken) LoginUser(LoginRequest request);
    
}