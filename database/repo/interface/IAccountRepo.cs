public interface IAccountRepo : IBaseRepo<AccountEntity>
{
    Task<AccountEntity?> GetByUsernameAsync(string username);
}