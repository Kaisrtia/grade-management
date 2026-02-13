using GradeManagement.DTO.Request;
using GradeManagement.DTO.Response;

namespace GradeManagement.ServiceInterface {
  internal interface IAdminService {
    Task<CourseResponseDTO> addCourse(CourseRequestDTO request);

    Task<ImportCoursesResponseDTO> importCourseFromFile(string filePath);

    Task<EnrollmentResponseDTO> addStudentToCourse(EnrollmentRequestDTO request);
  }
}
