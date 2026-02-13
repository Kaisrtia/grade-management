namespace GradeManagement.ServiceInterface
{
  public interface IIdGenerator
  {
    Task<string> GenerateFacultyId(string facultyName);
    Task<string> GenerateAdminId();
    Task<string> GenerateFManagerId();
    Task<string> GenerateStudentId(string facultyId);
    Task<string> GenerateCourseId();
    Task<string> GenerateResultId(string courseId);
  }
}
