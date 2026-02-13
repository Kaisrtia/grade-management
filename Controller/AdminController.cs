using GradeManagement.Config;
using GradeManagement.DTO.Request;
using GradeManagement.DTO.Response;
using GradeManagement.Repository;
using GradeManagement.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GradeManagement.Controller
{
  public class AdminController
  {
    // Don't store context - create new one for each operation
    public AdminController()
    {
    }

    public async Task<(bool success, string message, CourseResponseDTO? course)> AddCourse(string courseName)
    {
      try
      {
        using (var context = new AppDbContext())
        {
          var courseRepository = new CourseRepository(context);
          var userRepository = new UserRepository(context);
          var studentRepository = new StudentRepository(context);
          var resultRepository = new ResultRepository(context);
          var idGeneratorService = new IdGeneratorService(context);
          
          var adminService = new AdminService(courseRepository, userRepository, studentRepository, resultRepository, idGeneratorService);
          
          var request = new CourseRequestDTO { name = courseName };
          var result = await adminService.addCourse(request);
          return (true, $"Course added successfully with ID: {result.id}", result);
        }
      }
      catch (InvalidOperationException ex)
      {
        return (false, ex.Message, null);
      }
      catch (Exception ex)
      {
        return (false, $"Error adding course: {ex.Message}", null);
      }
    }

    public async Task<(bool success, string message, int successCount, int failureCount, List<string> errors)> ImportCoursesFromFile(string filePath)
    {
      try
      {
        using (var context = new AppDbContext())
        {
          var courseRepository = new CourseRepository(context);
          var userRepository = new UserRepository(context);
          var studentRepository = new StudentRepository(context);
          var resultRepository = new ResultRepository(context);
          var idGeneratorService = new IdGeneratorService(context);
          
          var adminService = new AdminService(courseRepository, userRepository, studentRepository, resultRepository, idGeneratorService);
          
          var result = await adminService.importCourseFromFile(filePath);
          return (true, result.message, result.successCount, result.failureCount, result.errors);
        }
      }
      catch (Exception ex)
      {
        return (false, $"Error importing courses: {ex.Message}", 0, 0, new List<string> { ex.Message });
      }
    }

    public async Task<(bool success, string message, int enrolledCount, List<string> errors)> EnrollStudents(string courseId, List<string> studentIds)
    {
      int successCount = 0;
      List<string> errors = new List<string>();

      foreach (var studentId in studentIds)
      {
        try
        {
          using (var context = new AppDbContext())
          {
            var courseRepository = new CourseRepository(context);
            var userRepository = new UserRepository(context);
            var studentRepository = new StudentRepository(context);
            var resultRepository = new ResultRepository(context);
            var idGeneratorService = new IdGeneratorService(context);
            
            var adminService = new AdminService(courseRepository, userRepository, studentRepository, resultRepository, idGeneratorService);
            
            var request = new EnrollmentRequestDTO { studentId = studentId, courseId = courseId };
            await adminService.addStudentToCourse(request);
            successCount++;
          }
        }
        catch (Exception ex)
        {
          errors.Add($"Student {studentId}: {ex.Message}");
        }
      }

      if (successCount == studentIds.Count)
      {
        return (true, $"Successfully enrolled {successCount} student(s)", successCount, errors);
      }
      else if (successCount > 0)
      {
        return (true, $"Enrolled {successCount} of {studentIds.Count} students. {errors.Count} failed.", successCount, errors);
      }
      else
      {
        return (false, "Failed to enroll any students", successCount, errors);
      }
    }

    public async Task<List<CourseResponseDTO>> GetAllCourses()
    {
      using (var context = new AppDbContext())
      {
        var courseRepository = new CourseRepository(context);
        var courses = await courseRepository.getAll();
        return courses.Select(c => new CourseResponseDTO(c)).ToList();
      }
    }

    public async Task<List<StudentResponseDTO>> GetAllStudents()
    {
      using (var context = new AppDbContext())
      {
        var studentRepository = new StudentRepository(context);
        var students = await studentRepository.getAll();
        return students.Select(s => new StudentResponseDTO(s)).ToList();
      }
    }

    public async Task<List<StudentResponseDTO>> GetCourseEnrollments(string courseId)
    {
      using (var context = new AppDbContext())
      {
        var resultRepository = new ResultRepository(context);
        var studentRepository = new StudentRepository(context);
        
        var allResults = await resultRepository.getAll();
        var courseResults = allResults.Where(r => r.cid == courseId);
        
        var enrolledStudentIds = courseResults.Select(r => r.sid).Distinct();
        var allStudents = await studentRepository.getAll();
        var enrolledStudents = allStudents.Where(s => enrolledStudentIds.Contains(s.id));
        
        return enrolledStudents.Select(s => new StudentResponseDTO(s)).ToList();
      }
    }

    public async Task<(bool success, string message, string? userId)> CreateFacultyManager(string name, string username, string password, string facultyId)
    {
      try
      {
        using (var context = new AppDbContext())
        {
          var authService = new AuthenticationService(context, new IdGeneratorService(context));
          
          var request = new RegisterFManagerRequestDTO
          {
            name = name,
            username = username,
            password = password,
            facultyId = facultyId
          };
          
          var result = await authService.RegisterFManager(request);
          
          if (result.success)
          {
            return (true, $"Faculty Manager created successfully with ID: {result.userId}", result.userId);
          }
          else
          {
            return (false, result.message, null);
          }
        }
      }
      catch (Exception ex)
      {
        return (false, $"Error creating faculty manager: {ex.Message}", null);
      }
    }

    public async Task<(bool success, string message, string? userId)> CreateStudent(string name, string username, string password, string facultyId)
    {
      try
      {
        using (var context = new AppDbContext())
        {
          var authService = new AuthenticationService(context, new IdGeneratorService(context));
          
          var request = new RegisterStudentRequestDTO
          {
            name = name,
            username = username,
            password = password,
            facultyId = facultyId
          };
          
          var result = await authService.RegisterStudent(request);
          
          if (result.success)
          {
            return (true, $"Student created successfully with ID: {result.userId}", result.userId);
          }
          else
          {
            return (false, result.message, null);
          }
        }
      }
      catch (Exception ex)
      {
        return (false, $"Error creating student: {ex.Message}", null);
      }
    }

    public async Task<(bool success, string message, string? facultyId)> CreateFaculty(string name)
    {
      try
      {
        using (var context = new AppDbContext())
        {
          var courseRepository = new CourseRepository(context);
          var userRepository = new UserRepository(context);
          var studentRepository = new StudentRepository(context);
          var resultRepository = new ResultRepository(context);
          var idGeneratorService = new IdGeneratorService(context);
          
          var adminService = new AdminService(courseRepository, userRepository, studentRepository, resultRepository, idGeneratorService);
          
          var request = new FacultyRequestDTO { name = name };
          var result = await adminService.addFaculty(request);
          return (true, $"Faculty created successfully with ID: {result.id}", result.id);
        }
      }
      catch (Exception ex)
      {
        return (false, $"Error creating faculty: {ex.Message}", null);
      }
    }

    public async Task<List<FacultyResponseDTO>> GetAllFaculties()
    {
      using (var context = new AppDbContext())
      {
        var faculties = await context.Faculties.ToListAsync();
        return faculties.Select(f => new FacultyResponseDTO { id = f.id, name = f.name }).ToList();
      }
    }
  }
}
