using Microsoft.EntityFrameworkCore;

public class RefreshTokenRepo : BaseRepo<RefreshTokenEntity>, IRefreshTokenRepo
{
    public RefreshTokenRepo(MeContext ctx) : base(ctx) { }


    public async Task<RefreshTokenEntity?> GetByTokenAsync(string refreshToken)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Token == refreshToken);
    }
}