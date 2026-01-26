using GradeManagement.Data;
using GradeManagement.RepositoryInterface;
using Microsoft.EntityFrameworkCore;

namespace GradeManagement.Repository {
  internal class BaseRepository<T> : IBaseRepository<T> where T : class {
    private readonly GradeDbContext _context;
    private readonly DbSet<T> _dbSet;

    public BaseRepository(GradeDbContext context) {
      _context = context;
      _dbSet = context.Set<T>();
    }

    // connect to SQL database
    public async Task<int> create (T entity) {
      await _dbSet.AddAsync(entity);
      return await _context.SaveChangesAsync();
    }

    public async Task<int> delete (string id) {
      var entity = await _dbSet.FindAsync(id);
      if (entity == null) return 0;
      
      _dbSet.Remove(entity);
      return await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<T>> getAll () {
      return await _dbSet.ToListAsync();
    }

    public async Task<T?> getById (string id) {
      return await _dbSet.FindAsync(id);
    }

    public async Task<int> update (T entity) {
      _dbSet.Update(entity);
      return await _context.SaveChangesAsync();
    }
  }
}
