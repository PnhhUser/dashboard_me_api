using Core.Responses;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    // GET: api/accounts
    [HttpGet("accounts")]
    public async Task<IActionResult> GetAll()
    {
        var accounts = await _accountService.GetAllAsync();

        return Ok(ApiResponse<IReadOnlyList<AccountModel>>.Ok(accounts));

    }

    // GET: api/account/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var account = await _accountService.GetByIdAsync(id);

        return Ok(ApiResponse<AccountModel>.Ok(account));
    }

    // GET: api/account/by-username?username=admin
    [HttpGet("by-username")]
    public async Task<IActionResult> GetByUsername([FromQuery] string username)
    {
        var account = await _accountService.GetByUsernameAsync(username);

        return Ok(ApiResponse<AccountModel>.Ok(account));
    }


    // POST: api/account
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateAccountDTO dto)
    {
        var account = await _accountService.AddAsync(dto);

        return Ok(ApiResponse<AccountModel>.Ok(account, "Account created successfully"));
    }


    // POST: api/account
    [HttpPost("edit")]
    public async Task<IActionResult> Edit([FromBody] EditAccountDTO dto)
    {
        var account = await _accountService.EditAsync(dto);

        return Ok(ApiResponse<AccountModel>.Ok(account, "Account edited successfully"));
    }

    // POST: api/account
    [HttpPost("remove/{id:int}")]
    public async Task<IActionResult> Remove(int id)
    {
        var account = await _accountService.RemoveAsync(id);

        return Ok(ApiResponse<AccountModel>.Ok(account, "Account removed successfully"));
    }
}
