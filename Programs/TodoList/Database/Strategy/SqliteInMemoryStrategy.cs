using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TodoList.Interfaces;

namespace Infrastructure.Database.Strategy;

public class SqliteInMemoryStrategy : IConnectionStrategy, IDisposable
{
    private const string _connectionString = "DataSource=:memory:";
    private readonly SqliteConnection _sqliteConnection;

    public SqliteInMemoryStrategy()
    {
        _sqliteConnection = new(_connectionString);
        _sqliteConnection.Open();
    }

    public void Configure(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(_sqliteConnection);
    }

    public void Dispose()
    {
        _sqliteConnection.Dispose();
    }
}