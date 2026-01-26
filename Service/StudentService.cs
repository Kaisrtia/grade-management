using GradeManagement.Model;
using GradeManagement.RepositoryInterface;
using GradeManagement.ServiceInterface;

namespace GradeManagement.Service {
  internal class StudentService : IStudentService {
    IResultRepository resultRepository = null!;
    ICourseRepository courseRepository = null!;
    IUserRepository userRepository = null!;

    public StudentService (IResultRepository resultRepository, ICourseRepository courseRepository, IUserRepository userRepository) {
      this.resultRepository = resultRepository;
      this.courseRepository = courseRepository;
      this.userRepository = userRepository;
    }

    public async Task<IEnumerable<Result>> showResult (string Id) {
      throw new NotImplementedException ();
    }
  }
}
