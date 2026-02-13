using Microsoft.EntityFrameworkCore;

public class StockRepo : BaseRepo<StockEntity>, IStockRepo
{
    public StockRepo(MeContext ctx) : base(ctx) { }


    public async Task<IReadOnlyList<StockEntity>> GetLatestStockAsync()
    {
        return await _dbSet
         .GroupBy(x => x.ProductId)
         .Select(g => g
             .OrderByDescending(x => x.CreatedAt)
             .ThenByDescending(x => x.Id)
             .First())
         .ToListAsync();
    }
}