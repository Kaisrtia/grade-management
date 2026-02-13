using GradeManagement.DTO.Request;
using GradeManagement.DTO.Response;

namespace GradeManagement.ServiceInterface {
  internal interface IAdminService {
    Task<int> addCourse(string cid, string name);
    Task<int> importCourseFromFile(string filePath);
    Task<int> addStudentToCourse(string Sid, string Cid);
    
    // User creation methods (admin-only)
    Task<AuthResponseDTO> CreateStudent(RegisterStudentRequestDTO request);
    Task<AuthResponseDTO> CreateFManager(RegisterFManagerRequestDTO request);
    Task<int> CreateFaculty(string name);
  }
}
