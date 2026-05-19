using Microsoft.Data.Sqlite;

namespace TodoList.Server
{
	public class TodoRepository
	{
		private readonly Database _db;

		public TodoRepository(Database db) => _db = db;

		// создание таблиц
		public async Task InitializeAsync()
		{
			using var connection = _db.CreateConnection();
			await connection.OpenAsync();

			var command = connection.CreateCommand();
			command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Users (Id TEXT PRIMARY KEY, Data BLOB);
                CREATE TABLE IF NOT EXISTS Todos (UserId TEXT PRIMARY KEY, Data BLOB);";
			await command.ExecuteNonQueryAsync();

			Console.WriteLine("=== БАЗА ДАННЫХ ИНИЦИАЛИЗИРОВАНА ===");
		}
	

	public async Task SaveDataAsync(string table, string id, byte[] data)
		{
			using var connection = _db.CreateConnection();
			await connection.OpenAsync();

			var command = connection.CreateCommand();
			string idColumn = (table == "Users") ? "Id" : "UserId";

			command.CommandText = $"INSERT OR REPLACE INTO {table} ({idColumn}, Data) VALUES (@id, @data)";
			command.Parameters.AddWithValue("@id", id);
			command.Parameters.AddWithValue("@data", data);

			await command.ExecuteNonQueryAsync();
		}

		public async Task<byte[]> GetDataAsync(string table, string id)
		{
			using var connection = _db.CreateConnection();
			await connection.OpenAsync();

			var command = connection.CreateCommand();

			string idColumn = (table == "Users") ? "Id" : "UserId";

			command.CommandText = $"SELECT Data FROM {table} WHERE {idColumn} = @id";
			command.Parameters.AddWithValue("@id", id);

			using var reader = await command.ExecuteReaderAsync();
			if (await reader.ReadAsync())
			{
				return (byte[])reader["Data"];
			}
			return null;
		}
	}
}
