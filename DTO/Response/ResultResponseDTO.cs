using GradeManagement.Entity;

namespace GradeManagement.DTO.Response
{
  public class ResultResponseDTO
  {
    public string studentId { get; set; }
    public string studentName { get; set; }
    public string courseId { get; set; }
    public string courseName { get; set; }
    public float grade { get; set; }

    public ResultResponseDTO() { }

    public ResultResponseDTO(Result result)
    {
      this.studentId = result.sid;
      this.studentName = result.Student?.name;
      this.courseId = result.cid;
      this.courseName = result.Course?.name;
      this.grade = result.grade;
    }
  }
}
