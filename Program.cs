using GradeManagement.Model;
using GradeManagement.Service;
internal class Program {
  private static void Main (string[] args) {
    Admin admin = new Admin ();
    System.Console.WriteLine (admin.role);
    Course math = new Course ("1", "Math");
    System.Console.WriteLine ($"Course ID: {math.id}, Course Name: {math.name}");
    
  }
}