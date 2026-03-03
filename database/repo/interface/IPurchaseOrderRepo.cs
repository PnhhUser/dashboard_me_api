public interface IPurchaseOrderRepo : IBaseRepo<PurchaseOrderEntity>
{
    Task<PurchaseOrderEntity?> GetByCodeAsync(string code);

    Task<bool> ExistsAsync(int id);
}