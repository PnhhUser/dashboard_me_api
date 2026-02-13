public interface IStockRepo : IBaseRepo<StockEntity>
{
    Task<IReadOnlyList<StockEntity>> GetLatestStockAsync();
}