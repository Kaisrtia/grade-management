namespace GradeManagement.DTO.Response
{
  public class ImportCoursesResponseDTO
  {
    public int successCount { get; set; }
    public int failureCount { get; set; }
    public List<string> errors { get; set; }
    public string message { get; set; }

    public ImportCoursesResponseDTO()
    {
      errors = new List<string>();
    }

    public ImportCoursesResponseDTO(int successCount, int failureCount, List<string> errors)
    {
      this.successCount = successCount;
      this.failureCount = failureCount;
      this.errors = errors ?? new List<string>();
      this.message = $"Imported {successCount} courses successfully. {failureCount} failed.";
    }
  }
}
