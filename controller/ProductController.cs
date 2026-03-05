using Core.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IProductImageService _productImageService;

    public ProductController(IProductService productService, IProductImageService productImageService)
    {
        _productService = productService;
        _productImageService = productImageService;
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

    /// <summary>
    /// Upload multiple images for a product
    /// </summary>
    /// <param name="dto">Upload images data</param>
    /// <returns>Success response</returns>
    [HttpPost("upload-images")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UploadImages([FromForm] UploadProductImagesDto dto)
    {
        await _productImageService.UploadImagesAsync(dto);

        return Ok(ApiResponse<string>.Ok(
            "Upload successful",
            ResponsesMessage.CreatedSuccessfully("Product images")
        ));
    }

    [HttpPut("set-thumbnail")]
    public async Task<IActionResult> SetThumbnail([FromBody] SetProductThumbnailDto dto)
    {
        await _productImageService
            .SetThumbnailAsync(dto.ProductId, dto.DisplayOrder);

        return Ok(new
        {
            message = "Thumbnail updated successfully."
        });
    }

    [HttpGet("{productId}/images")]
    public async Task<IActionResult> GetImagesByProductId(int productId)
    {
        var result = await _productImageService
            .GetImagesByProductId(productId);

        return Ok(result);
    }


    [HttpGet("{productId}/thumbnail")]
    public async Task<IActionResult> GetThumbnail(int productId)
    {
        var result = await _productImageService
            .GetThumbnailAsync(productId);

        return Ok(result);
    }


    /// <summary>
    /// Thay đổi ảnh theo displayOrder
    /// </summary>
    [HttpPut("{displayOrder:int}")]
    public async Task<IActionResult> ChangeImage(
        int productId,
        int displayOrder,
        IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File is required.");

        await _productImageService.ChangeImageAsync(productId, displayOrder, file);

        return Ok(new
        {
            message = "Changed image successfully."
        });
    }


    /// <summary>
    /// Xóa ảnh theo displayOrder
    /// </summary>
    [HttpDelete("{displayOrder:int}")]
    public async Task<IActionResult> RemoveImage(
        int productId,
        int displayOrder)
    {
        await _productImageService.RemoveImage(productId, displayOrder);

        return Ok(new
        {
            message = "Remove image successfully."
        });
    }


}
