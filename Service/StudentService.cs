using GradeManagement.DTO.Response;
using GradeManagement.Entity;
using GradeManagement.RepositoryInterface;
using GradeManagement.ServiceInterface;
using Microsoft.EntityFrameworkCore;

namespace GradeManagement.Service {
  internal class StudentService : IStudentService {
    IResultRepository resultRepository = null!;
    ICourseRepository courseRepository = null!;
    IStudentRepository studentRepository = null!;

    public StudentService (IResultRepository resultRepository, ICourseRepository courseRepository, IStudentRepository studentRepository) {
      this.resultRepository = resultRepository;
      this.courseRepository = courseRepository;
      this.studentRepository = studentRepository;
    }

    public async Task<IEnumerable<ResultResponseDTO>> showResult (string studentId) {
      // Validate student exists
      var student = await studentRepository.getById(studentId);
      if (student == null) {
        throw new ArgumentException($"Student with ID '{studentId}' does not exist");
      }

      // Get all results for the student (with navigation properties loaded)
      var results = await resultRepository.getResultsByStudent(studentId);

      // Map to DTOs
      return results.Select(r => new ResultResponseDTO(r)).ToList();
    }
  }
}
