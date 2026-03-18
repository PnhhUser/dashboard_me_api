using Core.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IProductImageService _productImageService;

    public ProductController(
        IProductService productService,
        IProductImageService productImageService)
    {
        _productService = productService;
        _productImageService = productImageService;
    }

    // ===================== PRODUCT =====================

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllAsync();
        return Ok(ApiResponse<IReadOnlyList<ProductModel>>.Ok(products));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        return Ok(ApiResponse<ProductModel>.Ok(product));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductDTO dto)
    {
        var product = await _productService.AddAsync(dto);
        return Ok(ApiResponse<ProductModel>.Ok(
            product,
            ResponsesMessage.CreatedSuccessfully("Product")
        ));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] EditProductDTO dto)
    {
        dto.Id = id;
        var product = await _productService.EditAsync(dto);

        return Ok(ApiResponse<ProductModel>.Ok(
            product,
            ResponsesMessage.EditedSuccessfully("Product")
        ));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _productService.RemoveAsync(id);

        return Ok(ApiResponse<ProductModel>.Ok(
            product,
            ResponsesMessage.RemovedSuccessfully("Product")
        ));
    }

    // ===================== PRODUCT IMAGES =====================

    /// <summary>
    /// Upload multiple images
    /// </summary>
    [HttpPost("{productId:int}/images")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadImages(
        int productId,
        [FromForm] List<IFormFile> files)
    {
        await _productImageService.UploadImagesAsync(productId, files);

        return Ok(ApiResponse<string>.Ok(
            "Upload successful",
            ResponsesMessage.CreatedSuccessfully("Product images")
        ));
    }

    /// <summary>
    /// Get all images
    /// </summary>
    [HttpGet("{productId:int}/images")]
    public async Task<IActionResult> GetImages(int productId)
    {
        var result = await _productImageService.GetImagesByProductId(productId);

        return Ok(ApiResponse<object>.Ok(result));
    }

    /// <summary>
    /// Get thumbnail
    /// </summary>
    [HttpGet("{productId:int}/images/thumbnail")]
    public async Task<IActionResult> GetThumbnail(int productId)
    {
        var result = await _productImageService.GetThumbnailAsync(productId);

        return Ok(ApiResponse<object>.Ok(result));
    }

    /// <summary>
    /// Set thumbnail
    /// </summary>
    [HttpPut("{productId:int}/images/thumbnail")]
    public async Task<IActionResult> SetThumbnail(
        int productId,
        [FromBody] SetProductThumbnailDto dto)
    {
        await _productImageService.SetThumbnailAsync(productId, dto.DisplayOrder);

        return Ok(ApiResponse<string>.Ok(
            "Thumbnail updated",
            ResponsesMessage.EditedSuccessfully("Product image")
        ));
    }

    /// <summary>
    /// Update single image
    /// </summary>
    [HttpPut("{productId:int}/images/{displayOrder:int}")]
    public async Task<IActionResult> UpdateImage(
        int productId,
        int displayOrder,
        IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File is required.");

        await _productImageService.ChangeImageAsync(productId, displayOrder, file);

        return Ok(ApiResponse<string>.Ok(
            "Image updated",
            ResponsesMessage.EditedSuccessfully("Product image")
        ));
    }

    /// <summary>
    /// Update multiple images
    /// </summary>
    [HttpPut("{productId:int}/images")]
    public async Task<IActionResult> UpdateImages(
        int productId,
        [FromForm] List<ProductImageUploadDto> images)
    {
        await _productImageService.ChangeImagesAsync(productId, images);

        return Ok(ApiResponse<string>.Ok(
            "Images updated",
            ResponsesMessage.EditedSuccessfully("Product image")
        ));
    }

    /// <summary>
    /// Delete image
    /// </summary>
    [HttpDelete("{productId:int}/images/{displayOrder:int}")]
    public async Task<IActionResult> DeleteImage(
        int productId,
        int displayOrder)
    {
        await _productImageService.RemoveImage(productId, displayOrder);

        return Ok(ApiResponse<string>.Ok(
            "Image deleted",
            ResponsesMessage.RemovedSuccessfully("Product image")
        ));
    }
}