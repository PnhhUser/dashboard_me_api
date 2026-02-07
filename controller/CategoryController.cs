using Core.Responses;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Category management endpoints
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    /// <summary>
    /// Get all categories
    /// </summary>
    /// <returns>List of all categories</returns>
    [HttpGet("list")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<CategoryModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _categoryService.GetAllAsync();

        return Ok(ApiResponse<IReadOnlyList<CategoryModel>>.Ok(categories));
    }

    /// <summary>
    /// Get category by ID
    /// </summary>
    /// <param name="id">Category ID</param>
    /// <returns>Category details</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<CategoryModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);

        return Ok(ApiResponse<CategoryModel>.Ok(category));
    }

    /// <summary>
    /// Create a new category
    /// </summary>
    /// <param name="dto">Category creation data</param>
    /// <returns>Created category</returns>
    [HttpPost("create")]
    [ProducesResponseType(typeof(ApiResponse<CategoryModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCategoryDTO dto)
    {
        var category = await _categoryService.AddAsync(dto);

        return Ok(ApiResponse<CategoryModel>.Ok(
            category,
            ResponsesMessage.CreatedSuccessfully("Category")));
    }

    /// <summary>
    /// Edit a category
    /// </summary>
    /// <param name="dto">Category edit data</param>
    /// <returns>Updated category</returns>
    [HttpPut("edit")]
    [ProducesResponseType(typeof(ApiResponse<CategoryModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Edit([FromBody] EditCategoryDTO dto)
    {
        var category = await _categoryService.EditAsync(dto);

        return Ok(ApiResponse<CategoryModel>.Ok(
            category,
            ResponsesMessage.EditedSuccessfully("Category")));
    }

    /// <summary>
    /// Delete (soft delete) a category
    /// </summary>
    /// <param name="id">Category ID</param>
    /// <returns>Deleted category</returns>
    [HttpDelete("remove/{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<CategoryModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove(int id)
    {
        var category = await _categoryService.RemoveAsync(id);

        return Ok(ApiResponse<CategoryModel>.Ok(
           category,
           ResponsesMessage.RemovedSuccessfully("Category")));
    }
}


