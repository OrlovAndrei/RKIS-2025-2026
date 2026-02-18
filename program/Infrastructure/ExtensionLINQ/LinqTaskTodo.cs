using Microsoft.EntityFrameworkCore;
using ShevricTodo.Database;

namespace System.Linq;

public static class LinqTaskTodo
{
	public enum Options
	{
		Name,
		Description,
		TaskId,
		TypeId,
		StateId,
		UserId,
		DateOfCreate,
		DateOfStart,
		DateOfEnd,
		Deadline
	}
	public static async Task<IQueryable<TaskTodo>> ContainsAsync(
	this Task<IQueryable<TaskTodo>> taskQuery,
	TaskTodo searchTemplate)
	{
		IQueryable<TaskTodo> query = await taskQuery;
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
		return query;
	}
	public static async Task<IQueryable<TaskTodo>> EndsWithAsync(
		this Task<IQueryable<TaskTodo>> taskQuery,
		TaskTodo searchTemplate)
	{
		IQueryable<TaskTodo> query = await taskQuery;
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
		return query;
	}
	public static async Task<IQueryable<TaskTodo>> StartsWithAsync(
		this Task<IQueryable<TaskTodo>> taskQuery,
		TaskTodo searchTemplate)
	{
		IQueryable<TaskTodo> query = await taskQuery;
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
		return query;
	}
	public static async Task<IQueryable<TaskTodo>> OrderByAsync(
		this Task<IQueryable<TaskTodo>> taskQuery,
		Options optionsOrders)
	{
		var query = await taskQuery;
		return optionsOrders switch
		{
			Options.Name => query.OrderBy(t => t.Name).AsQueryable(),
			Options.Description => query.OrderBy(t => t.Description).AsQueryable(),
			Options.TaskId => query.OrderBy(t => t.TaskId).AsQueryable(),
			Options.TypeId => query.OrderBy(t => t.TypeId).AsQueryable(),
			Options.StateId => query.OrderBy(t => t.StateId).AsQueryable(),
			Options.UserId => query.OrderBy(t => t.UserId).AsQueryable(),
			Options.DateOfCreate => query.OrderBy(t => t.DateOfCreate).AsQueryable(),
			Options.DateOfStart => query.OrderBy(t => t.DateOfStart).AsQueryable(),
			Options.DateOfEnd => query.OrderBy(t => t.DateOfEnd).AsQueryable(),
			Options.Deadline => query.OrderBy(t => t.Deadline).AsQueryable(),
			_ => query
		};
	}
	public static async Task<IQueryable<TaskTodo>> OrderByDescAsync(
		this Task<IQueryable<TaskTodo>> taskQuery,
		Options optionsOrders)
	{
		var query = await taskQuery;
		return optionsOrders switch
		{
			Options.Name => query.OrderByDescending(t => t.Name).AsQueryable(),
			Options.Description => query.OrderByDescending(t => t.Description).AsQueryable(),
			Options.TaskId => query.OrderByDescending(t => t.TaskId).AsQueryable(),
			Options.TypeId => query.OrderByDescending(t => t.TypeId).AsQueryable(),
			Options.StateId => query.OrderByDescending(t => t.StateId).AsQueryable(),
			Options.UserId => query.OrderByDescending(t => t.UserId).AsQueryable(),
			Options.DateOfCreate => query.OrderByDescending(t => t.DateOfCreate).AsQueryable(),
			Options.DateOfStart => query.OrderByDescending(t => t.DateOfStart).AsQueryable(),
			Options.DateOfEnd => query.OrderByDescending(t => t.DateOfEnd).AsQueryable(),
			Options.Deadline => query.OrderByDescending(t => t.Deadline).AsQueryable(),
			_ => query
		};
	}
	public static async Task<IQueryable<TaskTodo>> IdMinAndMaxAsync(
		this Task<IQueryable<TaskTodo>> taskQuery,
		int? min, int? max,
		Options option)
	{
		var query = await taskQuery;
		if (max is not null)
		{
			query = option switch
			{
				Options.TaskId => query.Where(t => t.TaskId <= max),
				Options.TypeId => query.Where(t => t.TypeId <= max),
				Options.StateId => query.Where(t => t.StateId <= max),
				Options.UserId => query.Where(t => t.UserId <= max),
				_ => query
			};
		}
		if (min is not null)
		{
			query = option switch
			{
				Options.TaskId => query.Where(t => t.TaskId >= min),
				Options.TypeId => query.Where(t => t.TypeId >= min),
				Options.StateId => query.Where(t => t.StateId >= min),
				Options.UserId => query.Where(t => t.UserId >= min),
				_ => query
			};
		}
		return query;
	}
	public static async Task<IQueryable<TaskTodo>> IdEqualsAsync(
		this Task<IQueryable<TaskTodo>> taskQuery,
		TaskTodo searchTemplate)
	{
		var query = await taskQuery;
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
		return query;
	}
	public static async Task<IQueryable<TaskTodo>> DateMinAndMaxAsync(
		this Task<IQueryable<TaskTodo>> taskQuery,
		DateTime? min, DateTime? max,
		Options option)
	{
		var query = await taskQuery;
		if (max is not null)
		{
			query = option switch
			{
				Options.DateOfCreate => query.Where(t => t.DateOfCreate <= max),
				Options.DateOfEnd => query.Where(t => t.DateOfEnd <= max),
				Options.DateOfStart => query.Where(t => t.DateOfStart <= max),
				Options.Deadline => query.Where(t => t.Deadline <= max),
				_ => query
			};
		}
		if (min is not null)
		{
			query = option switch
			{
				Options.DateOfCreate => query.Where(t => t.DateOfCreate >= max),
				Options.DateOfEnd => query.Where(t => t.DateOfEnd >= max),
				Options.DateOfStart => query.Where(t => t.DateOfStart >= max),
				Options.Deadline => query.Where(t => t.Deadline >= max),
				_ => query
			};
		}
		return query;
	}
	public static async Task<IQueryable<TaskTodo>> DateEqualsAsync(
		this Task<IQueryable<TaskTodo>> taskQuery,
		TaskTodo searchTemplate)
	{
		var query = await taskQuery;
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
	public static async Task<IQueryable<TaskTodo>> TopAsync(
		this Task<IQueryable<TaskTodo>> taskQuery,
		int top) => (await taskQuery).Take(top);
	public static async Task<IQueryable<TaskTodo>> ToAsync(
		this IQueryable<TaskTodo> taskQuery) => taskQuery;
	public static async Task<IQueryable<TaskTodo>> Start(
		this DbSet<TaskTodo> tasks) => await tasks.AsQueryable().ToAsync();
	public static async Task<IEnumerable<TaskTodo>> Finish(
		this Task<IQueryable<TaskTodo>> taskQuery) => await (await taskQuery).ToArrayAsync();
}
