using Microsoft.EntityFrameworkCore;

public class PurchaseOrderItemRepo : BaseRepo<PurchaseOrderItemEntity>, IPurchaseOrderItemRepo
{
    public PurchaseOrderItemRepo(MeContext ctx) : base(ctx) { }


}