using GradeManagement.Data;
using GradeManagement.Model;
using GradeManagement.RepositoryInterface;

namespace GradeManagement.Repository {
  internal class ResultRepository : BaseRepository<Result>, IResultRepository {
    public ResultRepository(GradeDbContext context) : base(context) {
    }
  }
}
