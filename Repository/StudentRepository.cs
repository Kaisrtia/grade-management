using GradeManagement.Config;
using GradeManagement.Entity;
using GradeManagement.RepositoryInterface;

namespace GradeManagement.Repository 
{
  internal class StudentRepository : BaseRepository<Student>, IStudentRepository 
  {
    public StudentRepository(AppDbContext context) : base(context)
    {
    }
  }
}
