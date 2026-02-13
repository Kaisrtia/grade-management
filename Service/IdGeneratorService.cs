using GradeManagement.Config;
using GradeManagement.ServiceInterface;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace GradeManagement.Service
{
  public class IdGeneratorService : IIdGenerator
  {
    private readonly AppDbContext _context;

    public IdGeneratorService(AppDbContext context)
    {
      _context = context;
    }

    public async Task<string> GenerateFacultyId(string facultyName)
    {
      // Format: {NAME}Fxxx (e.g., COMPUTERSCIENEF001)
      string prefix = facultyName.ToUpper().Replace(" ", "");
      string pattern = $"{prefix}F";

      var existingIds = await _context.Faculties
        .Where(f => f.id.StartsWith(pattern))
        .Select(f => f.id)
        .ToListAsync();

      int maxNumber = 0;
      foreach (var id in existingIds)
      {
        var match = Regex.Match(id, pattern + @"(\d+)$");
        if (match.Success && int.TryParse(match.Groups[1].Value, out int number))
        {
          if (number > maxNumber) maxNumber = number;
        }
      }

      return $"{pattern}{(maxNumber + 1):D3}";
    }

    public async Task<string> GenerateAdminId()
    {
      // Format: ADMxxx (e.g., ADM001)
      string pattern = "ADM";

      var existingIds = await _context.Admins
        .Where(a => a.id.StartsWith(pattern))
        .Select(a => a.id)
        .ToListAsync();

      int maxNumber = 0;
      foreach (var id in existingIds)
      {
        var match = Regex.Match(id, pattern + @"(\d+)$");
        if (match.Success && int.TryParse(match.Groups[1].Value, out int number))
        {
          if (number > maxNumber) maxNumber = number;
        }
      }

      return $"{pattern}{(maxNumber + 1):D3}";
    }

    public async Task<string> GenerateFManagerId()
    {
      // Format: FMxxx (e.g., FM001)
      string pattern = "FM";

      var existingIds = await _context.FManagers
        .Where(fm => fm.id.StartsWith(pattern))
        .Select(fm => fm.id)
        .ToListAsync();

      int maxNumber = 0;
      foreach (var id in existingIds)
      {
        var match = Regex.Match(id, pattern + @"(\d+)$");
        if (match.Success && int.TryParse(match.Groups[1].Value, out int number))
        {
          if (number > maxNumber) maxNumber = number;
        }
      }

      return $"{pattern}{(maxNumber + 1):D3}";
    }

    public async Task<string> GenerateStudentId(string facultyId)
    {
      // Format: {FACULTY_ID}STDxxx (e.g., COMPUTERSCIENEF001STD001)
      string pattern = $"{facultyId}STD";

      var existingIds = await _context.Students
        .Where(s => s.id.StartsWith(pattern))
        .Select(s => s.id)
        .ToListAsync();

      int maxNumber = 0;
      foreach (var id in existingIds)
      {
        var match = Regex.Match(id, pattern + @"(\d+)$");
        if (match.Success && int.TryParse(match.Groups[1].Value, out int number))
        {
          if (number > maxNumber) maxNumber = number;
        }
      }

      return $"{pattern}{(maxNumber + 1):D3}";
    }

    public async Task<string> GenerateCourseId()
    {
      // Format: Cxxx (e.g., C001)
      string pattern = "C";

      var existingIds = await _context.Courses
        .Where(c => c.id.StartsWith(pattern))
        .Select(c => c.id)
        .ToListAsync();

      int maxNumber = 0;
      foreach (var id in existingIds)
      {
        var match = Regex.Match(id, pattern + @"(\d+)$");
        if (match.Success && int.TryParse(match.Groups[1].Value, out int number))
        {
          if (number > maxNumber) maxNumber = number;
        }
      }

      return $"{pattern}{(maxNumber + 1):D3}";
    }

    public async Task<string> GenerateResultId(string courseId)
    {
      // Format: {COURSE_ID}RSxxx (e.g., C001RS001)
      string pattern = $"{courseId}RS";

      // Note: Result doesn't have a single ID field, it uses composite key (sid, cid)
      // This method is provided for potential future use if needed
      // For now, we'll return a conceptual ID based on the pattern
      
      // Since Result uses composite key, we might not use this directly
      // But keeping it for consistency with the specification
      return $"{pattern}001"; // Placeholder implementation
    }
  }
}
