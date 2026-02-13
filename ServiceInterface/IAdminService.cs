namespace GradeManagement.ServiceInterface {
  internal interface IAdminService {
    Task<int> addCourse(string cid, string name);

    Task<int> importCourseFromFile(string filePath);

    Task<int> addStudentToCourse(string Sid, string Cid);
  }
}
