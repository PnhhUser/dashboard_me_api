public class StockRepo : BaseRepo<StockEntity>, IStockRepo
{
    public StockRepo(MeContext ctx) : base(ctx) { }
}