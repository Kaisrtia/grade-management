namespace GradeManagement.Model {
  internal class Student : User {
    string _Fid;

    public string Fid {
      get { return _Fid; }
      set { _Fid = value; }
    }
    public Student() { 
      role = Role.STUDENT;
    }

    public Student (string id, string name, string username, string password, string Fid)
      : base(id, name, username, password, Role.STUDENT) {
      this.Fid = Fid;
    }
  }
}
