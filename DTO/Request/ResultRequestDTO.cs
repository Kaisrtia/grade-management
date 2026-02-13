using System.ComponentModel.DataAnnotations;

namespace GradeManagement.DTO.Request
{
  public class ResultRequestDTO
  {
    [Required(ErrorMessage = "Student ID is required")]
    public string sid { get; set; }

    [Required(ErrorMessage = "Course ID is required")]
    public string cid { get; set; }

    [Required(ErrorMessage = "Grade is required")]
    [Range(0, 100, ErrorMessage = "Grade must be between 0 and 100")]
    public float grade { get; set; }
  }
}
