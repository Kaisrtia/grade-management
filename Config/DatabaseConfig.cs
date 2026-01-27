using MySql.Data.MySqlClient;

namespace GradeManagement.Config
{
	internal class DatabaseConfig
	{
		private static string ConnectionString = "Server=localhost;Database=grademanagement;Uid=root;Pwd=root;";
		public static MySqlConnection GetConnection()
		{
			return new MySqlConnection(ConnectionString);
		}
	}
}


