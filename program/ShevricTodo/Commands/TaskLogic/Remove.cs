using ShevricTodo.Database;
using TodoList.Commands;

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
		TaskTodo? preciseTask = await Logic<TaskTodo>.ProcessQuantity(
			items: tasksTodo,
			ifTheQuantityIsZero: TaskNotFound,
			ifTheQuantityIsOne: TaskIsOne,
			ifTheQuantityIsMany: TasksIsMore
		);
		int result = preciseTask is not null ? 1 : 0;
		return (result, preciseTask);

		async Task<TaskTodo?> TaskNotFound()
		{
			showMessage("Профиль не был найден.");
			return null;
		}
		async Task<TaskTodo?> TaskIsOne()
		{
			TaskTodo preciseTask = tasksTodo.First();
			await showTaskTodo(preciseTask);
			if (inputBool("Хотите ли вы удалить эту задачу?"))
			{
				await RemoveTask(task: preciseTask);
				return preciseTask;
			}
			return null;
		}
		async Task<TaskTodo?> TasksIsMore()
		{
			TaskTodo preciseTask =
				await Search.Clarification(
					searchTaskTodo: searchTaskTodo,
					inputOneOf: inputOneOf,
					tasksTodo: tasksTodo,
					searchTemplate: searchTemplate);
			await RemoveTask(task: preciseTask);
			return preciseTask;
		}
	}
}
