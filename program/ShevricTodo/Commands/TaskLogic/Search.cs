using ShevricTodo.Authentication;
using ShevricTodo.Database;
namespace ShevricTodo.Commands.TaskObj;

internal static partial class Search
{
	public static async Task<IEnumerable<TaskTodo>> SearchTasksContains(
		TaskTodo searchTemplate)
	{
		using (Todo db = new())
		{
			return await db.Tasks
				.StartFilter()
				.FilterTasksContainsAsync(searchTemplate: searchTemplate)
				.FilterDateEqualsAsync(searchTemplate: searchTemplate)
				.FilterIdEqualsAsync(searchTemplate: searchTemplate)
				.FinishFilter();
		}
	}
	public static async Task<IEnumerable<TaskTodo>> SearchTasksStartsWith(
		TaskTodo searchTemplate)
	{
		using (Todo db = new())
		{
			return await db.Tasks
				.StartFilter()
				.FilterTasksStartsWithAsync(searchTemplate: searchTemplate)
				.FilterDateEqualsAsync(searchTemplate: searchTemplate)
				.FilterIdEqualsAsync(searchTemplate: searchTemplate)
				.FinishFilter();
		}
	}
	public static async Task<IEnumerable<TaskTodo>> SearchTasksEndsWith(
		TaskTodo searchTemplate)
	{
		using (Todo db = new())
		{
			return await db.Tasks
				.StartFilter()
				.FilterTasksEndsWithAsync(searchTemplate: searchTemplate)
				.FilterDateEqualsAsync(searchTemplate: searchTemplate)
				.FilterIdEqualsAsync(searchTemplate: searchTemplate)
				.FinishFilter();
		}
	}
	private static async Task SearchAndPrintTasksOfActiveUser(
		Func<TaskTodo, Task<IEnumerable<TaskTodo>>> searchTask,
		Action<string> showMessage,
		Func<TaskTodo, Task> showTask,
		Func<IEnumerable<TaskTodo>, Task> showTasks,
		TaskTodo searchTemplate)
	{
		searchTemplate.UserId = (await ActiveProfile.GetActiveProfile()).UserId;
		await SearchAndPrintTasks(
			searchTask, showMessage, showTask, showTasks, searchTemplate);
	}
	private static async Task SearchAndPrintTasks(
		Func<TaskTodo, Task<IEnumerable<TaskTodo>>> searchTask,
		Action<string> showMessage,
		Func<TaskTodo, Task> showTask,
		Func<IEnumerable<TaskTodo>, Task> showTasks,
		TaskTodo searchTemplate)
	{
		IEnumerable<TaskTodo> tasks = await searchTask(searchTemplate);
		switch (tasks.Count())
		{
			case 0:
				showMessage("Нет ни одной похожей задачи.");
				break;
			case 1:
				await showTask(tasks.First());
				break;
			default:
				await showTasks(tasks);
				break;
		}
	}
	public static async Task<TaskTodo> Clarification(
		Func<TaskTodo, Task<IEnumerable<TaskTodo>>> searchTaskTodo,
		Func<Dictionary<int, string>,
			string?,
			int,
			KeyValuePair<int, string>> inputOneOf,
		TaskTodo searchTemplate,
		IEnumerable<TaskTodo> tasksTodo)
	{
		KeyValuePair<int, string> taskIdAndName = inputOneOf(
			(Dictionary<int, string>)
			(from task in tasksTodo
			 select new { task.TaskId, task.Name }),
			"Какую задачу вы хотите удалить?", 5);
		searchTemplate.TaskId = taskIdAndName.Key;
		searchTemplate.Name = taskIdAndName.Value;
		return (await searchTaskTodo(searchTemplate)).First();
	}
}
