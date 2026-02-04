using Microsoft.EntityFrameworkCore;
using ShevricTodo.Authentication;
using ShevricTodo.Database;
using ShevricTodo.Parser;
using static System.Console;

namespace ShevricTodo;

internal class Program
{
	public static async Task Main(string[] args)
	{
		using (Todo db = new())
		{
			if (await db.Database.EnsureCreatedAsync()) // true если DB не было и false если была
			{
				if (!File.Exists(ActiveProfile.PathToProfile) ||
				await ActiveProfile.Read() is null)// true если чтение профиля завершится неудачей и вернется null
				{
					(int result, Profile newProfile) = await Commands.Profile.Add.Done(
						inputString: Input.Text.ShortText,
						inputDateTime: Input.When.Date,
						inputBool: Input.Button.YesOrNo,
						inputPassword: Input.Password.CheckingThePassword);
					if (result <= 0) // если не было создано ни одного профиля
					{
						throw new FileLoadException();
					}
					await ActiveProfile.Update(newProfile);
				}
			}
			else if (db.Database.HasPendingModelChanges()) // проверяет старая ли ДБ и соответствует ли она актуальной модели ef core
			{
				try
				{
					await db.Database.MigrateAsync();
				}
				catch (Microsoft.Data.Sqlite.SqliteException ex)
				{
					WriteLine(ex);
					throw new InvalidOperationException(@"
Скорее всего были изменении в модели базы данных и вам надо создать новую миграцию.
Команда:
dotnet ef migrations add <название_миграции>
И применить изменения.
Команда:
dotnet ef database update");
				}
			}
		}
		Parse.Run(args: args);
	}
	public static async Task Run()
	{
		int cycles = 0;
		while (true)
		{
			Write("> ");
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
