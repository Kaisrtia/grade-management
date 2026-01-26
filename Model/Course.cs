namespace GradeManagement.Model {
  internal class Course {
    string _id;
    string _name;

    public string id {
      get { return _id; }
      set { _id = value; }
    }

    public string name {
      get { return _name; }
      set { _name = value; }
    }

    public Course() { }

    public Course (string id, string name) {
      _id = id;
      _name = name;
    }
  }
}
