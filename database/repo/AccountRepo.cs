using Microsoft.EntityFrameworkCore;

public class AccountRepo : BaseRepo<AccountEntity>, IAccountRepo
{
    public AccountRepo(MeContext ctx) : base(ctx) { }


    public async Task<AccountEntity?> GetByUsernameAsync(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return null;
        }

        var lowerUsername = username.ToLowerInvariant();
        return await _dbSet.FirstOrDefaultAsync(x => x.Username.ToLower() == lowerUsername && x.DeletedAt == null);
    }
}