using System.ComponentModel.DataAnnotations;
using GradeManagement.Config;
using GradeManagement.RepositoryInterface;
using Microsoft.EntityFrameworkCore;

namespace GradeManagement.Repository
{
  internal class BaseRepository<T> : IBaseRepository<T> where T : class, new()
  {
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(AppDbContext context)
    {
      _context = context;
      _dbSet = _context.Set<T>();
    }

    // Helper to validate data
    private void ValidateEntity(T entity)
    {
      var context = new ValidationContext(entity);
      var results = new List<ValidationResult>();

      bool isValid = Validator.TryValidateObject(entity, context, results, true);

      if (!isValid)
      {
        throw new ValidationException(results[0].ErrorMessage);
      }
    }

    // CRUD operations with EF Core
    public async Task<int> create(T entity)
    {
      ValidateEntity(entity);
      await _dbSet.AddAsync(entity);
      return await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<T>> getAll()
    {
      return await _dbSet.ToListAsync();
    }

    public async Task<T?> getById(string id)
    {
      return await _dbSet.FindAsync(id);
    }

    public async Task<int> update(T entity)
    {
      ValidateEntity(entity);
      _dbSet.Update(entity);
      return await _context.SaveChangesAsync();
    }

    public async Task<int> delete(string id)
    {
      var entity = await getById(id);
      if (entity == null)
      {
        return 0;
      }
      _dbSet.Remove(entity);
      return await _context.SaveChangesAsync();
    }
  }
}
