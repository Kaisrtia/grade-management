using GradeManagement.Model;

namespace GradeManagement.RepositoryInterface {
  internal interface IUserRepository : IBaseRepository<User> {
    Task<User?> getUserByUsername(string username);
  }
}
