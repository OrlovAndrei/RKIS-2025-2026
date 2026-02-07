using ShevricTodo.Database;

namespace ShevricTodo.Commands.TaskVerb;

internal class Remove : TaskObj
{
	public static async Task<(int result, TaskTodo? deletedTaskTodo)> Done(
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
		switch (tasksTodo.Count())
		{
			case 0:
				showMessage("Ни одной задачи не было найдено.");
				break;
			case 1:
				await showTaskTodo(tasksTodo.First());
				if (inputBool("Хотите ли вы удалить эту задачу?"))
				{
					using (Todo db = new())
					{
						db.Tasks.RemoveRange(tasksTodo);
						result = await db.SaveChangesAsync();
						return (result, tasksTodo.First());
					}
				}
				break;
			default:
				TaskTodo preciseTask =
					await Search.Clarification(
						searchTaskTodo: searchTaskTodo,
						inputOneOf: inputOneOf,
						tasksTodo: tasksTodo,
						searchTemplate: searchTemplate);
				using (Todo db = new())
				{
					db.Tasks.RemoveRange(tasksTodo);
					result = await db.SaveChangesAsync();
					return (result, tasksTodo.First());
				}
		}
		return (result, null);
	}
	public static async Task<(int result, TaskTodo? deletedTaskTodo)> Done(
		Func<TaskTodo, Task<IEnumerable<TaskTodo>>> searchTaskTodo,
		TaskTodo searchTemplate)
	{
		return await Done(searchTaskTodo: searchTaskTodo,
		   searchTemplate: searchTemplate,
		   inputBool: Input.Button.YesOrNo,
		   inputOneOf: Input.OneOf.GetOneFromList,
		   showTaskTodo: Show.ShowTask,
		   showMessage: Console.WriteLine);
	}
	public static async Task<(int result, TaskTodo? deletedTaskTodo)> DoneContains(
		TaskTodo searchTemplate)
	{
		return await Done(searchTaskTodo: Search.SearchTasksContains,
		   searchTemplate: searchTemplate);
	}
	public static async Task<(int result, TaskTodo? deletedTaskTodo)> DoneStartsWith(
		TaskTodo searchTemplate)
	{
		return await Done(searchTaskTodo: Search.SearchTasksStartsWith,
		   searchTemplate: searchTemplate);
	}
	public static async Task<(int result, TaskTodo? deletedTaskTodo)> DoneEndsWith(
		TaskTodo searchTemplate)
	{
		return await Done(searchTaskTodo: Search.SearchTasksEndsWith,
		   searchTemplate: searchTemplate);
	}
}
