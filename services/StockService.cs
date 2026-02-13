using Core.Errors;
using Core.Exceptions;

/// <summary>
/// Service for managing stock
/// </summary>
public class StockService : IStockService
{
    private readonly IStockRepo _stockRepo;
    private readonly IProductRepo _productRepo;

    public StockService(IStockRepo stockRepo, IProductRepo productRepo)
    {
        _stockRepo = stockRepo;
        _productRepo = productRepo;
    }

    /// <summary>
    /// Get all stock entries
    /// </summary>
    /// <returns>List of all active stock entries</returns>
    public async Task<IReadOnlyList<StockModel>> GetAllAsync()
    {
        var stocks = await _stockRepo.GetAllAsync();

        return stocks.Select(StockModel.ToModel).ToList();
    }

    /// <summary>
    /// Get a stock entry by ID
    /// </summary>
    /// <param name="id">Stock ID</param>
    /// <returns>Stock model</returns>
    /// <exception cref="AppException">Thrown when stock not found</exception>
    public async Task<StockModel> GetByIdAsync(int id)
    {
        var stock = await _stockRepo.GetByIdAsync(id);

        if (stock == null)
        {
            throw new AppException(ErrorCode.NotFound, ErrorMessage.StockNotFound);
        }

        return StockModel.ToModel(stock);
    }

    /// <summary>
    /// Create a new stock entry
    /// </summary>
    /// <param name="dto">Stock creation data</param>
    /// <returns>Created stock model</returns>
    /// <exception cref="AppException">Thrown when validation fails or product doesn't exist</exception>
    public async Task<StockModel> AddAsync(CreateStockDTO dto)
    {
        var product = await _productRepo.GetByIdAsync(dto.ProductId);

        if (product == null)
        {
            throw new AppException(
                ErrorCode.NotFound,
                ErrorMessage.ProductNotFound
            );
        }

        if (dto.Quantity < 0)
        {
            throw new AppException(
                ErrorCode.ValidationError,
                ErrorMessage.QuantityInvalid
            );
        }

        if (dto.Cost <= 0)
        {
            throw new AppException(
                ErrorCode.ValidationError,
                ErrorMessage.CostInvalid
            );
        }

        var entity = new StockEntity
        {
            ProductId = dto.ProductId,
            Product = product,
            Quantity = dto.Quantity,
            Cost = dto.Cost
        };

        await _stockRepo.AddAsync(entity);
        await _stockRepo.SaveAsync();

        return StockModel.ToModel(entity);
    }

    /// <summary>
    /// Edit an existing stock entry
    /// </summary>
    /// <param name="dto">Stock edit data</param>
    /// <returns>Updated stock model</returns>
    /// <exception cref="AppException">Thrown when validation fails or stock not found</exception>
    public async Task<StockModel> EditAsync(EditStockDTO dto)
    {
        var existed = await _stockRepo.GetByIdAsync(dto.Id);

        if (existed == null)
        {
            throw new AppException(
                ErrorCode.NotFound,
                ErrorMessage.StockNotFound
            );
        }

        var product = await _productRepo.GetByIdAsync(dto.ProductId);

        if (product == null)
        {
            throw new AppException(
                ErrorCode.NotFound,
                ErrorMessage.ProductNotFound
            );
        }

        if (dto.Quantity < 0)
        {
            throw new AppException(
                ErrorCode.ValidationError,
                ErrorMessage.QuantityInvalid
            );
        }

        if (dto.Cost <= 0)
        {
            throw new AppException(
                ErrorCode.ValidationError,
                ErrorMessage.CostInvalid
            );
        }

        existed.ProductId = dto.ProductId;
        existed.Product = product;
        existed.Quantity = dto.Quantity;
        existed.Cost = dto.Cost;
        existed.UpdatedAt = DateTime.UtcNow;

        await _stockRepo.SaveAsync();

        return StockModel.ToModel(existed);
    }

    /// <summary>
    /// Delete (soft delete) a stock entry
    /// </summary>
    /// <param name="id">Stock ID</param>
    /// <returns>Deleted stock model</returns>
    /// <exception cref="AppException">Thrown when stock not found</exception>
    public async Task<StockModel> RemoveAsync(int id)
    {
        var existed = await _stockRepo.GetByIdAsync(id);

        if (existed == null)
        {
            throw new AppException(
                ErrorCode.NotFound,
                ErrorMessage.StockNotFound
            );
        }

        existed.UpdatedAt = DateTime.UtcNow;
        existed.DeletedAt = DateTime.UtcNow;

        await _stockRepo.SaveAsync();

        return StockModel.ToModel(existed);
    }

    /// <summary>
    /// Get the latest stock entries for each product

    public async Task<IReadOnlyList<StockModel>> GetLatestStockAsync()
    {
        var stocks = await _stockRepo.GetLatestStockAsync();

        return stocks.Select(StockModel.ToModel).ToList();
    }
}
