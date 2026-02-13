using System.ComponentModel.DataAnnotations;

namespace GradeManagement.DTO.Request
{
  public class FacultyRequestDTO
  {
    [Required(ErrorMessage = "Faculty name is required")]
    [StringLength(100, ErrorMessage = "Faculty name cannot exceed 100 characters")]
    public string name { get; set; }

    public FacultyRequestDTO() { }
  }
}
