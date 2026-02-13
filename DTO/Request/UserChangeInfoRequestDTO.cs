using System.ComponentModel.DataAnnotations;

namespace GradeManagement.DTO.Request
{
  public class UserChangeInfoRequestDTO
  {
    [Required(ErrorMessage = "User ID is required")]
    public string id { get; set; }

    [Required(ErrorMessage = "Name cannot be empty")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 50 characters")]
    public string name { get; set; }

    [Required(ErrorMessage = "Username cannot be empty and must be at least 5 characters")]
    [StringLength(50, MinimumLength = 5, ErrorMessage = "Username must be between 5 and 50 characters")]
    public string username { get; set; }

    [Required(ErrorMessage = "Password cannot be empty and must be at least 5 characters")]
    [StringLength(50, MinimumLength = 5, ErrorMessage = "Password must be between 5 and 50 characters")]
    public string password { get; set; }
  }
}
