using GradeManagement.DTO.Response;

namespace GradeManagement.ServiceInterface {
  internal interface IStudentService {
    Task<IEnumerable<ResultResponseDTO>> showResult (string Id);
  }
}
