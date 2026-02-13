using Core.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Account management endpoints
/// </summary>
[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    /// <summary>
    /// Get all accounts
    /// </summary>
    /// <returns>List of all accounts</returns>
    [HttpGet("list")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<AccountModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var accounts = await _accountService.GetAllAsync();

        return Ok(ApiResponse<IReadOnlyList<AccountModel>>.Ok(accounts));
    }

    /// <summary>
    /// Get account by ID
    /// </summary>
    /// <param name="id">Account ID</param>
    /// <returns>Account details</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<AccountModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var account = await _accountService.GetByIdAsync(id);

        return Ok(ApiResponse<AccountModel>.Ok(account));
    }

    /// <summary>
    /// Get account by username
    /// </summary>
    /// <param name="username">Username to search</param>
    /// <returns>Account details</returns>
    [HttpGet("by-username")]
    [ProducesResponseType(typeof(ApiResponse<AccountModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByUsername([FromQuery] string username)
    {
        var account = await _accountService.GetByUsernameAsync(username);

        return Ok(ApiResponse<AccountModel>.Ok(account));
    }

    /// <summary>
    /// Create a new account
    /// </summary>
    /// <param name="dto">Account creation data</param>
    /// <returns>Created account</returns>
    [HttpPost("create")]
    [ProducesResponseType(typeof(ApiResponse<AccountModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateAccountDTO dto)
    {
        var account = await _accountService.AddAsync(dto);

        return Ok(ApiResponse<AccountModel>.Ok(account, ResponsesMessage.CreatedSuccessfully("Account")));
    }

    /// <summary>
    /// Edit an account
    /// </summary>
    /// <param name="dto">Account edit data</param>
    /// <returns>Updated account</returns>
    [HttpPut("edit")]
    [ProducesResponseType(typeof(ApiResponse<AccountModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Edit([FromBody] EditAccountDTO dto)
    {
        var account = await _accountService.EditAsync(dto);

        return Ok(ApiResponse<AccountModel>.Ok(account, ResponsesMessage.EditedSuccessfully("Account")));
    }

    /// <summary>
    /// Delete (soft delete) an account
    /// </summary>
    /// <param name="id">Account ID</param>
    /// <returns>Deleted account</returns>
    [HttpDelete("remove/{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<AccountModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove(int id)
    {
        var account = await _accountService.RemoveAsync(id);

        return Ok(ApiResponse<AccountModel>.Ok(account, ResponsesMessage.RemovedSuccessfully("Account")));
    }
}
