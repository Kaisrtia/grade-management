using GradeManagement.Model;

namespace GradeManagement.ServiceInterface {
  internal interface IStudentService {
    Task<IEnumerable<Result>> showResult (string Id);
  }
}
