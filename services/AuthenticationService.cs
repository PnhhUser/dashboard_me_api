using System.IdentityModel.Tokens.Jwt;
using Core.Errors;
using Core.Exceptions;
using Core.Utils;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;

public class AuthenticationService : IAuthenticationService
{
    private readonly IAccountRepo _accountRepo;
    private readonly JwtSettings _jwt;

    private readonly IRefreshTokenRepo _refreshTokenRepo;

    public AuthenticationService(
        IAccountRepo accountRepo,
        IOptions<JwtSettings> jwtOptions,
        IRefreshTokenRepo refreshTokenRepo)
    {
        _accountRepo = accountRepo;
        _jwt = jwtOptions.Value;
        _refreshTokenRepo = refreshTokenRepo;
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
                new Claim(ClaimTypes.NameIdentifier, accountId.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
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


    public async Task<AuthModel> LoginAsync(LoginDTO dto)
    {
        // make lookup consistent with registration/login
        var username = Core.Utils.StringHelper.NormalizeUsername(dto.Username);
        var account = await _accountRepo.GetByUsernameAsync(username);

        if (account == null || !PasswordHasher.Verify(dto.Password, account.PasswordHash))
        {
            throw new AppException(
                ErrorCode.Unauthorized,
                ErrorMessage.InvalidCredentials);
        }

        var token = GenerateJwtToken(account.Id, account.Username, account.Role);

        var refreshToken = GenerateRefreshToken();

        var refreshTokenEntity = new RefreshTokenEntity
        {
            Token = refreshToken,
            AccountId = account.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        await _refreshTokenRepo.AddAsync(refreshTokenEntity);
        await _refreshTokenRepo.SaveAsync();

        return new AuthModel
        {
            AccessToken = token,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwt.ExpirationInMinutes)
        };
    }

    private string GenerateRefreshToken()
    {
        var bytes = new byte[64];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(bytes);

        return Convert.ToBase64String(bytes);
    }


    public async Task<AuthModel> RegisterAsync(RegisterDTO dto)
    {
        var username = Core.Utils.StringHelper.NormalizeUsername(dto.Username);

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

        var refreshToken = GenerateRefreshToken();

        var refreshTokenEntity = new RefreshTokenEntity
        {
            Token = refreshToken,
            AccountId = newAccount.Id,
            ExpiresAt = now.AddDays(7)
        };


        await _refreshTokenRepo.AddAsync(refreshTokenEntity);
        await _refreshTokenRepo.SaveAsync();

        return new AuthModel
        {
            AccessToken = token,
            RefreshToken = refreshToken,
            ExpiresAt = expiry
        };
    }

    public async Task LogoutAsync(int accountId, string refreshToken)
    {
        var tokenEntity = await _refreshTokenRepo.GetByTokenAsync(refreshToken);

        if (tokenEntity == null)
            return;

        if (tokenEntity.AccountId != accountId)
        {
            throw new AppException(
                ErrorCode.Unauthorized,
                "Unauthorized logout attempt");
        }

        if (tokenEntity.IsRevoked)
        {
            throw new AppException(
            ErrorCode.Unauthorized,
            "Refresh token already revoked");
        }

        if (tokenEntity.ExpiresAt < DateTime.UtcNow)
            return;

        tokenEntity.IsRevoked = true;
        tokenEntity.RevokedAt = DateTime.UtcNow;

        await _refreshTokenRepo.SaveAsync();
    }


    public async Task<AuthModel> RefreshAsync(string refreshToken)
    {
        var tokenEntity = await _refreshTokenRepo.GetByTokenAsync(refreshToken);

        if (tokenEntity == null)
        {
            throw new AppException(
                ErrorCode.Unauthorized,
                "Invalid refresh token");
        }

        if (tokenEntity.IsRevoked)
        {
            throw new AppException(
                ErrorCode.Unauthorized,
                "Refresh token revoked");
        }

        if (tokenEntity.ExpiresAt < DateTime.UtcNow)
        {
            throw new AppException(
                ErrorCode.Unauthorized,
                "Refresh token expired");
        }

        var account = await _accountRepo.GetByIdAsync(tokenEntity.AccountId);

        if (account == null)
        {
            throw new AppException(
                ErrorCode.Unauthorized,
                "Account not found");
        }

        // revoke token cũ
        tokenEntity.IsRevoked = true;
        tokenEntity.RevokedAt = DateTime.UtcNow;

        // tạo token mới
        var newAccessToken = GenerateJwtToken(
            account.Id,
            account.Username,
            account.Role);

        var newRefreshToken = GenerateRefreshToken();

        var newRefreshEntity = new RefreshTokenEntity
        {
            Token = newRefreshToken,
            AccountId = account.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        await _refreshTokenRepo.AddAsync(newRefreshEntity);
        await _refreshTokenRepo.SaveAsync();

        return new AuthModel
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwt.ExpirationInMinutes)
        };
    }

    public Task<AuthUserModel> CheckAuthAsync(ClaimsPrincipal user)
    {
        var id = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var username = user.FindFirstValue(ClaimTypes.Name)!;
        var role = Enum.Parse<RoleEnum>(user.FindFirstValue(ClaimTypes.Role)!);

        var result = new AuthUserModel
        {
            Id = id,
            Username = username,
            Role = role
        };

        return Task.FromResult(result);
    }
}