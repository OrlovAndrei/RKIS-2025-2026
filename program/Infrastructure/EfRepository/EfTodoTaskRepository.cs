using Domain.Entities.TaskEntity;
using Domain.Interfaces;
using Domain.Specifications;
using Infrastructure.Database;
using Infrastructure.Database.Entity;
using Infrastructure.EfRepository.Mapper;
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
			query = query.Where(t => taskCriteria.TaskId.IsSatisfiedBy(Guid.Parse(t.TaskId)));
		}
		if (taskCriteria.StateId is not null)
		{
			query = query.Where(t => taskCriteria.StateId.IsSatisfiedBy(t.StateId));
		}
		if (taskCriteria.PriorityLevel is not null)
		{
			query = query.Where(t => taskCriteria.PriorityLevel.IsSatisfiedBy(t.PriorityLevel));
		}
		if (taskCriteria.ProfileId is not null)
		{
			query = query.Where(t => taskCriteria.ProfileId.IsSatisfiedBy(Guid.Parse(t.ProfileId)));
		}
		if (taskCriteria.Name is not null)
		{
			query = query.Where(t => taskCriteria.Name.IsSatisfiedBy(t.Name));
		}
		if (taskCriteria.Description is not null)
		{
			query = query.Where(t => taskCriteria.Description.IsSatisfiedBy(t.Description));
		}
		if (taskCriteria.CreatedAt is not null)
		{
			query = query.Where(t => taskCriteria.CreatedAt.IsSatisfiedBy(t.CreateAt));
		}
		if (taskCriteria.Deadline is not null)
		{
			query = query.Where(t => taskCriteria.Deadline.IsSatisfiedBy(t.Deadline ?? default));
		}
		return query;
	}
	public async Task<IEnumerable<TodoTask>> FindAsync(TaskCriteria profileCriteria)
	{
		var query = _context.Tasks.AsQueryable();
		query = await ApplyCriteriaAsync(query, profileCriteria);
		return (await query.ToArrayAsync()).Select(t => t.ToDomain()).ToArray();
	}

	public async Task<TodoTask?> FindSingleAsync(TaskCriteria profileCriteria)
	{
		var query = _context.Tasks.AsQueryable();
		query = await ApplyCriteriaAsync(query, profileCriteria);
        var result = await query.FirstOrDefaultAsync();
        return result?.ToDomain();
    }

	public async Task<bool> ExistsAsync(TaskCriteria profileCriteria)
	{
		var query = _context.Tasks.AsQueryable();
		query = await ApplyCriteriaAsync(query, profileCriteria);
        return await query.AnyAsync();
    }

	public async Task<int> CountAsync(TaskCriteria profileCriteria)
	{
		var query = _context.Tasks.AsQueryable();
		query = await ApplyCriteriaAsync(query, profileCriteria);
		return await query.CountAsync();
	}
}