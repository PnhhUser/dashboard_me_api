public interface IAccountService
{
    Task<IReadOnlyList<AccountModel>> GetAllAsync();

    Task<AccountModel> GetByIdAsync(int id);

    Task<AccountModel> GetByUsernameAsync(string username);

    Task<AccountModel> AddAsync(CreateAccountDTO dto);

    Task<AccountModel> EditAsync(EditAccountDTO dto);

    Task<AccountModel> RemoveAsync(int id);
}