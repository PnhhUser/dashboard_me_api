using System.Security.Claims;

public interface IAuthenticationService
{
    Task<AuthModel> RegisterAsync(RegisterDTO dto);

    Task<AuthModel> LoginAsync(LoginDTO dto);

    Task LogoutAsync(int accountId, string refreshToken);

    Task<AuthModel> RefreshAsync(string refreshToken);

    Task<AuthUserModel> CheckAuthAsync(ClaimsPrincipal user);

    // Task<bool> IsUsernameAvailableAsync(string username);

    // Task<bool> ValidateTokenAsync(string token);

    // Task<int?> GetAccountIdFromTokenAsync(string token);

    // Task<bool> ChangePasswordAsync(int accountId, string currentPassword, string newPassword);

    // Task<bool> ResetPasswordAsync(string username, string newPassword);

    // Task<bool> ActivateAccountAsync(int accountId);

    // Task<bool> DeactivateAccountAsync(int accountId);

    // Task<RoleEnum> GetUserRolesAsync(int accountId);
}