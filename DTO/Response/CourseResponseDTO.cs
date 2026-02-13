using GradeManagement.Entity;

namespace GradeManagement.DTO.Response
{
  public class CourseResponseDTO
  {
    public string id { get; set; }
    public string name { get; set; }

    public CourseResponseDTO() { }

    public CourseResponseDTO(Course course)
    {
      this.id = course.id;
      this.name = course.name;
    }
  }
}
