using Microsoft.EntityFrameworkCore;
using ShevricTodo.Database;

namespace Infrastructure.ExtensionLINQ;

public static class LinqTaskTodo
{
	public enum Options
	{
		UserName,
		LastName,
		FirstName,
		UserId,
		DateOfCreate,
		Birthday
	}
	public static async Task<IQueryable<Profile>> ContainsAsync(
	this Task<IQueryable<Profile>> profileQuery,
	Profile searchTemplate)
	{
		IQueryable<Profile> query = await profileQuery;
		if (!string.IsNullOrEmpty(searchTemplate.FirstName))
		{
			query = query
				.Where(t => t.FirstName != null
				&& t.FirstName.Contains(searchTemplate.FirstName));
		}
		if (!string.IsNullOrEmpty(searchTemplate.LastName))
		{
			query = query
				.Where(t => t.LastName != null
				&& t.LastName.Contains(searchTemplate.LastName));
		}
		if (!string.IsNullOrEmpty(searchTemplate.UserName))
		{
			query = query
				.Where(t => t.UserName != null
				&& t.UserName.Contains(searchTemplate.UserName));
		}
		return query;
	}
	public static async Task<IQueryable<Profile>> EndsWithAsync(
	this Task<IQueryable<Profile>> profileQuery,
	Profile searchTemplate)
	{
		IQueryable<Profile> query = await profileQuery;
		if (!string.IsNullOrEmpty(searchTemplate.FirstName))
		{
			query = query
				.Where(t => t.FirstName != null
				&& t.FirstName.EndsWith(searchTemplate.FirstName));
		}
		if (!string.IsNullOrEmpty(searchTemplate.LastName))
		{
			query = query
				.Where(t => t.LastName != null
				&& t.LastName.EndsWith(searchTemplate.LastName));
		}
		if (!string.IsNullOrEmpty(searchTemplate.UserName))
		{
			query = query
				.Where(t => t.UserName != null
				&& t.UserName.EndsWith(searchTemplate.UserName));
		}
		return query;
	}
	public static async Task<IQueryable<Profile>> StartsWithAsync(
	this Task<IQueryable<Profile>> profileQuery,
	Profile searchTemplate)
	{
		IQueryable<Profile> query = await profileQuery;
		if (!string.IsNullOrEmpty(searchTemplate.FirstName))
		{
			query = query
				.Where(t => t.FirstName != null
				&& t.FirstName.StartsWith(searchTemplate.FirstName));
		}
		if (!string.IsNullOrEmpty(searchTemplate.LastName))
		{
			query = query
				.Where(t => t.LastName != null
				&& t.LastName.StartsWith(searchTemplate.LastName));
		}
		if (!string.IsNullOrEmpty(searchTemplate.UserName))
		{
			query = query
				.Where(t => t.UserName != null
				&& t.UserName.StartsWith(searchTemplate.UserName));
		}
		return query;
	}
	public static async Task<IQueryable<Profile>> OrderByAsync(
		this Task<IQueryable<Profile>> profileQuery,
		Options optionsOrders)
	{
		var query = await profileQuery;
		return optionsOrders switch
		{
			Options.FirstName => query.OrderBy(t => t.FirstName).AsQueryable(),
			Options.LastName => query.OrderBy(t => t.LastName).AsQueryable(),
			Options.UserName => query.OrderBy(t => t.UserName).AsQueryable(),
			Options.UserId => query.OrderBy(t => t.UserId).AsQueryable(),
			Options.DateOfCreate => query.OrderBy(t => t.DateOfCreate).AsQueryable(),
			Options.Birthday => query.OrderBy(t => t.Birthday).AsQueryable(),
			_ => query
		};
	}
	public static async Task<IQueryable<Profile>> OrderByDescAsync(
		this Task<IQueryable<Profile>> profileQuery,
		Options optionsOrders)
	{
		var query = await profileQuery;
		return optionsOrders switch
		{
			Options.FirstName => query.OrderByDescending(t => t.FirstName).AsQueryable(),
			Options.LastName => query.OrderByDescending(t => t.LastName).AsQueryable(),
			Options.UserName => query.OrderByDescending(t => t.UserName).AsQueryable(),
			Options.UserId => query.OrderByDescending(t => t.UserId).AsQueryable(),
			Options.DateOfCreate => query.OrderByDescending(t => t.DateOfCreate).AsQueryable(),
			Options.Birthday => query.OrderByDescending(t => t.Birthday).AsQueryable(),
			_ => query
		};
	}
	public static async Task<IQueryable<Profile>> IdMinAndMaxAsync(
		this Task<IQueryable<Profile>> profileQuery,
		int? min, int? max,
		Options option)
	{
		var query = await profileQuery;
		if (max is not null)
		{
			query = option switch
			{
				Options.UserId => query.Where(t => t.UserId <= max),
				_ => query
			};
		}
		if (min is not null)
		{
			query = option switch
			{
				Options.UserId => query.Where(t => t.UserId >= min),
				_ => query
			};
		}
		return query;
	}
	public static async Task<IQueryable<Profile>> IdEqualsAsync(
		this Task<IQueryable<Profile>> profileQuery,
		Profile searchTemplate)
	{
		var query = await profileQuery;
		if (searchTemplate.UserId.HasValue)
		{
			query = query
				.Where(t => t.UserId == searchTemplate.UserId);
		}
		return query;
	}
	public static async Task<IQueryable<Profile>> DateMinAndMaxAsync(
		this Task<IQueryable<Profile>> profileQuery,
		DateTime? min, DateTime? max,
		Options option)
	{
		var query = await profileQuery;
		if (max is not null)
		{
			query = option switch
			{
				Options.DateOfCreate => query.Where(t => t.DateOfCreate <= max),
				Options.Birthday => query.Where(t => t.Birthday <= max),
				_ => query
			};
		}
		if (min is not null)
		{
			query = option switch
			{
				Options.DateOfCreate => query.Where(t => t.DateOfCreate >= max),
				Options.Birthday => query.Where(t => t.Birthday >= max),
				_ => query
			};
		}
		return query;
	}
	public static async Task<IQueryable<Profile>> DateEqualsAsync(
		this Task<IQueryable<Profile>> profileQuery,
		Profile searchTemplate)
	{
		var query = await profileQuery;
		if (searchTemplate.DateOfCreate.HasValue)
		{
			query = query
				.Where(t => t.DateOfCreate == searchTemplate.DateOfCreate);
		}
		if (searchTemplate.Birthday.HasValue)
		{
			query = query
				.Where(t => t.Birthday == searchTemplate.Birthday);
		}
		return query;
	}
	public static async Task<IQueryable<Profile>> TopAsync(
		this Task<IQueryable<Profile>> profileQuery,
		int top) => (await profileQuery).Take(top);
	public static async Task<IQueryable<Profile>> AsTask(
		this IQueryable<Profile> profileQuery) => profileQuery;
	public static async Task<IQueryable<Profile>> Start(
		this DbSet<Profile> tasks) => await tasks.AsQueryable().AsTask();
	public static async Task<IEnumerable<Profile>> Finish(
		this Task<IQueryable<Profile>> profileQuery) => await (await profileQuery).ToArrayAsync();
}
