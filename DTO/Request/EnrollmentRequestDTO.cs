using System.ComponentModel.DataAnnotations;

namespace GradeManagement.DTO.Request
{
  public class EnrollmentRequestDTO
  {
    [Required(ErrorMessage = "Student ID is required")]
    public string studentId { get; set; }

    [Required(ErrorMessage = "Course ID is required")]
    public string courseId { get; set; }
  }
}
