using GradeManagement.Data;
using GradeManagement.Model;
using GradeManagement.RepositoryInterface;

namespace GradeManagement.Repository {
  internal class CourseRepository : BaseRepository<Course>, ICourseRepository {
    public CourseRepository(GradeDbContext context) : base(context) {
    }
  }
}
