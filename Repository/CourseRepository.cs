using GradeManagement.Config;
using GradeManagement.Entity;
using GradeManagement.RepositoryInterface;

namespace GradeManagement.Repository 
{
  internal class CourseRepository : BaseRepository<Course>, ICourseRepository 
  {
    public CourseRepository(AppDbContext context) : base(context)
    {
    }
  }
}
