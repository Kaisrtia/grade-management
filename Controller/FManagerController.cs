using GradeManagement.Config;
using GradeManagement.DTO.Request;
using GradeManagement.DTO.Response;
using GradeManagement.Entity;
using GradeManagement.Repository;
using GradeManagement.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GradeManagement.Controller
{
  public class FManagerController
  {
    public FManagerController()
    {
    }

    public async Task<List<ResultResponseDTO>> GetFacultyStudentGrades(string managerId)
    {
      using (var context = new AppDbContext())
      {
        // Get the manager's faculty ID
        var manager = await context.Set<FManager>().FindAsync(managerId);
        if (manager == null || string.IsNullOrEmpty(manager.fid))
        {
          return new List<ResultResponseDTO>();
        }

        // Get all students in this faculty
        var students = await context.Set<Student>()
          .Where(s => s.fid == manager.fid)
          .ToListAsync();

        var studentIds = students.Select(s => s.id).ToHashSet();

        // Get all results for students in this faculty
        var results = await context.Set<Result>()
          .Include(r => r.Student)
          .Include(r => r.Course)
          .Where(r => studentIds.Contains(r.sid))
          .ToListAsync();

        return results.Select(r => new ResultResponseDTO(r)).ToList();
      }
    }

    public async Task<(bool success, string message)> UpdateStudentGrade(string managerId, ResultRequestDTO request)
    {
      try
      {
        using (var context = new AppDbContext())
        {
          // Verify manager and get their faculty
          var manager = await context.Set<FManager>().FindAsync(managerId);
          if (manager == null || string.IsNullOrEmpty(manager.fid))
          {
            return (false, "Faculty Manager not found or not assigned to a faculty");
          }

          // Verify student belongs to the manager's faculty
          var student = await context.Set<Student>().FindAsync(request.sid);
          if (student == null)
          {
            return (false, "Student not found");
          }

          if (student.fid != manager.fid)
          {
            return (false, "You can only modify grades for students in your faculty");
          }

          // Use the FManagerService to update the grade
          var resultRepository = new ResultRepository(context);
          var courseRepository = new CourseRepository(context);
          var studentRepository = new StudentRepository(context);
          
          var fManagerService = new FManagerService(studentRepository, courseRepository, resultRepository);
          
          var result = await fManagerService.updateResult(request);
          return (true, $"Grade updated successfully for {result.studentName} in {result.courseName}");
        }
      }
      catch (ArgumentException ex)
      {
        return (false, ex.Message);
      }
      catch (InvalidOperationException ex)
      {
        return (false, ex.Message);
      }
      catch (Exception ex)
      {
        return (false, $"Error updating grade: {ex.Message}");
      }
    }
  }
}
