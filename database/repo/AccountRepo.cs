using Microsoft.EntityFrameworkCore;

public class AccountRepo : BaseRepo<AccountEntity>, IAccountRepo
{
    public AccountRepo(MeContext context) : base(context) { }


    public async Task<AccountEntity?> GetByUsernameAsync(string username)
    {
        if (username != username.ToLowerInvariant())
        {
            return null;
        }

        return await _dbSet.FirstOrDefaultAsync(x => x.Username == username);
    }
}