using Core.Responses;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authService;
    public AuthenticationController(IAuthenticationService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// User registration endpoint
    /// </summary>
    /// <param name="dto">Registration data</param>
    /// <returns>Registered user details</returns>
    ///     
    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<LoginModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
    {
        var result = await _authService.RegisterAsync(dto);
        return Ok(ApiResponse<LoginModel>.Ok(result));
    }


    /// <summary>
    /// User login endpoint
    /// </summary>
    /// <param name="dto">Login data</param>
    /// <returns>JWT token and expiration</returns>
    /// 
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<LoginModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        var result = await _authService.LoginAsync(dto);
        return Ok(ApiResponse<LoginModel>.Ok(result));
    }

    /// <summary>
    /// User logout endpoint
    /// </summary>
    /// <param name="accountId">Account ID</param>
    /// <returns>Action result</returns>
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout([FromBody] int accountId)
    {
        await _authService.LogoutAsync(accountId);
        return Ok();
    }
}