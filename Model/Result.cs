namespace GradeManagement.Model {
  internal class Result {
    string _Sid;
    string _Cid;
    float _grade;

    public string Sid {
      get { return _Sid; }
      set { _Sid = value; }
    }

    public string Cid {
      get { return _Cid; }
      set { _Cid = value; }
    }

    public float grade {
      get { return _grade; }
      set { _grade = value; }
    }

    public Result () { }

    public Result(string Sid, string Cid, float grade) {
      _Sid = Sid;
      _Cid = Cid;
      _grade = grade;
    }
  }
}
