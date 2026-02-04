using GradeManagement.Entity;
using GradeManagement.RepositoryInterface;
using GradeManagement.ServiceInterface;

namespace GradeManagement.Service {
  internal class UserService : IUserService {
    IUserRepository userRepository = null!;

    public UserService (IUserRepository userRepository) {
      this.userRepository = userRepository;
    }

    public async Task<bool> changeInfo (User user) {
      User? tempU = await userRepository.getUserByUsername(user.username);
      if (tempU != null) {
        return false;
      }
      try {
        int affectedRows = await userRepository.update(user);
        return affectedRows > 0;
      } catch (Exception e) {
        System.Console.WriteLine(e.Message);
        return false;
      }
    }
  }
}
