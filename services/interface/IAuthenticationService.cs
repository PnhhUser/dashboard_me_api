public interface IAuthenticationService
{
    Task<LoginModel> RegisterAsync(RegisterDTO dto);

    Task<LoginModel> LoginAsync(LoginDTO dto);

    Task LogoutAsync(int accountId);

    // Task<bool> IsUsernameAvailableAsync(string username);

    // Task<bool> ValidateTokenAsync(string token);

    // Task<int?> GetAccountIdFromTokenAsync(string token);

    // Task<bool> ChangePasswordAsync(int accountId, string currentPassword, string newPassword);

    // Task<bool> ResetPasswordAsync(string username, string newPassword);

    // Task<bool> ActivateAccountAsync(int accountId);

    // Task<bool> DeactivateAccountAsync(int accountId);

    // Task<RoleEnum> GetUserRolesAsync(int accountId);
}