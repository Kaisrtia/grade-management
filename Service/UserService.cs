using GradeManagement.DTO.Request;
using GradeManagement.DTO.Response;
using GradeManagement.Entity;
using GradeManagement.RepositoryInterface;
using GradeManagement.ServiceInterface;

namespace GradeManagement.Service {
  internal class UserService : IUserService {
    IUserRepository userRepository = null!;

    public UserService (IUserRepository userRepository) {
      this.userRepository = userRepository;
    }

    public async Task<UserResponseDTO> changeInfo (UserChangeInfoRequestDTO request) {
      // Validate user exists
      var user = await userRepository.getById(request.id);
      if (user == null) {
        throw new ArgumentException($"User with ID '{request.id}' does not exist");
      }

      // Check if new username is taken by another user
      if (user.username != request.username) {
        var existingUser = await userRepository.getUserByUsername(request.username);
        if (existingUser != null && existingUser.id != request.id) {
          throw new InvalidOperationException($"Username '{request.username}' is already taken");
        }
      }

      // Update user properties
      user.name = request.name;
      user.username = request.username;
      
      // Hash password if it's being changed (don't re-hash if it's already hashed)
      // Assuming the request password is plain text
      user.password = BCrypt.Net.BCrypt.HashPassword(request.password);

      await userRepository.update(user);

      return new UserResponseDTO(user);
    }
  }
}
