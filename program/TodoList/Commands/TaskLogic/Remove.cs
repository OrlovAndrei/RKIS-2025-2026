using ShevricTodo.Database;

namespace ShevricTodo.Commands.TaskObj;

internal partial class Remove : TaskObj
{
	private static async Task<(int result, TaskTodo? deletedTaskTodo)> Done(
		Func<TaskTodo, Task<IEnumerable<TaskTodo>>> searchTaskTodo,
		Func<string, bool> inputBool,
		Func<Dictionary<int, string>,
			string?,
			int,
			KeyValuePair<int, string>> inputOneOf,
		Func<TaskTodo, Task> showTaskTodo,
		Action<string> showMessage,
		TaskTodo searchTemplate)
	{
		IEnumerable<TaskTodo> tasksTodo = await searchTaskTodo(searchTemplate);
		int result = 0;
		TaskTodo? preciseTask = null;
		switch (tasksTodo.Count())
		{
			case 0:
				showMessage("Ни одной задачи не было найдено.");
				break;
			case 1:
				preciseTask = tasksTodo.First();
				await showTaskTodo(preciseTask);
				if (inputBool("Хотите ли вы удалить эту задачу?"))
				{
					using (Todo db = new())
					{
						db.Tasks.RemoveRange(preciseTask);
						result = await db.SaveChangesAsync();
					}
				}
				break;
			default:
				preciseTask =
					await Search.Clarification(
						searchTaskTodo: searchTaskTodo,
						inputOneOf: inputOneOf,
						tasksTodo: tasksTodo,
						searchTemplate: searchTemplate);
				using (Todo db = new())
				{
					db.Tasks.RemoveRange(preciseTask);
					result = await db.SaveChangesAsync();
				}
				break;
		}
		return (result, preciseTask);
	}
}
