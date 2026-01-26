namespace GradeManagement.Model {
  internal class FManager : User {
    string _Fid;

    public string Fid {
      get { return _Fid; }
      set { _Fid = value; }
    }

    public FManager() { }

    public FManager(string id, string name, string username, string password, string Fid)
      : base(id, name, username, password, Role.FMANAGER) {
      _Fid = Fid;
    }
  }
}
