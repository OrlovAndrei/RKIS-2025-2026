using Infrastructure.Database.Strategy;
using TodoList.Infrastructure;

namespace TodoList.Presentation.WebApi.DependencyInjection;

public class SqliteStrategyDi : SqliteStrategy
{
    private const string dataDirectory = "data";
    private static readonly string _todosFilePath = Path.Combine(dataDirectory, "todo.db");
    public SqliteStrategyDi() : base($"DataSource={_todosFilePath}")
    {
        FileManager.EnsureDataDirectory(dataDirectory);
        FileManager.EnsureDataFile(_todosFilePath);
    }
}
