namespace GradeManagement.Model {
  enum Role {
    ADMIN,
    FMANAGER,
    STUDENT
  }
  internal class User {
    protected string _id;
    protected string _name;
    protected string _username;
    protected string _password;
    protected Role _role;

    public string id {
      get { return _id; }
      set { _id = value; }
    }

    public string name {
      get { return _name; }
      set { _name = value; }
    }

    public string username {
      get { return _username; }
      set { _username = value; }
    }

    public string password {
      get { return _password; }
      set { _password = value; }
    }

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