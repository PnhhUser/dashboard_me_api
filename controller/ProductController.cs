using Core.Responses;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// Get all products
    /// </summary>
    /// <returns>List of all products</returns>
    [HttpGet("list")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ProductModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllAsync();

        return Ok(ApiResponse<IReadOnlyList<ProductModel>>.Ok(products));
    }

    /// <summary>
    /// Get product by ID
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <returns>Product details</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<ProductModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);

        return Ok(ApiResponse<ProductModel>.Ok(product));
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    /// <param name="dto">Product creation data</param>
    /// <returns>Created product</returns>
    [HttpPost("create")]
    [ProducesResponseType(typeof(ApiResponse<ProductModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateProductDTO dto)
    {
        var product = await _productService.AddAsync(dto);

        return Ok(ApiResponse<ProductModel>.Ok(product, ResponsesMessage.CreatedSuccessfully("Product")));
    }

    /// <summary>
    /// Edit existing product
    /// </summary>
    /// <param name="dto">Product edit data</param>
    /// <returns>Updated product</returns>
    [HttpPut("edit")]
    [ProducesResponseType(typeof(ApiResponse<ProductModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Edit([FromBody] EditProductDTO dto)
    {
        var product = await _productService.EditAsync(dto);

        return Ok(ApiResponse<ProductModel>.Ok(product, ResponsesMessage.EditedSuccessfully("Product")));
    }

    /// <summary>
    /// Delete (soft delete) a product
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <returns>Deleted product</returns>
    [HttpDelete("remove/{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<ProductModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove(int id)
    {
        var product = await _productService.RemoveAsync(id);

        return Ok(ApiResponse<ProductModel>.Ok(product, ResponsesMessage.RemovedSuccessfully("Product")));
    }
}
