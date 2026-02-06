using Microsoft.EntityFrameworkCore;
using ShevricTodo.Authentication;
using ShevricTodo.Database;

namespace ShevricTodo.Commands.Task;

internal class Search : Task
{
	public static async Task<IEnumerable<TaskTodo>> SearchTasks(
		TaskTodo searchTemplate)
	{
		using (Todo db = new())
		{
			IQueryable<TaskTodo> query = db.Tasks.AsQueryable();
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
			return await query.ToListAsync();
		}
	}
	public static async System.Threading.Tasks.Task SearchAndPrintTasksOfActiveUser(
		Func<TaskTodo, Task<IEnumerable<TaskTodo>>> searchProfile,
		Action<string> showMessage,
		Action<TaskTodo> showProfile,
		Action<IEnumerable<TaskTodo>> showProfiles,
		TaskTodo searchTemplate)
	{
		searchTemplate.UserId = (await ActiveProfile.GetActiveProfile()).UserId;
		await SearchAndPrintTasks(
			searchProfile, showMessage, showProfile, showProfiles, searchTemplate);
	}
	public static async System.Threading.Tasks.Task SearchAndPrintTasks(
		Func<TaskTodo, Task<IEnumerable<TaskTodo>>> searchProfile,
		Action<string> showMessage,
		Action<TaskTodo> showProfile,
		Action<IEnumerable<TaskTodo>> showProfiles,
		TaskTodo searchTemplate)
	{
		IEnumerable<TaskTodo> tasks = await searchProfile(searchTemplate);
		switch (tasks.Count())
		{
			case 0:
				showMessage("Нет ни одной похожей задачи.");
				break;
			case 1:
				showProfile(tasks.First());
				break;
			default:
				showProfiles(tasks);
				break;
		}
	}
}
