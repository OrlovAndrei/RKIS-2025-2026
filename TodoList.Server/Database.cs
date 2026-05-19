using Microsoft.Data.Sqlite;

namespace TodoList.Server
{

	public class Database
	{
		public string ConnectionString { get; }

		public Database(string connectionString)
		{
			ConnectionString = connectionString;
		}

		public SqliteConnection CreateConnection() => new SqliteConnection(ConnectionString);
	}
}