using System.ComponentModel.DataAnnotations;

namespace GradeManagement.DTO.Request
{
  public class LoginRequestDTO
  {
    [Required(ErrorMessage = "Username is required")]
    public string username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string password { get; set; }
  }
}
