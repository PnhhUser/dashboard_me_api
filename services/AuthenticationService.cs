using System.IdentityModel.Tokens.Jwt;
using Core.Errors;
using Core.Exceptions;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;

public class AuthenticationService : IAuthenticationService
{
    private readonly IAccountRepo _accountRepo;
    private readonly JwtSettings _jwt;

    public AuthenticationService(
        IAccountRepo accountRepo,
        IOptions<JwtSettings> jwtOptions)
    {
        _accountRepo = accountRepo;
        _jwt = jwtOptions.Value;
    }


    private string GenerateJwtToken(int accountId, string username, RoleEnum role)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwt.Secret));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, accountId.ToString()),
        new Claim(JwtRegisteredClaimNames.UniqueName, username),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.Role, role.ToString())
    };

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwt.ExpirationInMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    public async Task<LoginModel> LoginAsync(LoginDTO dto)
    {
        var account = await _accountRepo.GetByUsernameAsync(dto.Username);

        if (account == null || !PasswordHasher.Verify(dto.Password, account.PasswordHash))
        {
            throw new AppException(
                ErrorCode.Unauthorized,
                ErrorMessage.InvalidCredentials);
        }

        var token = GenerateJwtToken(account.Id, account.Username, account.Role);

        return new LoginModel
        {
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwt.ExpirationInMinutes)
        };
    }


    public async Task<LoginModel> RegisterAsync(RegisterDTO dto)
    {
        var username = dto.Username.Trim().ToLower();

        if (username.Contains("admin"))
        {
            throw new AppException(
                ErrorCode.BadRequest,
                "Username cannot contain reserved word 'admin'.");
        }

        var existingAccount = await _accountRepo.GetByUsernameAsync(username);
        if (existingAccount != null)
        {
            throw new AppException(
                ErrorCode.Conflict,
                ErrorMessage.UsernameAlreadyExists);
        }

        var hashedPassword = PasswordHasher.Hash(dto.Password);
        var now = DateTime.UtcNow;

        var newAccount = new AccountEntity
        {
            Username = username,
            PasswordHash = hashedPassword,
            Active = ActiveEnum.Active,
            Role = RoleEnum.User,
            CreatedAt = now
        };

        await _accountRepo.AddAsync(newAccount);
        await _accountRepo.SaveAsync();


        // 🔥 Generate token ngay sau khi tạo
        var token = GenerateJwtToken(
            newAccount.Id,
            newAccount.Username,
            newAccount.Role);

        var expiry = now.AddMinutes(_jwt.ExpirationInMinutes);

        return new LoginModel
        {
            Token = token,
            ExpiresAt = expiry
        };
    }

    public async Task LogoutAsync(int accountId)
    {
        // In a real-world application, you might want to implement token blacklisting
        // or other mechanisms to invalidate the token on logout.
        await Task.CompletedTask;
    }
}