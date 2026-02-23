using Application.Interfaces.Repository;
using Application.Specifications;
using Domain.Entities.TaskEntity;
using Infrastructure.Database;
using Infrastructure.Database.Entity;
using Infrastructure.EfRepository.Mapper;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EfRepository;

public class EfTodoTaskRepository(TodoContext context) : ITodoTaskRepository
{
	private readonly TodoContext _context = context;

	public async Task<int> AddAsync(TodoTask todo)
	{
		_context.Tasks.Add(todo.ToEntity());
		return await _context.SaveChangesAsync();
	}

	public async Task<int> DeleteAsync(Guid id)
	{
		var task = _context.Tasks.Find(id);
		if (task is not null)
		{
			_context.Tasks.Remove(task);
			return await _context.SaveChangesAsync();
		}
		else
		{
			throw new KeyNotFoundException($"TodoTask with id {id} not found.");
		}
	}

	public async Task<IEnumerable<TodoTask>> GetAllAsync()
	{
		return await _context.Tasks.Select(t => t.ToDomain()).ToListAsync();
	}
	public async Task<TodoTask?> GetByIdAsync(Guid id)
	{
		return await _context.Tasks.FindAsync(id) is var taskEntity && taskEntity is not null
			? taskEntity.ToDomain()
			: null;
	}

	public async Task<int> UpdateAsync(TodoTask todo)
	{
		var existingTask = _context.Tasks.Find(todo.TaskId);
		if (existingTask is not null)
		{
			existingTask.Name = todo.Name;
			existingTask.Description = todo.Description;
			existingTask.Deadline = todo.Deadline;
			existingTask.StateId = todo.State.StateId;
			existingTask.PriorityLevel = todo.Priority.Level;
			return await _context.SaveChangesAsync();
		}
		else
		{
			throw new KeyNotFoundException($"TodoTask with id {todo.TaskId} not found.");
		}
	}
	private async Task<IQueryable<TodoTaskEntity>> ApplyCriteriaAsync(
		IQueryable<TodoTaskEntity> query,
		TaskCriteria taskCriteria)
	{
		if (taskCriteria.TaskId is not null)
		{
			var tidExpr = taskCriteria.TaskId.IsSatisfiedBy<TodoTaskEntity>(t => Guid.Parse(t.TaskId));
			query = query.Where(tidExpr);
		}
		if (taskCriteria.StateId is not null)
		{
			var sidExpr = taskCriteria.StateId.IsSatisfiedBy<TodoTaskEntity>(t => t.StateId);
			query = query.Where(sidExpr);
		}
		if (taskCriteria.PriorityLevel is not null)
		{
			var plExpr = taskCriteria.PriorityLevel.IsSatisfiedBy<TodoTaskEntity>(t => t.PriorityLevel);
			query = query.Where(plExpr);
		}
		if (taskCriteria.ProfileId is not null)
		{
			var pidExpr = taskCriteria.ProfileId.IsSatisfiedBy<TodoTaskEntity>(t => Guid.Parse(t.ProfileId));
			query = query.Where(pidExpr);
		}
		if (taskCriteria.Name is not null)
		{
			var nameExpr = taskCriteria.Name.IsSatisfiedBy<TodoTaskEntity>(t => t.Name);
			query = query.Where(nameExpr);
		}
		if (taskCriteria.Description is not null)
		{
			var descExpr = taskCriteria.Description.IsSatisfiedBy<TodoTaskEntity>(t => t.Description ?? string.Empty);
			query = query.Where(descExpr);
		}
		if (taskCriteria.CreatedAt is not null)
		{
			var caExpr = taskCriteria.CreatedAt.IsSatisfiedBy<TodoTaskEntity>(t => t.CreateAt);
			query = query.Where(caExpr);
		}
		if (taskCriteria.Deadline is not null)
		{
			var dlExpr = taskCriteria.Deadline.IsSatisfiedBy<TodoTaskEntity>(t => t.Deadline ?? default);
			query = query.Where(dlExpr);
		}
		return query;
	}
	public async Task<IEnumerable<TodoTask>> FindAsync(TaskCriteria profileCriteria)
	{
		var query = _context.Tasks.AsExpandable();
		query = await ApplyCriteriaAsync(query, profileCriteria);
		return (await query.ToArrayAsync()).Select(t => t.ToDomain()).ToArray();
	}

	public async Task<TodoTask?> FindSingleAsync(TaskCriteria profileCriteria)
	{
		var query = _context.Tasks.AsExpandable();
		query = await ApplyCriteriaAsync(query, profileCriteria);
        var result = await query.FirstOrDefaultAsync();
        return result?.ToDomain();
    }

	public async Task<bool> ExistsAsync(TaskCriteria profileCriteria)
	{
		var query = _context.Tasks.AsExpandable();
		query = await ApplyCriteriaAsync(query, profileCriteria);
        return await query.AnyAsync();
    }

	public async Task<int> CountAsync(TaskCriteria profileCriteria)
	{
		var query = _context.Tasks.AsExpandable();
		query = await ApplyCriteriaAsync(query, profileCriteria);
		return await query.CountAsync();
	}
}