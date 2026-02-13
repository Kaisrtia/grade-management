using GradeManagement.Entity;

namespace GradeManagement.DTO.Response
{
  public class StudentResponseDTO
  {
    public string id { get; set; }
    public string name { get; set; }
    public string username { get; set; }
    public Role role { get; set; }
    public string facultyId { get; set; }
    public string facultyName { get; set; }

    public StudentResponseDTO() { }

    public StudentResponseDTO(Student student)
    {
      this.id = student.id;
      this.name = student.name;
      this.username = student.username;
      this.role = student.role;
      this.facultyId = student.fid;
      this.facultyName = student.Faculty?.name;
    }
  }
}
