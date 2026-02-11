using ShevricTodo.Database;

namespace ShevricTodo.Commands.TaskObj;

internal partial class Remove
{
	private static async Task<(int result, TaskTodo? deletedTaskTodo)> Done(
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
