using GradeManagement.Config;
using GradeManagement.Model;
using GradeManagement.RepositoryInterface;
using MySql.Data.MySqlClient;

namespace GradeManagement.Repository
{
  internal class UserRepository : BaseRepository<User>, IUserRepository
  {
    // connect to database
    public async Task<User?> getUserByUsername(string username)
    {
      using (var conn = DatabaseConfig.GetConnection())
      {
        await conn.OpenAsync();
        string query = "SELECT * FROM user WHERE username = @username";

        using (var cmd = new MySqlCommand(query, conn))
        {
          cmd.Parameters.AddWithValue("@username", username);

          using (var reader = await cmd.ExecuteReaderAsync())
          {
            if (await reader.ReadAsync())
            {
              return new User
              {
                id = reader["id"].ToString()!,
                name = reader["name"].ToString()!,
                username = reader["username"].ToString()!,
                password = reader["password"].ToString()!,
                role = (Role)Enum.Parse(typeof(Role), reader["role"].ToString()!)
              };
            }
          }
        }
      }
      return null;
    }
  }
}
