using GradeManagement.DTO.Request;
using GradeManagement.DTO.Response;

namespace GradeManagement.ServiceInterface {
  internal interface IStudentService {
    Task<IEnumerable<ResultResponseDTO>> showResult (string Id);
    Task<bool> updateStudent(UserChangeInfoRequestDTO request);
  }
}
