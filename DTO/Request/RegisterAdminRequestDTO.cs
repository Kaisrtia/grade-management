using System.ComponentModel.DataAnnotations;

namespace GradeManagement.DTO.Request
{
  public class RegisterAdminRequestDTO
  {
    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 50 characters")]
    public string name { get; set; }

    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, MinimumLength = 5, ErrorMessage = "Username must be between 5 and 50 characters")]
    public string username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(50, MinimumLength = 5, ErrorMessage = "Password must be between 5 and 50 characters")]
    public string password { get; set; }
  }
}
