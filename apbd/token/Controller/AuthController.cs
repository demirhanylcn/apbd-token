using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using token.Contracts;
using token.ServiceInterfaces;


namespace token.Controller;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth)
    {
        _auth = auth;
    }

    [HttpPost]
    [Route("register")]
    [AllowAnonymous]
    public IActionResult RegisterUser([FromBody] RegisterUserRequest request)
    {
        _auth.RegisterUser(request);
        return Ok();
    }
    
    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var (accessToken, refreshToken) = _auth.LoginUser(request);
        return Ok(new
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        });
    }    
}