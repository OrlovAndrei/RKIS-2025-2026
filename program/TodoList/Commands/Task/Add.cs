using ShevricTodo.Authentication;
using ShevricTodo.Database;

namespace ShevricTodo.Commands.Task;

internal class Add
{
	public static async Task<int> AddNew(
		TaskTodo newTask)
	{
		using (Todo db = new())
		{
			await db.Tasks.AddAsync(newTask);
			return await db.SaveChangesAsync();
		}
	}
	public static async Task<int> AddNew(
		TaskTodo[] newTasks)
	{
		using (Todo db = new())
		{
			await db.Tasks.AddRangeAsync(newTasks);
			return await db.SaveChangesAsync();
		}
	}
	public static async Task<(int resultSave, TaskTodo taskTodo)> Done(
		Func<string, string?> inputStringShort,
		Func<string, string?> inputStringLong,
		Func<string, DateTime?> inputDateTime,
		Func<string, bool> inputBool,
		Func<Dictionary<int, string>,
			IDictionary<int, string>> inputOneOf,
		string? name = null,
		string? description = null,
		DateTime? deadline = null)
	{
		DateTime nowDateTime = DateTime.Now;
		TaskTodo newTask = new()
		{
			Name = name ?? inputStringShort("Введите название задачи: "),
			StateId = inputOneOf(await GetAllStates()).First().Key,
			TypeId = inputOneOf(await GetAllTypes()).First().Key,
			Description = description ?? inputStringLong("Введите описание задачи: "),
			Deadline = deadline ?? (inputBool("Желаете ввести крайний срок на выполнение задачи? ")
				? inputDateTime("Введите крайний срок на выполнение задачи")
				: null),
			DateOfCreate = nowDateTime,
			UserId = ActiveProfile.Read().Id,
		};
		return (await AddNew(newTask), newTask);
	}
	public static async Task<Dictionary<int, string>> GetAllStates()
	{
		using (Todo db = new())
		{
			return (Dictionary<int, string>)
				(from stale in db.StatesOfTask
				 select new
				 {
					 stale.StateId,
					 stale.Name
				 });
		}
	}
	public static async Task<Dictionary<int, string>> GetAllTypes()
	{
		using (Todo db = new())
		{
			return (Dictionary<int, string>)
				(from type in db.TypesOfTasks
				 select new
				 {
					 type.TypeId,
					 type.Name
				 });
		}
	}
}
