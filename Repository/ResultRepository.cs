using GradeManagement.Config;
using GradeManagement.Entity;
using GradeManagement.RepositoryInterface;
using Microsoft.EntityFrameworkCore;

namespace GradeManagement.Repository 
{
  internal class ResultRepository : BaseRepository<Result>, IResultRepository 
  {
    public ResultRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Result>> getResult(string sid, string cid) {
      return await _dbSet
        .Where(r => r.sid == sid && r.cid == cid)
        .ToListAsync();
    }

    public async Task<IEnumerable<Result>> getResultsByStudent(string sid) {
      return await _dbSet
        .Include(r => r.Student)
        .Include(r => r.Course)
        .Where(r => r.sid == sid)
        .ToListAsync();
    }

    // Note: Result has composite key (sid, cid) so getById won't work
    // You may need to add custom methods like getResultByStudentAndCourse
  }
}
