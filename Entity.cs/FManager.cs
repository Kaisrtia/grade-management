using System.ComponentModel.DataAnnotations.Schema;

namespace GradeManagement.Entity 
{
  [Table("fManager")]
  public class FManager : User 
  {
    public string fid { get; set; }

    public FManager() { }

    public FManager(string id, string name, string username, string password, string fid)
      : base(id, name, username, password, Role.FMANAGER) {
      this.fid = fid;
    }
  }
}
