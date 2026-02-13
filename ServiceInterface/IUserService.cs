using GradeManagement.DTO.Request;
using GradeManagement.DTO.Response;
using System.Threading.Tasks;

namespace GradeManagement.ServiceInterface {
  internal interface IUserService {
    Task<UserResponseDTO> changeInfo(UserChangeInfoRequestDTO request);
  }
}
