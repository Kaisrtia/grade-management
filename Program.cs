using GradeManagement.Model;
using GradeManagement.Service;
using GradeManagement.Config;
using MySql.Data.MySqlClient;
using GradeManagement.Repository;
using System.Windows.Forms; // Add this for WinForms

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
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new MainForm());
    }
  }
}