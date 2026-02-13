using GradeManagement.DTO.Request;
using GradeManagement.DTO.Response;
using GradeManagement.Entity;
using GradeManagement.RepositoryInterface;
using GradeManagement.ServiceInterface;
using Microsoft.EntityFrameworkCore;

namespace GradeManagement.Service {
  internal class AdminService : IAdminService {
    ICourseRepository courseRepository = null!;
    IUserRepository userRepository = null!;
    IStudentRepository studentRepository = null!;
    IResultRepository resultRepository = null!;
    IIdGeneratorService idGeneratorService = null!;

    public AdminService (ICourseRepository courseRepository, IUserRepository userRepository, IStudentRepository studentRepository, IResultRepository resultRepository, IIdGeneratorService idGeneratorService) {
      this.courseRepository = courseRepository;
      this.userRepository = userRepository;
      this.studentRepository = studentRepository;
      this.resultRepository = resultRepository;
      this.idGeneratorService = idGeneratorService;
    }

    public async Task<CourseResponseDTO> addCourse(CourseRequestDTO request) {
      // Auto-generate course ID
      string courseId = await idGeneratorService.GenerateCourseId();

      Course course = new Course(courseId, request.name);
      await courseRepository.create(course);
      
      return new CourseResponseDTO(course);
    }

    public async Task<ImportCoursesResponseDTO> importCourseFromFile(string filePath) {
      if (!File.Exists(filePath)) {
        throw new FileNotFoundException($"File not found: {filePath}");
      }

      int successCount = 0;
      int failureCount = 0;
      List<string> errors = new List<string>();

      try {
        var lines = await File.ReadAllLinesAsync(filePath);
        
        foreach (var line in lines) {
          if (string.IsNullOrWhiteSpace(line)) continue;

          // Expected format: courseId,courseName
          var parts = line.Split(',');
          if (parts.Length != 2) {
            failureCount++;
            errors.Add($"Invalid format in line: {line}");
            continue;
          }

          string courseId = parts[0].Trim();
          string courseName = parts[1].Trim();

          if (string.IsNullOrEmpty(courseId) || string.IsNullOrEmpty(courseName)) {
            failureCount++;
            errors.Add($"Empty course ID or name in line: {line}");
            continue;
          }

          try {
            // Check if course already exists
            var existingCourse = await courseRepository.getById(courseId);
            if (existingCourse != null) {
              failureCount++;
              errors.Add($"Course '{courseId}' already exists");
              continue;
            }

            Course course = new Course(courseId, courseName);
            await courseRepository.create(course);
            successCount++;
          } catch (Exception ex) {
            failureCount++;
            errors.Add($"Failed to import course '{courseId}': {ex.Message}");
          }
        }
      } catch (Exception ex) {
        throw new InvalidOperationException($"Error reading file: {ex.Message}", ex);
      }

      return new ImportCoursesResponseDTO(successCount, failureCount, errors);
    }

    public async Task<EnrollmentResponseDTO> addStudentToCourse(EnrollmentRequestDTO request) {
      // Validate student exists
      var student = await studentRepository.getById(request.studentId);
      if (student == null) {
        throw new ArgumentException($"Student with ID '{request.studentId}' does not exist");
      }

      // Validate course exists
      var course = await courseRepository.getById(request.courseId);
      if (course == null) {
        throw new ArgumentException($"Course with ID '{request.courseId}' does not exist");
      }

      // Check for duplicate enrollment
      var existingEnrollment = await resultRepository.getResult(request.studentId, request.courseId);
      if (existingEnrollment.Any()) {
        throw new InvalidOperationException($"Student '{request.studentId}' is already enrolled in course '{request.courseId}'");
      }

      // Create enrollment with initial grade of 0
      Result enrollment = new Result(request.studentId, request.courseId, 0);
      enrollment.Student = student;
      enrollment.Course = course;
      
      await resultRepository.create(enrollment);

      return new EnrollmentResponseDTO(enrollment, "Student enrolled successfully");
    }

    public async Task<FacultyResponseDTO> addFaculty(FacultyRequestDTO request) {
      // Auto-generate faculty ID
      string facultyId = await idGeneratorService.GenerateFacultyId(request.name);

      Faculty faculty = new Faculty(facultyId, request.name);
      
      // We need to use the context directly since there's no FacultyRepository
      // This is a temporary solution - ideally we should have a FacultyRepository
      var context = courseRepository.GetType().GetField("_context", 
        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
        ?.GetValue(courseRepository) as GradeManagement.Config.AppDbContext;
      
      if (context == null) {
        throw new InvalidOperationException("Unable to access database context");
      }

      context.Faculties.Add(faculty);
      await context.SaveChangesAsync();
      
      return new FacultyResponseDTO { id = faculty.id, name = faculty.name };
    }
  }
}
