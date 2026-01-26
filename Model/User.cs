using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GradeManagement.Model {
  public enum Role {
    ADMIN,
    FMANAGER,
    STUDENT
  }

  [Table("Users")]
  public class User {
    protected string _id = null!;
    protected string _name = null!;
    protected string _username = null!;
    protected string _password = null!;
    protected Role _role;

    [Key]
    [Column("Id")]
    [MaxLength(50)]
    public string id {
      get { return _id; }
      set { _id = value; }
    }

    [Column("Name")]
    [MaxLength(100)]
    public string name {
      get { return _name; }
      set { _name = value; }
    }

    [Column("Username")]
    [MaxLength(50)]
    public string username {
      get { return _username; }
      set { _username = value; }
    }

    [Column("Password")]
    [MaxLength(255)]
    public string password {
      get { return _password; }
      set { _password = value; }
    }

    [Column("Role")]
    public Role role {
      get { return _role; }
      set { _role = value; }
    }

    public User() { }

    public User (string id, string name, string username, string password, Role role) {
      _id = id;
      _name = name;
      _username = username;
      _password = password;
      _role = role;
    }
  }
}