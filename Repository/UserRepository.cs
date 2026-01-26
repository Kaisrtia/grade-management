using GradeManagement.Model;
using GradeManagement.RepositoryInterface;

namespace GradeManagement.Repository {
  internal class UserRepository : BaseRepository<User>, IUserRepository {
    // connect to database
    public Task<User?> getUserByUsername (string username) {
      throw new NotImplementedException ();
    }
  }
}
