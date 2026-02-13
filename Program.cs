using GradeManagement.Entity;
using GradeManagement.Service;
using GradeManagement.Config;
using GradeManagement.Repository;
using GradeManagement.DTO.Request;
using GradeManagement.Forms;
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
        // Login successful, route based on role
        User loggedInUser = loginForm.LoggedInUser;
        
        Form mainForm;
        switch (loggedInUser.role)
        {
          case Role.ADMIN:
            mainForm = new AdminMenuForm(loggedInUser);
            break;
          case Role.FMANAGER:
            // TODO: Create FManagerMenuForm
            mainForm = new MainForm(loggedInUser);
            break;
          case Role.STUDENT:
            // TODO: Create StudentMenuForm
            mainForm = new MainForm(loggedInUser);
            break;
          default:
            mainForm = new MainForm(loggedInUser);
            break;
        }
        
        Application.Run(mainForm);
      }
      // If login cancelled or failed, application exits
    }
  }
}
