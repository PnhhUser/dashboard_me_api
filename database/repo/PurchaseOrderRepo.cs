using Microsoft.EntityFrameworkCore;

public class PurchaseOrderRepo : BaseRepo<PurchaseOrderEntity>, IPurchaseOrderRepo
{
    public PurchaseOrderRepo(MeContext ctx) : base(ctx) { }


    public async Task<PurchaseOrderEntity?> GetByCodeAsync(string code)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Code == code);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _dbSet
            .AnyAsync(x => x.Id == id);
    }
}