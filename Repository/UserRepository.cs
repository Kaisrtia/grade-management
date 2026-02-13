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

    public async Task<User?> getUserByUsername(string username)
    {
      return await _dbSet.FirstOrDefaultAsync(u => u.username == username);
    }
  }
}
