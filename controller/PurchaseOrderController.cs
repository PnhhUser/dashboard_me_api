using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class PurchaseOrderController : ControllerBase
{
    public PurchaseOrderController() { }
}