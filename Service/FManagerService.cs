using GradeManagement.RepositoryInterface;
using GradeManagement.ServiceInterface;

namespace GradeManagement.Service {
  internal class FManagerService : IFManagerService {
    IStudentRepository studentRepository;
    ICourseRepository courseRepository;
    IResultRepository resultRepository;
    public async Task<int> updateResult (string Sid, string Cid, float grade) {
      throw new NotImplementedException ();
    }
  }
}
