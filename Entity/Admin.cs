using System.ComponentModel.DataAnnotations.Schema;

namespace GradeManagement.Entity 
{
  [Table("admin")]
  public class Admin : User 
  {
    public Admin() {
      role = Role.ADMIN;
    }

    public Admin(string id, string name, string username, string password)
      : base(id, name, username, password, Role.ADMIN) {
    }
  }
}
