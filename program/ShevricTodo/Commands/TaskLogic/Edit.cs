using ShevricTodo.Database;
using TodoList.Commands;

namespace ShevricTodo.Commands.TaskObj;

internal partial class Edit : TaskObj
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
	TaskTodo searchTemplate,
	TaskTodo updateTemplate)
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
			showMessage("Задача не была найдена.");
			return null;
		}
		async Task<TaskTodo?> TaskIsOne()
		{
			TaskTodo preciseTask = tasksTodo.First();
			await showTaskTodo(preciseTask);
			if (inputBool("Хотите ли вы изменить эту задачу?"))
			{
				await UpdateTask(task: preciseTask, updateTemplate: updateTemplate);
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
			await UpdateTask(task: preciseTask, updateTemplate: updateTemplate);
			return preciseTask;
		}
	}
	private static async Task<int> UpdateTask(TaskTodo task, TaskTodo updateTemplate)
	{
		if (string.IsNullOrWhiteSpace(updateTemplate.Name) is false)
		{
			task.Name = updateTemplate.Name;
		}
		if (string.IsNullOrWhiteSpace(updateTemplate.Description) is false)
		{
			task.Description = updateTemplate.Description;
		}
		if (updateTemplate.TypeId.HasValue)
		{
			task.TypeId = updateTemplate.TypeId;
		}
		if (updateTemplate.StateId.HasValue)
		{
			task.StateId = updateTemplate.StateId;
		}
		if (updateTemplate.DateOfStart.HasValue)
		{
			task.DateOfStart = updateTemplate.DateOfStart;
		}
		if (updateTemplate.DateOfEnd.HasValue)
		{
			task.DateOfEnd = updateTemplate.DateOfEnd;
		}
		if (updateTemplate.Deadline.HasValue)
		{
			task.Deadline = updateTemplate.Deadline;
		}
		return await UpdateTask(task: task);
	}
}