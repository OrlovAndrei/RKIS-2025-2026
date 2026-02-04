using ShevricTodo.Authentication;
using ShevricTodo.Database;

namespace ShevricTodo.Commands.Task;

internal class List
{
	public static async Task<IEnumerable<TaskTodo>> GetAllTasks()
	{
		Database.Profile user = await ActiveProfile.GetActiveProfile();
		using (Todo db = new())
		{
			return from task in db.Tasks
				   where task.UserId == user.UserId
				   orderby task.TaskId
				   select task;
		}
	}
}
