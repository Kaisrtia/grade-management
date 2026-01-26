namespace GradeManagement.ServiceInterface {
  internal interface IAdminService {
    Task<int> addCourse (string Cid, string Cname);

    Task<int> importCourseFromFile(string filePath);

    Task<int> addStudentToCourse(string Sid, string Cid);
  }
}
