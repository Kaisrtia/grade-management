using GradeManagement.Entity;
using GradeManagement.Service;
using GradeManagement.Config;
using MySql.Data.MySqlClient;
using GradeManagement.Repository;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore; // For Migrate() method

namespace GradeManagement // Namespace is usually recommended for WinForms
{
  internal static class Program
  {
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread] // Required for WinForms
    static void Main()
    {
      // Initialize Entity Framework Core DbContext and apply migrations
      using (var context = new AppDbContext())
      {
        try
        {
          // Apply pending migrations (creates database if not exists, updates schema if needed)
          context.Database.Migrate();
          
          MessageBox.Show(
            "Database initialized successfully!\n\n" +
            "All migrations have been applied.",
            "Success",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
          MessageBox.Show(
            $"Database initialization failed!\n\n" +
            $"Error: {ex.Message}\n\n" +
            $"Stack Trace:\n{ex.StackTrace}\n\n" +
            $"Inner Exception: {ex.InnerException?.Message}",
            "Error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);
        }
      }

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new MainForm());
    }
  }
}