using GradeManagement.Config;
using GradeManagement.Entity;
using GradeManagement.RepositoryInterface;
using Microsoft.EntityFrameworkCore;

namespace GradeManagement.Repository
{
  internal class UserRepository : BaseRepository<User>, IUserRepository
  {
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    // Custom method to get user by username using EF Core LINQ
    public async Task<User?> getUserByUsername(string username)
    {
      return await _dbSet.FirstOrDefaultAsync(u => u.username == username);
    }
  }
}
