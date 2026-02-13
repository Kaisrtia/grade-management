using GradeManagement.Entity;

namespace GradeManagement.DTO.Response
{
  public class UserResponseDTO
  {
    public string id { get; set; }
    public string name { get; set; }
    public string username { get; set; }
    public Role role { get; set; }

    public UserResponseDTO() { }

    public UserResponseDTO(User user)
    {
      this.id = user.id;
      this.name = user.name;
      this.username = user.username;
      this.role = user.role;
    }
  }
}
