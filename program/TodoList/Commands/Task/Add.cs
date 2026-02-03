using ShevricTodo.Database;

namespace ShevricTodo.Commands.Task;

internal class Add
{
	public static async Task<int> AddNewTask(
		TaskTodo newTask)
	{
		using (Todo db = new())
		{
			await db.Tasks.AddAsync(newTask);
			return await db.SaveChangesAsync();
		}
	}
	public static async Task<int> AddNewTask(
		TaskTodo[] newTasks)
	{
		using (Todo db = new())
		{
			await db.Tasks.AddRangeAsync(newTasks);
			return await db.SaveChangesAsync();
		}
	}
}
