using GradeManagement.DTO.Request;
using GradeManagement.DTO.Response;
using GradeManagement.RepositoryInterface;
using GradeManagement.ServiceInterface;
using Microsoft.EntityFrameworkCore;

namespace GradeManagement.Service {
  internal class FManagerService : IFManagerService {
    IStudentRepository studentRepository = null!;
    ICourseRepository courseRepository = null!;
    IResultRepository resultRepository = null!;

    public FManagerService (IStudentRepository studentRepository, ICourseRepository courseRepository, IResultRepository resultRepository) {
      this.studentRepository = studentRepository;
      this.courseRepository = courseRepository;
      this.resultRepository = resultRepository;
    }

    public async Task<ResultResponseDTO> updateResult (ResultRequestDTO request) {
      // Validate student exists
      var student = await studentRepository.getById(request.sid);
      if (student == null) {
        throw new ArgumentException($"Student with ID '{request.sid}' does not exist");
      }

      // Validate course exists
      var course = await courseRepository.getById(request.cid);
      if (course == null) {
        throw new ArgumentException($"Course with ID '{request.cid}' does not exist");
      }

      // Validate enrollment exists
      var existingResults = await resultRepository.getResult(request.sid, request.cid);
      var existingResult = existingResults.FirstOrDefault();
      
      if (existingResult == null) {
        throw new InvalidOperationException($"Enrollment not found for student '{request.sid}' in course '{request.cid}'");
      }

      // Update the grade
      existingResult.grade = request.grade;
      await resultRepository.update(existingResult);

      // Load navigation properties for response
      existingResult.Student = student;
      existingResult.Course = course;

      return new ResultResponseDTO(existingResult);
    }
  }
}
