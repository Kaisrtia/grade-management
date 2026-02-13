using GradeManagement.Entity;

namespace GradeManagement.DTO.Response
{
  public class AuthResponseDTO
  {
    public bool success { get; set; }
    public string message { get; set; }
    public string? userId { get; set; }
    public string? username { get; set; }
    public string? name { get; set; }
    public Role? role { get; set; }

    public AuthResponseDTO() { }

    public AuthResponseDTO(bool success, string message, User? user = null)
    {
      this.success = success;
      this.message = message;
      if (user != null)
      {
        this.userId = user.id;
        this.username = user.username;
        this.name = user.name;
        this.role = user.role;
      }
    }
  }
}
