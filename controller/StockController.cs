using Core.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class StockController : ControllerBase
{
    private readonly IStockService _stockService;

    public StockController(IStockService stockService)
    {
        _stockService = stockService;
    }

    /// <summary>
    /// Get all stock entries
    /// </summary>
    /// <returns>List of all stock entries</returns>
    [HttpGet("list")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<StockModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var stocks = await _stockService.GetAllAsync();

        return Ok(ApiResponse<IReadOnlyList<StockModel>>.Ok(stocks));
    }

    /// <summary>
    /// Get stock entry by ID
    /// </summary>
    /// <param name="id">Stock ID</param>
    /// <returns>Stock details</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<StockModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var stock = await _stockService.GetByIdAsync(id);

        return Ok(ApiResponse<StockModel>.Ok(stock));
    }

    /// <summary>
    /// Get latest stock entries for all products

    [HttpGet("latest")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<StockModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLatestStock()
    {
        var stocks = await _stockService.GetLatestStockAsync();
        return Ok(ApiResponse<IReadOnlyList<StockModel>>.Ok(stocks));
    }

    /// <summary>
    /// Create a new stock entry
    /// </summary>
    /// <param name="dto">Stock creation data</param>
    /// <returns>Created stock</returns>
    [HttpPost("create")]
    [ProducesResponseType(typeof(ApiResponse<StockModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateStockDTO dto)
    {
        var stock = await _stockService.AddAsync(dto);

        return Ok(ApiResponse<StockModel>.Ok(stock, ResponsesMessage.CreatedSuccessfully("Stock")));
    }

    /// <summary>
    /// Edit a stock entry
    /// </summary>
    /// <param name="dto">Stock edit data</param>
    /// <returns>Updated stock</returns>
    [HttpPut("edit")]
    [ProducesResponseType(typeof(ApiResponse<StockModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Edit([FromBody] EditStockDTO dto)
    {
        var stock = await _stockService.EditAsync(dto);

        return Ok(ApiResponse<StockModel>.Ok(stock, ResponsesMessage.EditedSuccessfully("Stock")));
    }

    /// <summary>
    /// Delete (soft delete) a stock entry
    /// </summary>
    /// <param name="id">Stock ID</param>
    /// <returns>Deleted stock</returns>
    [HttpDelete("remove/{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<StockModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove(int id)
    {
        var stock = await _stockService.RemoveAsync(id);

        return Ok(ApiResponse<StockModel>.Ok(stock, ResponsesMessage.RemovedSuccessfully("Stock")));
    }
}
