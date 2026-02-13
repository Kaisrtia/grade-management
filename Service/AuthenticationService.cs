using GradeManagement.Config;
using GradeManagement.DTO.Request;
using GradeManagement.DTO.Response;
using GradeManagement.Entity;
using GradeManagement.ServiceInterface;
using Microsoft.EntityFrameworkCore;

namespace GradeManagement.Service
{
  public class AuthenticationService : IAuthenticationService
  {
    private readonly AppDbContext _context;
    private readonly IIdGeneratorService _idGenerator;

    public AuthenticationService(AppDbContext context, IIdGeneratorService idGenerator)
    {
      _context = context;
      _idGenerator = idGenerator;
    }

    #region Login

    public async Task<AuthResponseDTO> Login(LoginRequestDTO request)
    {
      try
      {
        // Find user by username
        var user = await _context.Users
          .FirstOrDefaultAsync(u => u.username == request.username);

        if (user == null)
        {
          return new AuthResponseDTO(false, "Invalid username or password");
        }

        // Verify password
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.password, user.password);

        if (!isPasswordValid)
        {
          return new AuthResponseDTO(false, "Invalid username or password");
        }

        // Login successful
        return new AuthResponseDTO(true, "Login successful", user);
      }
      catch (Exception ex)
      {
        return new AuthResponseDTO(false, $"Login failed: {ex.Message}");
      }
    }

    #endregion

    #region Register Admin

    public async Task<AuthResponseDTO> RegisterAdmin(RegisterAdminRequestDTO request)
    {
      try
      {
        // Check if username already exists
        var existingUser = await _context.Users
          .FirstOrDefaultAsync(u => u.username == request.username);

        if (existingUser != null)
        {
          return new AuthResponseDTO(false, "Username already exists");
        }

        // Generate ID
        string adminId = await _idGenerator.GenerateAdminId();

        // Hash password
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.password);

        // Create admin entity
        var admin = new Admin(adminId, request.name, request.username, hashedPassword);

        // Save to database
        _context.Admins.Add(admin);
        await _context.SaveChangesAsync();

        // Return success response
        return new AuthResponseDTO(true, "Admin registered successfully", admin);
      }
      catch (Exception ex)
      {
        return new AuthResponseDTO(false, $"Registration failed: {ex.Message}");
      }
    }

    #endregion
  }
}
