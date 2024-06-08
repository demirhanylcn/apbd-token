using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using token.Contracts;
using token.Exception;
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
        try
        {
            _auth.RegisterUser(request);
            return Ok();
        }
        catch (WeakPasswordException exception)
        {
            return BadRequest(exception.Message);
        }
        catch (UserWithMailExistsException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        try
        {
            var (accessToken, refreshToken) = _auth.LoginUser(request);
            return Ok(new
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }
        catch (ThiefExeption exeption)
        {
            return BadRequest(exeption.Message);
        }
        catch (UserDoesntExistsException exception)
        {
            return BadRequest(exception.Message);
        }
        
    }

    [HttpGet]
    [Route("getNewAccessToken")]
    [AllowAnonymous]
    public async Task<IActionResult> NewAccessToken(string refleshToken)
    {
        try
        {
            var result = await _auth.GetNewAccessToken(refleshToken);
            return Ok(result);
        }
        catch (UserDoesntExistsException exception)
        {
            return BadRequest(exception.Message);
        }
        
    }
}