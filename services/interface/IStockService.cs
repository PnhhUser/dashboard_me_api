/// <summary>
/// Interface for stock service operations
/// </summary>
public interface IStockService
{
    /// <summary>
    /// Get all stock entries
    /// </summary>
    Task<IReadOnlyList<StockModel>> GetAllAsync();

    /// <summary>
    /// Get stock entry by ID
    /// </summary>
    Task<StockModel> GetByIdAsync(int id);

    /// <summary>
    /// Create a new stock entry
    /// </summary>
    Task<StockModel> AddAsync(CreateStockDTO dto);

    /// <summary>
    /// Edit an existing stock entry
    /// </summary>
    Task<StockModel> EditAsync(EditStockDTO dto);

    /// <summary>
    /// Delete (soft delete) a stock entry
    /// </summary>
    Task<StockModel> RemoveAsync(int id);
}
