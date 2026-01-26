using GradeManagement.Data;
using GradeManagement.Model;
using GradeManagement.RepositoryInterface;

namespace GradeManagement.Repository {
  internal class StudentRepository : BaseRepository<Student>, IStudentRepository {
    public StudentRepository(GradeDbContext context) : base(context) {
    }
  }
}
