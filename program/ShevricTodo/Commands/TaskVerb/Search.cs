using ShevricTodo.Database;

namespace ShevricTodo.Commands.TaskObj;

internal partial class Search
{
	private static async Task SearchAndPrintTasksOfActiveUser(
		Func<TaskTodo, Task<IEnumerable<TaskTodo>>> searchTask,
		TaskTodo searchTemplate) => await SearchAndPrintTasksOfActiveUser(
			searchTask: searchTask,
			searchTemplate: searchTemplate,
			showMessage: Console.WriteLine,
			showTask: Show.ShowTask,
			showTasks: List.PrintTasks);
	public static async Task SearchContainsAndPrintTasksOfActiveUser(
		TaskTodo searchTemplate) => await SearchAndPrintTasksOfActiveUser(
			searchTemplate: searchTemplate,
			searchTask: SearchTasksContains);
	public static async Task SearchStartsWithAndPrintTasksOfActiveUser(
		TaskTodo searchTemplate) => await SearchAndPrintTasksOfActiveUser(
			searchTemplate: searchTemplate,
			searchTask: SearchTasksStartsWith);
	public static async Task SearchEndsWithAndPrintTasksOfActiveUser(
		TaskTodo searchTemplate) => await SearchAndPrintTasksOfActiveUser(
			searchTemplate: searchTemplate,
			searchTask: SearchTasksEndsWith);
	private static async Task SearchAndPrintTasks(
		Func<TaskTodo, Task<IEnumerable<TaskTodo>>> searchTask,
		TaskTodo searchTemplate) => await SearchAndPrintTasks(
			searchTask: searchTask,
			searchTemplate: searchTemplate,
			showMessage: Console.WriteLine,
			showTask: Show.ShowTask,
			showTasks: List.PrintTasks);
	public static async Task SearchContainsAndPrintTasks(
		TaskTodo searchTemplate) => await SearchAndPrintTasks(
			searchTask: SearchTasksContains,
			searchTemplate: searchTemplate);
	public static async Task SearchEndsWithAndPrintTasks(
		TaskTodo searchTemplate) => await SearchAndPrintTasks(
			searchTask: SearchTasksEndsWith,
			searchTemplate: searchTemplate);
	public static async Task SearchStartsWithAndPrintTasks(
		TaskTodo searchTemplate) => await SearchAndPrintTasks(
			searchTask: SearchTasksStartsWith,
			searchTemplate: searchTemplate);
}
