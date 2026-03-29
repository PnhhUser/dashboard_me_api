using System.Security.Claims;
using Core.Responses;
using Microsoft.AspNetCore.Authorization;
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
    [ProducesResponseType(typeof(ApiResponse<AuthModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
    {
        var result = await _authService.RegisterAsync(dto);

        Response.Cookies.Append("refreshToken", result.RefreshToken, RefreshCookieOptions());

        return Ok(ApiResponse<AuthModel>.Ok(result));
    }


    /// <summary>
    /// User login endpoint
    /// </summary>
    /// <param name="dto">Login data</param>
    /// <returns>JWT token and expiration</returns>
    /// 
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<AuthModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        var result = await _authService.LoginAsync(dto);

        Response.Cookies.Append("refreshToken", result.RefreshToken, RefreshCookieOptions());

        return Ok(ApiResponse<AuthModel>.Ok(result));
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var accountId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var refreshToken = Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(refreshToken))
            return Unauthorized();

        await _authService.LogoutAsync(accountId, refreshToken);

        Response.Cookies.Delete("refreshToken", RefreshCookieOptions());

        return Ok(ApiResponse.Ok());
    }


    // [HttpPost("refresh-token")]
    // [ProducesResponseType(typeof(ApiResponse<AuthModel>), StatusCodes.Status200OK)]
    // public async Task<IActionResult> Refresh([FromBody] RefreshTokenDTO dto)
    // {
    //     var result = await _authService.RefreshAsync(dto.RefreshToken);

    //     return Ok(ApiResponse<AuthModel>.Ok(result));
    // }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(refreshToken))
            return Unauthorized();

        var result = await _authService.RefreshAsync(refreshToken);

        if (result == null)
        {
            Response.Cookies.Delete("refreshToken");
            return Unauthorized();
        }

        Response.Cookies.Append("refreshToken", result.RefreshToken, RefreshCookieOptions());

        return Ok(ApiResponse<AuthModel>.Ok(result));
    }

    [Authorize]
    [HttpGet("check-auth")]
    [ProducesResponseType(typeof(ApiResponse<AuthUserModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CheckAuth()
    {
        var result = await _authService.CheckAuthAsync(User);

        return Ok(ApiResponse<AuthUserModel>.Ok(result));
    }


    private CookieOptions RefreshCookieOptions()
    {
        var isProd = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production";

        return new CookieOptions
        {
            HttpOnly = true,
            Secure = isProd, // 🔥 Prod = true, Local = false
            SameSite = isProd ? SameSiteMode.None : SameSiteMode.Lax,
            Path = "/",

            // 🔥 KHÔNG set Domain nếu không cần
            // Domain = "localhost" ❌ bỏ đi
        };
    }
}