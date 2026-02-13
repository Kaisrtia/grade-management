using GradeManagement.Entity;
using GradeManagement.Service;
using GradeManagement.Config;
using GradeManagement.Repository;
using GradeManagement.DTO.Request;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;

namespace GradeManagement
{
  internal static class Program
  {
    [STAThread]
    static void Main()
    {
      // Initialize database and create default admin
      using (var context = new AppDbContext())
      {
        context.Database.Migrate();
        var authenticationService = new AuthenticationService(context, new IdGeneratorService(context));
        var loginRequest = new LoginRequestDTO { username = "admin", password = "admin" };
        var loginResponse = authenticationService.Login(loginRequest);
        
        if (loginResponse != null)
        {
            MessageBox.Show($"Login successful: {loginResponse.Result.message}");
        }
      }

      // Start WinForms application
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new MainForm());
    }
  }
}
