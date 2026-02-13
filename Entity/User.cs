using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GradeManagement.Entity
{
  public enum Role
  {
    ADMIN,
    FMANAGER,
    STUDENT
  }
  
  [Table("user")]
  public class User
  {
    [Key]
    public string id { get; set; }

    [Required(ErrorMessage = "Name cannot be empty")]
    [StringLength(50, MinimumLength = 1)]
    public string name { get; set; }

    [Required(ErrorMessage = "Username cannot be empty and at least 5 characters")]
    [StringLength(50, MinimumLength = 5)]
    public string username { get; set; }

    [Required(ErrorMessage = "Password cannot be empty and at least 5 characters")]
    [StringLength(100, MinimumLength = 5)] 
    public string password { get; set; }
    public Role role { get; set; }

    public User() { }

    public User(string _id, string _name, string _username, string _password, Role _role)
    {
      id = _id;
      name = _name;
      username = _username;
      password = _password;
      role = _role;
    }
  }
}