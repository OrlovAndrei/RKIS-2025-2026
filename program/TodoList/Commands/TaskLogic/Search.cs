using Microsoft.EntityFrameworkCore;
using ShevricTodo.Authentication;
using ShevricTodo.Database;

namespace ShevricTodo.Commands.TaskObj;

internal partial class Search : TaskObj
{
	private static async Task<IQueryable<TaskTodo>> FilterIdAndDate(
		IQueryable<TaskTodo> query,
		TaskTodo searchTemplate)
	{
		if (searchTemplate.TaskId.HasValue)
		{
			query = query
				.Where(t => t.TaskId == searchTemplate.TaskId);
		}
		if (searchTemplate.TypeId.HasValue)
		{
			query = query
				.Where(t => t.TypeId == searchTemplate.TypeId);
		}
		if (searchTemplate.StateId.HasValue)
		{
			query = query
				.Where(t => t.StateId == searchTemplate.StateId);
		}
		if (searchTemplate.UserId.HasValue)
		{
			query = query
				.Where(t => t.UserId == searchTemplate.UserId);
		}
		if (searchTemplate.DateOfCreate.HasValue)
		{
			query = query
				.Where(t => t.DateOfCreate == searchTemplate.DateOfCreate);
		}
		if (searchTemplate.DateOfStart.HasValue)
		{
			query = query
				.Where(t => t.DateOfStart == searchTemplate.DateOfStart);
		}
		if (searchTemplate.DateOfEnd.HasValue)
		{
			query = query
				.Where(t => t.DateOfEnd == searchTemplate.DateOfEnd);
		}
		if (searchTemplate.Deadline.HasValue)
		{
			query = query
				.Where(t => t.Deadline == searchTemplate.Deadline);
		}
		return query;
	}
	protected internal static async Task<IEnumerable<TaskTodo>> SearchTasksContains(
		TaskTodo searchTemplate)
	{
		using (Todo db = new())
		{
			IQueryable<TaskTodo> query = db.Tasks.AsQueryable();
			query = await FilterIdAndDate(query, searchTemplate);
			if (!string.IsNullOrEmpty(searchTemplate.Name))
			{
				query = query
					.Where(t => t.Name != null 
					&& t.Name.Contains(searchTemplate.Name));
			}
			if (!string.IsNullOrEmpty(searchTemplate.Description))
			{
				query = query
					.Where(t => t.Description != null 
					&& t.Description.Contains(searchTemplate.Description));
			}
			return await query.ToListAsync();
		}
	}
	protected internal static async Task<IEnumerable<TaskTodo>> SearchTasksStartsWith(
		TaskTodo searchTemplate)
	{
		using (Todo db = new())
		{
			IQueryable<TaskTodo> query = db.Tasks.AsQueryable();
			query = await FilterIdAndDate(query, searchTemplate);
			if (!string.IsNullOrEmpty(searchTemplate.Name))
			{
				query = query
					.Where(t => t.Name != null
					&& t.Name.StartsWith(searchTemplate.Name));
			}
			if (!string.IsNullOrEmpty(searchTemplate.Description))
			{
				query = query
					.Where(t => t.Description != null
					&& t.Description.StartsWith(searchTemplate.Description));
			}
			return await query.ToListAsync();
		}
	}
	protected internal static async Task<IEnumerable<TaskTodo>> SearchTasksEndsWith(
		TaskTodo searchTemplate)
	{
		using (Todo db = new())
		{
			IQueryable<TaskTodo> query = db.Tasks.AsQueryable();
			query = await FilterIdAndDate(query, searchTemplate);
			if (!string.IsNullOrEmpty(searchTemplate.Name))
			{
				query = query
					.Where(t => t.Name != null
					&& t.Name.EndsWith(searchTemplate.Name));
			}
			if (!string.IsNullOrEmpty(searchTemplate.Description))
			{
				query = query
					.Where(t => t.Description != null
					&& t.Description.EndsWith(searchTemplate.Description));
			}
			return await query.ToListAsync();
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
	protected internal static async Task<TaskTodo> Clarification(
		Func<TaskTodo, Task<IEnumerable<TaskTodo>>> searchTaskTodo,
		Func<Dictionary<int, string>,
			string?,
			int,
			KeyValuePair<int, string>> inputOneOf,
		TaskTodo searchTemplate,
		IEnumerable<TaskTodo> tasksTodo)
	{
		KeyValuePair<int, string> taskIdAndName = inputOneOf((Dictionary<int, string>)
					(from task in tasksTodo
					 select new { task.TaskId, task.Name }),
					 "Какую задачу вы хотите удалить?", 5);
		searchTemplate.TaskId = taskIdAndName.Key;
		searchTemplate.Name = taskIdAndName.Value;
		return (await searchTaskTodo(searchTemplate)).First();
	}
}
