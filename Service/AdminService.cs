using GradeManagement.Config;
using GradeManagement.DTO.Request;
using GradeManagement.DTO.Response;
using GradeManagement.Entity;
using GradeManagement.RepositoryInterface;
using GradeManagement.ServiceInterface;
using Microsoft.EntityFrameworkCore;

namespace GradeManagement.Service {
  internal class AdminService : IAdminService {
    private readonly ICourseRepository courseRepository;
    private readonly IUserRepository userRepository;
    private readonly IStudentRepository studentRepository;
    private readonly IResultRepository resultRepository;
    private readonly AppDbContext _context;
    private readonly IIdGenerator _idGenerator;

    public AdminService (ICourseRepository courseRepository, IUserRepository userRepository, IStudentRepository studentRepository, IResultRepository resultRepository, AppDbContext context, IIdGenerator idGenerator) {
      this.courseRepository = courseRepository;
      this.userRepository = userRepository;
      this.studentRepository = studentRepository;
      this.resultRepository = resultRepository;
      this._context = context;
      this._idGenerator = idGenerator;
    }

    #region Course Management

    public async Task<int> addCourse (string cid, string cname) {
      throw new NotImplementedException ();
    }

    public async Task<int> importCourseFromFile (string filePath) {
      throw new NotImplementedException ();
    }

    public async Task<int> addStudentToCourse (string Sid, string Cid) {
      throw new NotImplementedException ();
    }

    #endregion

    #region User Creation (Admin-Only)

    public async Task<AuthResponseDTO> CreateStudent(RegisterStudentRequestDTO request)
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

        // Verify faculty exists
        var faculty = await _context.Faculties
          .FirstOrDefaultAsync(f => f.id == request.facultyId);

        if (faculty == null)
        {
          return new AuthResponseDTO(false, "Faculty not found");
        }

        // Generate ID
        string studentId = await _idGenerator.GenerateStudentId(request.facultyId);

        // Hash password
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.password);

        // Create student entity
        var student = new Student(studentId, request.name, request.username, hashedPassword, request.facultyId);

        // Save to database
        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        // Return success response
        return new AuthResponseDTO(true, "Student created successfully", student);
      }
      catch (Exception ex)
      {
        return new AuthResponseDTO(false, $"Failed to create student: {ex.Message}");
      }
    }

    public async Task<AuthResponseDTO> CreateFManager(RegisterFManagerRequestDTO request)
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

        // Verify faculty exists
        var faculty = await _context.Faculties
          .FirstOrDefaultAsync(f => f.id == request.facultyId);

        if (faculty == null)
        {
          return new AuthResponseDTO(false, "Faculty not found");
        }

        // Generate ID
        string fmId = await _idGenerator.GenerateFManagerId();

        // Hash password
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.password);

        // Create faculty manager entity
        var fmanager = new FManager(fmId, request.name, request.username, hashedPassword, request.facultyId);

        // Save to database
        _context.FManagers.Add(fmanager);
        await _context.SaveChangesAsync();

        // Return success response
        return new AuthResponseDTO(true, "Faculty Manager created successfully", fmanager);
      }
      catch (Exception ex)
      {
        return new AuthResponseDTO(false, $"Failed to create faculty manager: {ex.Message}");
      }
    }

    public async Task<int> CreateFaculty(string name)
    {
      try
      {
        // Generate ID
        string facultyId = await _idGenerator.GenerateFacultyId(name);

        // Create faculty entity
        var faculty = new Faculty(facultyId, name);

        // Save to database
        _context.Faculties.Add(faculty);
        return await _context.SaveChangesAsync();
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Failed to create faculty: {ex.Message}");
        return 0;
      }
    }

    #endregion
  }
}
