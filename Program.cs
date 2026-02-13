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
      // Initialize database
      using (var context = new AppDbContext())
      {
        context.Database.Migrate();
      }

      // Start WinForms application
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      
      // Show login form first
      var loginForm = new LoginForm();
      if (loginForm.ShowDialog() == DialogResult.OK)
      {
        // Login successful, open main form
        Application.Run(new MainForm(loginForm.LoggedInUser));
      }
      // If login cancelled or failed, application exits
    }
  }
}
