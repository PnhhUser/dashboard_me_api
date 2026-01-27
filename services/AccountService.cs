using Core.Errors;
using Core.Exceptions;

public class AccountService : IAccountService
{
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
        var account = await _accountRepo.GetByUsernameAsync(username);

        if (account == null || account.DeletedAt != null)
        {
            throw new AppException(ErrorCode.NotFound, ErrorMessage.AccountNotFound);
        }

        return AccountModel.ToModel(account);
    }

    // ---------- CREATE ----------
    public async Task<AccountModel> AddAsync(CreateAccountDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Username))
        {
            throw new AppException(
                ErrorCode.ValidationError,
                ErrorMessage.UsernameIsRequired);
        }

        if (dto.Username.Length <= 3)
        {
            throw new AppException(
                ErrorCode.ValidationError,
                ErrorMessage.UsernameMinLength);
        }

        if (string.IsNullOrWhiteSpace(dto.Password))
        {
            throw new AppException(ErrorCode.ValidationError, ErrorMessage.PasswordIsRequired);
        }

        if (dto.Password.Length <= 3)
        {
            throw new AppException(
                ErrorCode.ValidationError,
                ErrorMessage.PasswordMinLength);
        }

        var existed = await _accountRepo.GetByUsernameAsync(dto.Username);

        if (existed != null)
        {
            throw new AppException(
                ErrorCode.ValidationError,
                ErrorMessage.UsernameAlreadyExists
            );
        }

        var entity = new AccountEntity
        {
            Username = dto.Username,
            PasswordHash = PasswordHasher.Hash(dto.Password),
            Active = dto.Active,
            CreatedAt = DateTime.UtcNow
        };

        await _accountRepo.AddAsync(entity);
        await _accountRepo.SaveAsync();

        return AccountModel.ToModel(entity);
    }

    // ---------- EDIT ----------
    public async Task<AccountModel> EditAsync(EditAccountDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Username))
        {
            throw new AppException(
                ErrorCode.ValidationError,
                ErrorMessage.UsernameIsRequired);
        }

        if (dto.Username.Length <= 3)
        {
            throw new AppException(
                ErrorCode.ValidationError,
                ErrorMessage.UsernameMinLength);
        }

        if (string.IsNullOrWhiteSpace(dto.Password))
        {
            throw new AppException(ErrorCode.ValidationError, ErrorMessage.PasswordIsRequired);
        }

        if (dto.Password.Length <= 3)
        {
            throw new AppException(
                ErrorCode.ValidationError,
                ErrorMessage.PasswordMinLength);
        }

        var existed = await _accountRepo.GetByIdAsync(dto.Id);

        if (existed == null)
        {
            throw new AppException(
                ErrorCode.NotFound,
                ErrorMessage.AccountNotFound
            );
        }

        var duplicated = await _accountRepo.GetByUsernameAsync(dto.Username);

        if (duplicated != null && duplicated.Id != existed.Id)
        {
            throw new AppException(
                ErrorCode.ValidationError,
                ErrorMessage.UsernameAlreadyExists
            );
        }

        existed.Username = dto.Username;
        existed.PasswordHash = PasswordHasher.Hash(dto.Password);
        existed.Active = dto.Active;
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

        existed.UpdatedAt = DateTime.UtcNow;
        existed.DeletedAt = DateTime.UtcNow;

        await _accountRepo.SaveAsync();

        return AccountModel.ToModel(existed);
    }
}