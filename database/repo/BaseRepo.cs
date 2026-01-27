using Microsoft.EntityFrameworkCore;

public abstract class BaseRepo<T> : IBaseRepo<T>
    where T : BaseEntity
{
    protected readonly DbContext _context;
    protected readonly DbSet<T> _dbSet;

    protected BaseRepo(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    // ---------- CREATE ----------
    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    // ---------- SAVE ----------
    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    // ---------- READ ----------
    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet
            .FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _dbSet
            .AsNoTracking()
            .Where(x => x.DeletedAt == null)
            .ToListAsync();
    }

    // ---------- DELETE ----------
    public async Task<bool> DeleteByIdAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null)
            return false;

        _dbSet.Remove(entity);
        return true;
    }

}
