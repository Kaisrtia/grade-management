using System.ComponentModel.DataAnnotations;

namespace GradeManagement.DTO.Request
{
  public class CourseRequestDTO
  {
    [Required(ErrorMessage = "Course name is required")]
    [StringLength(100, ErrorMessage = "Course name cannot exceed 100 characters")]
    public string name { get; set; }

    public CourseRequestDTO() { }
  }
}
