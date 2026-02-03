using Microsoft.EntityFrameworkCore;
using ShevricTodo.Database;
using ShevricTodo.Parser;
using static System.Console;

namespace shevricTodo;

internal class Program
{
	public static async Task Main(string[] args)
	{
		using (Todo db = new())
		{
			if (await db.Database.EnsureCreatedAsync()) // true если DB не было и false если была
			{
				//происходит первичная регистрация пользователя
			}
			if (db.Database.HasPendingModelChanges())
			{
				throw new InvalidOperationException(@"
Скорее всего были изменении в модели базы данных и вам надо создать новую миграцию.
Команда:
dotnet ef migrations add <название_миграции>");
			}
			await db.Database.MigrateAsync();
		}
		Parse.Run(args: args);
	}
	public static async Task Run()
	{
		int cycles = 0;
		while (true)
		{
			string inputTerminal = ReadLine() ?? "--help";
			string[] args = inputTerminal.Split(
				separator: " ",
				options: StringSplitOptions.TrimEntries |
				StringSplitOptions.RemoveEmptyEntries);
			await Main(args: args);
			cycles++;
		}
	}
}
