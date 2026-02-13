using GradeManagement.RepositoryInterface;

namespace GradeManagement.Service {
  internal class AdminService {
    ICourseRepository courseRepository = null!;
    IUserRepository userRepository = null!;
    IStudentRepository studentRepository = null!;
    IResultRepository resultRepository = null!;

    public AdminService (ICourseRepository courseRepository, IUserRepository userRepository, IStudentRepository studentRepository, IResultRepository resultRepository) {
      this.courseRepository = courseRepository;
      this.userRepository = userRepository;
      this.studentRepository = studentRepository;
      this.resultRepository = resultRepository;
    }
    public async Task<int> addCourse (string cid, string cname) {
      throw new NotImplementedException ();
    }
    public async Task<int> importCourseFromFile (string filePath) {
      throw new NotImplementedException ();
    }
    public async Task<int> addStudentToCourse (string Sid, string Cid) {
      throw new NotImplementedException ();
    }
  }
}
