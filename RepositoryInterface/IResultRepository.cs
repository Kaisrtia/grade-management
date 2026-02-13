using GradeManagement.Entity;

namespace GradeManagement.RepositoryInterface {
  internal interface IResultRepository : IBaseRepository<Result> {
    Task<IEnumerable<Result>> getResult(string sid, string cid);
    Task<IEnumerable<Result>> getResultsByStudent(string sid);
  }
}
