using GradeManagement.DTO.Request;
using GradeManagement.DTO.Response;

namespace GradeManagement.ServiceInterface {
  internal interface IFManagerService {
    Task<ResultResponseDTO> updateResult (ResultRequestDTO request);
  }
}
