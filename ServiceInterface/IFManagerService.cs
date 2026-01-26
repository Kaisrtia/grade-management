namespace GradeManagement.ServiceInterface {
  internal interface IFManagerService {
    Task<int> updateResult (string Sid, string Cid, float grade);
  }
}
