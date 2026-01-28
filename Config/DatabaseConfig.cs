using MySql.Data.MySqlClient;

namespace GradeManagement.Config
{
	internal class DatabaseConfig
	{
		private static string ConnectionString = "Server=localhost;Port=3307;Database=grade_management;Uid=root;Pwd=1;";
		public static MySqlConnection GetConnection()
		{
			return new MySqlConnection(ConnectionString);
		}
	}
}


