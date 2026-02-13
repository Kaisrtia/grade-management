using GradeManagement.Entity;

namespace GradeManagement.DTO.Response
{
  public class EnrollmentResponseDTO
  {
    public string studentId { get; set; }
    public string studentName { get; set; }
    public string courseId { get; set; }
    public string courseName { get; set; }
    public float grade { get; set; }
    public string message { get; set; }

    public EnrollmentResponseDTO() { }

    public EnrollmentResponseDTO(Result result, string message = "Enrollment successful")
    {
      this.studentId = result.sid;
      this.studentName = result.Student?.name;
      this.courseId = result.cid;
      this.courseName = result.Course?.name;
      this.grade = result.grade;
      this.message = message;
    }
  }
}
