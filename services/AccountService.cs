using Core.Errors;
using Core.Exceptions;
using Core.Utils;

public class AccountService : IAccountService
{
    private const int USERNAME_MIN_LEN = 3;
    private const int PASSWORD_MIN_LEN = 6;
    private readonly IAccountRepo _accountRepo;
    public AccountService(IAccountRepo accountRepo)
    {
        _accountRepo = accountRepo;
    }

    // ---------- READ ----------
    public async Task<IReadOnlyList<AccountModel>> GetAllAsync()
    {
        var accounts = await _accountRepo.GetAllAsync();

        return accounts.Select(AccountModel.ToModel).ToList();
    }

    public async Task<AccountModel> GetByIdAsync(int id)
    {
        var account = await _accountRepo.GetByIdAsync(id);

        if (account == null)
        {
            throw new AppException(ErrorCode.NotFound, ErrorMessage.AccountNotFound);
        }

        return AccountModel.ToModel(account);
    }

    public async Task<AccountModel> GetByUsernameAsync(string username)
    {
        var normalized = Core.Utils.StringHelper.NormalizeUsername(username);
        var account = await _accountRepo.GetByUsernameAsync(normalized);

        if (account == null)
        {
            throw new AppException(ErrorCode.NotFound, ErrorMessage.AccountNotFound);
        }

        return AccountModel.ToModel(account);
    }

    // ---------- CREATE ----------
    public async Task<AccountModel> AddAsync(CreateAccountDTO dto)
    {
        // normalize and validate username
        var username = Core.Utils.StringHelper.NormalizeUsername(dto.Username);
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new AppException(
                ErrorCode.ValidationError,
                ErrorMessage.UsernameIsRequired);
        }

        if (username.Length < USERNAME_MIN_LEN)
        {
            throw new AppException(
                ErrorCode.ValidationError,
                ErrorMessage.UsernameMinLength);
        }

        if (username.Contains("admin"))
        {
            throw new AppException(
                ErrorCode.ValidationError,
                "Username cannot contain reserved word 'admin'.");
        }

        if (string.IsNullOrWhiteSpace(dto.Password))
        {
            throw new AppException(ErrorCode.ValidationError, ErrorMessage.PasswordIsRequired);
        }

        if (dto.Password.Length < PASSWORD_MIN_LEN)
        {
            throw new AppException(
                ErrorCode.ValidationError,
                ErrorMessage.PasswordMinLength);
        }

        if (!Enum.IsDefined(typeof(RoleEnum), dto.Role))
        {
            throw new AppException(ErrorCode.ValidationError, ErrorMessage.RoleInvalid);
        }

        var existed = await _accountRepo.GetByUsernameAsync(username);

        if (existed != null)
        {
            throw new AppException(
                ErrorCode.ValidationError,
                ErrorMessage.UsernameAlreadyExists
            );
        }

        var entity = new AccountEntity
        {
            Username = username,
            PasswordHash = PasswordHasher.Hash(dto.Password),
            Active = dto.Active,
            Role = dto.Role,
            CreatedAt = DateTime.UtcNow
        };

        await _accountRepo.AddAsync(entity);
        await _accountRepo.SaveAsync();

        return AccountModel.ToModel(entity);
    }

    // ---------- EDIT ----------
    public async Task<AccountModel> EditAsync(EditAccountDTO dto)
    {
        var username = Core.Utils.StringHelper.NormalizeUsername(dto.Username);
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new AppException(
                ErrorCode.ValidationError,
                ErrorMessage.UsernameIsRequired);
        }

        if (username.Length < USERNAME_MIN_LEN)
        {
            throw new AppException(
                ErrorCode.ValidationError,
                ErrorMessage.UsernameMinLength);
        }

        if (username.Contains("admin"))
        {
            throw new AppException(
                ErrorCode.ValidationError,
                "Username cannot contain reserved word 'admin'.");
        }

        if (string.IsNullOrWhiteSpace(dto.Password))
        {
            throw new AppException(ErrorCode.ValidationError, ErrorMessage.PasswordIsRequired);
        }

        if (dto.Password.Length < PASSWORD_MIN_LEN)
        {
            throw new AppException(
                ErrorCode.ValidationError,
                ErrorMessage.PasswordMinLength);
        }

        if (!Enum.IsDefined(typeof(RoleEnum), dto.Role))
        {
            throw new AppException(ErrorCode.ValidationError, ErrorMessage.RoleInvalid);
        }

        var existed = await _accountRepo.GetByIdAsync(dto.Id);

        if (existed == null)
        {
            throw new AppException(
                ErrorCode.NotFound,
                ErrorMessage.AccountNotFound
            );
        }

        var duplicated = await _accountRepo.GetByUsernameAsync(username);

        if (duplicated != null && duplicated.Id != existed.Id)
        {
            throw new AppException(
                ErrorCode.ValidationError,
                ErrorMessage.UsernameAlreadyExists
            );
        }

        existed.Username = username;
        existed.PasswordHash = PasswordHasher.Hash(dto.Password);
        existed.Active = dto.Active;
        existed.Role = dto.Role;
        existed.UpdatedAt = DateTime.UtcNow;

        await _accountRepo.SaveAsync();

        return AccountModel.ToModel(existed);
    }

    // ---------- REMOVE ----------
    public async Task<AccountModel> RemoveAsync(int id)
    {
        var existed = await _accountRepo.GetByIdAsync(id);

        if (existed == null)
        {
            throw new AppException(
                ErrorCode.NotFound,
                ErrorMessage.AccountNotFound
            );
        }

        _accountRepo.SoftDelete(existed);

        await _accountRepo.SaveAsync();

        return AccountModel.ToModel(existed);
    }
}