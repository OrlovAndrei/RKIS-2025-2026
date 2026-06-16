using Microsoft.EntityFrameworkCore;
using TodoList.Interfaces;

namespace Infrastructure.Database.Strategy;

public class SqliteStrategy : IConnectionStrategy
{
    private readonly string _connectionString;
    
    public SqliteStrategy(string connectionString)
    {
        _connectionString = connectionString;
    }
	public void Configure(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlite(_connectionString);
	}
}