using System.ComponentModel.DataAnnotations;

namespace GradeManagement.DTO.Request
{
  public class CourseRequestDTO
  {
    [Required(ErrorMessage = "Course ID is required")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Course ID must be between 1 and 50 characters")]
    public string id { get; set; }

    [Required(ErrorMessage = "Course name is required")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Course name must be between 1 and 100 characters")]
    public string name { get; set; }
  }
}
