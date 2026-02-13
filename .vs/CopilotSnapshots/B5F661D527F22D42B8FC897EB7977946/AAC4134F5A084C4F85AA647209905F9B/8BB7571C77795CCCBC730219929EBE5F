using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GradeManagement.Entity {
  [Table("result")]
  public class Result {
    public string sid { get; set; }
    public string cid { get; set; }
    public float grade { get; set; }

    public Result() { }

    public Result(string sid, string cid, float grade) {
      this.sid = sid;
      this.cid = cid;
      this.grade = grade;
    }
  }
}
