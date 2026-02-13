using GradeManagement.DTO.Request;
using GradeManagement.DTO.Response;

namespace GradeManagement.ServiceInterface
{
  public interface IAuthenticationService
  {
    Task<AuthResponseDTO> Login(LoginRequestDTO request);
    Task<AuthResponseDTO> RegisterAdmin(RegisterAdminRequestDTO request);
  }
}
