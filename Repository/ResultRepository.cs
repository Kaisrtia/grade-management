using GradeManagement.Config;
using GradeManagement.Entity;
using GradeManagement.RepositoryInterface;

namespace GradeManagement.Repository 
{
  internal class ResultRepository : BaseRepository<Result>, IResultRepository 
  {
    public ResultRepository(AppDbContext context) : base(context)
    {
    }
    
    // Note: Result has composite key (sid, cid) so getById won't work
    // You may need to add custom methods like getResultByStudentAndCourse
  }
}
