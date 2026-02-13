using GradeManagement.Config;
using GradeManagement.DTO.Request;
using GradeManagement.DTO.Response;
using GradeManagement.Repository;
using GradeManagement.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GradeManagement.Controller
{
  public class StudentController
  {
    public StudentController()
    {
    }

    public async Task<List<ResultResponseDTO>> GetStudentGrades(string studentId)
    {
      using (var context = new AppDbContext())
      {
        var resultRepository = new ResultRepository(context);
        var courseRepository = new CourseRepository(context);
        var studentRepository = new StudentRepository(context);
        
        var studentService = new StudentService(resultRepository, courseRepository, studentRepository);
        
        var results = await studentService.showResult(studentId);
        return new List<ResultResponseDTO>(results);
      }
    }

    public async Task<(bool success, string message)> UpdateStudent(UserChangeInfoRequestDTO request)
    {
      try
      {
        using (var context = new AppDbContext())
        {
          var resultRepository = new ResultRepository(context);
          var courseRepository = new CourseRepository(context);
          var studentRepository = new StudentRepository(context);
          
          var studentService = new StudentService(resultRepository, courseRepository, studentRepository);
          
          bool updated = await studentService.updateStudent(request);
          if (updated)
          {
            return (true, "Student information updated successfully");
          }
          else
          {
            return (false, "Failed to update student information");
          }
        }
      }
      catch (ArgumentException ex)
      {
        return (false, ex.Message);
      }
      catch (Exception ex)
      {
        return (false, $"Error updating student: {ex.Message}");
      }
    }
  }
}
