using System.ComponentModel.DataAnnotations;
using GradeManagement.Config;
using GradeManagement.RepositoryInterface;
using MySql.Data.MySqlClient;

namespace GradeManagement.Repository
{
  internal class BaseRepository<T> : IBaseRepository<T> where T : class, new()
  {
    protected string _tableName;

    public BaseRepository()
    {
      _tableName = typeof(T).Name.ToLower();
    }

    // Helper to validate data
    private void ValidateEntity(T entity)
    {
      var context = new ValidationContext(entity);
      var results = new List<ValidationResult>();

      bool isValid = Validator.TryValidateObject(entity, context, results, true);

      if (!isValid)
      {
        throw new ValidationException(results[0].ErrorMessage);
      }
    }

    // Helper to map ADO.NET DataReader results to C# Objects
    private bool ReaderHasColumn(MySqlDataReader reader, string columnName)
    {
      for (int i = 0; i < reader.FieldCount; i++)
      {
        if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase)) return true;
      }
      return false;
    }
    private T MapReaderToObject(MySqlDataReader reader)
    {
      T obj = new T();
      foreach (var prop in typeof(T).GetProperties())
      {
        // Check if column exists in result to avoid errors
        if (!ReaderHasColumn(reader, prop.Name)) continue;

        var val = reader[prop.Name];
        if (val != DBNull.Value)
        {
          if (prop.PropertyType.IsEnum)
          {
            prop.SetValue(obj, Enum.Parse(prop.PropertyType, val.ToString()!));
          }
          else
          {
            // Convert types if necessary (e.g. Int64 to Int32)
            prop.SetValue(obj, Convert.ChangeType(val, prop.PropertyType));
          }
        }
      }
      return obj;
    }

    // CRUD operation
    public async Task<int> create(T entity)
    {
      ValidateEntity(entity);
      using (var conn = DatabaseConfig.GetConnection())
      {
        await conn.OpenAsync();
        var properties = typeof(T).GetProperties();
        var columns = string.Join(", ", properties.Select(p => p.Name.ToLower()));
        var parameters = string.Join(", ", properties.Select(p => "@" + p.Name));
        string query = $"insert into {_tableName} ({columns}) values ({parameters})";
        using (var cmd = new MySqlCommand(query, conn))
        {
          foreach (var prop in properties)
          {
            var value = prop.GetValue(entity)!;

            if (prop.PropertyType.IsEnum) // change to string if enum type
            {
              cmd.Parameters.AddWithValue("@" + prop.Name, value.ToString());
            }
            else
            {
              cmd.Parameters.AddWithValue("@" + prop.Name, value ?? DBNull.Value);
            }
          }
          return await cmd.ExecuteNonQueryAsync();
        }
      }
    }

    public async Task<IEnumerable<T>> getAll()
    {
      var list = new List<T>();

      using (var conn = DatabaseConfig.GetConnection())
      {
        await conn.OpenAsync();

        // ADO.NET: Command Object
        string query = $"SELECT * FROM {_tableName}";
        using (var cmd = new MySqlCommand(query, conn))
        {
          // ADO.NET: DataReader Object
          using (var reader = await cmd.ExecuteReaderAsync())
          {
            while (await reader.ReadAsync())
            {
              list.Add(MapReaderToObject((MySqlDataReader)reader));
            }
          }
        }
      }
      return list;
    }

    public async Task<T?> getById(string id)
    {
      using (var conn = DatabaseConfig.GetConnection())
      {
        await conn.OpenAsync();
        string query = $"SELECT * FROM {_tableName} WHERE id = @id";

        using (var cmd = new MySqlCommand(query, conn))
        {
          // ADO.NET: Parameters to prevent SQL Injection
          cmd.Parameters.AddWithValue("@id", id);

          using (var reader = await cmd.ExecuteReaderAsync())
          {
            if (await reader.ReadAsync())
            {
              return MapReaderToObject((MySqlDataReader)reader);
            }
          }
        }
      }
      return null;
    }

    public async Task<int> update(T entity)
    {
      using (var conn = DatabaseConfig.GetConnection())
      {
        await conn.OpenAsync();

        var properties = typeof(T).GetProperties();
        var idProperty = properties.FirstOrDefault(p => p.Name.ToLower() == "id");

        if (idProperty == null) throw new Exception("Entity must have an 'id' property");

        var setClause = string.Join(", ", properties.Where(p => p.Name.ToLower() != "id").Select(p => $"{p.Name.ToLower()} = @{p.Name}"));
        string query = $"UPDATE {_tableName} SET {setClause} WHERE id = @id";

        using (var cmd = new MySqlCommand(query, conn))
        {
          foreach (var prop in properties)
          {
            cmd.Parameters.AddWithValue("@" + prop.Name, prop.GetValue(entity) ?? DBNull.Value);
          }
          return await cmd.ExecuteNonQueryAsync();
        }
      }
    }

    public async Task<int> delete(string id)
    {
      using (var conn = DatabaseConfig.GetConnection())
      {
        await conn.OpenAsync();
        string query = $"DELETE FROM {_tableName} WHERE id = @id";
        using (var cmd = new MySqlCommand(query, conn))
        {
          cmd.Parameters.AddWithValue("@id", id);
          return await cmd.ExecuteNonQueryAsync();
        }
      }
    }
  }
}
