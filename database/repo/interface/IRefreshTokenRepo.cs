public interface IRefreshTokenRepo : IBaseRepo<RefreshTokenEntity>
{
    Task<RefreshTokenEntity?> GetByTokenAsync(string refreshToken);
}