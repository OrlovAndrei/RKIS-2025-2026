using ShevricTodo.Database;

namespace ShevricTodo.Commands.TaskObj;

internal partial class Edit
{
	private static async Task<(int result, TaskTodo? deletedTaskTodo)> Done(
	Func<TaskTodo, Task<IEnumerable<TaskTodo>>> searchTaskTodo,
	TaskTodo searchTemplate,
	TaskTodo updateTemplate) => await Done(
			searchTaskTodo: searchTaskTodo,
			inputBool: Input.Button.YesOrNo,
			inputOneOf: Input.OneOf.GetOneFromList,
			showTaskTodo: Show.ShowTask,
			showMessage: Console.WriteLine,
			searchTemplate: searchTemplate,
			updateTemplate: updateTemplate
		);
	private static async Task<(int result, TaskTodo? deletedTaskTodo)> DoneContains(
		TaskTodo searchTemplate,
		TaskTodo updateTemplate) => await Done(
			searchTaskTodo: Search.SearchTasksContains,
			searchTemplate: searchTemplate,
			updateTemplate: updateTemplate
		);
	private static async Task<(int result, TaskTodo? deletedTaskTodo)> DoneEndsWith(
		TaskTodo searchTemplate,
		TaskTodo updateTemplate) => await Done(
			searchTaskTodo: Search.SearchTasksEndsWith,
			searchTemplate: searchTemplate,
			updateTemplate: updateTemplate
		);
	private static async Task<(int result, TaskTodo? deletedTaskTodo)> DoneStartsWith(
		TaskTodo searchTemplate,
		TaskTodo updateTemplate) => await Done(
			searchTaskTodo: Search.SearchTasksStartsWith,
			searchTemplate: searchTemplate,
			updateTemplate: updateTemplate
		);
}