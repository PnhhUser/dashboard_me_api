public interface IBaseRepo<T> where T : class
{
    Task AddAsync(T entity);

    Task<bool> DeleteByIdAsync(int id);

    Task<IReadOnlyList<T>> GetAllAsync();

    Task<T?> GetByIdAsync(int id);

    Task SaveAsync();
}