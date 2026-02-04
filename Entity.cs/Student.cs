using System.ComponentModel.DataAnnotations.Schema;

namespace GradeManagement.Entity 
{
    [Table("student")]
    public class Student : User
    {
    public string fid { get; set; }

    public Student() { 
      role = Role.STUDENT;
    }

    public Student (string id, string name, string username, string password, string fid)
      : base(id, name, username, password, Role.STUDENT) {
      this.fid = fid;
    }
  }
}
