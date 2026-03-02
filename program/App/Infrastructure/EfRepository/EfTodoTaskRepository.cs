using Application.Interfaces;
using Application.Interfaces.Repository;
using Application.Specifications;
using Domain.Entities.TaskEntity;
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
		var task = _context.Tasks.Find(id.ToString());
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

	public async Task<IEnumerable<TodoTask>> GetAllAsync(IUserContext userContext)
	{
		return _context.Tasks.AsParallel().Where(t => t.ProfileId == userContext.UserId.ToString()).ToList().ConvertAll(t => t.ToDomain());
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
			query = query.Where(t => t.TaskId == taskCriteria.TaskId.Value.ToString());
		}
		if (taskCriteria.StateId is not null)
		{
			query = query.Where(t => t.StateId == taskCriteria.StateId.Value);
		}
		if (taskCriteria.PriorityLevel is not null)
		{
			query = query.Where(t => t.PriorityLevel >= taskCriteria.PriorityLevel.Value.From &&
			t.PriorityLevel >= taskCriteria.PriorityLevel.Value.To);
		}
		if (taskCriteria.ProfileId is not null)
		{
			query = query.Where(t => t.ProfileId == taskCriteria.ProfileId.Value.ToString());
		}
		if (taskCriteria.Name is not null)
		{
			query = query.Where(t => t.Name.Contains(taskCriteria.Name.Value));
		}
		if (taskCriteria.Description is not null)
		{
			query = query.Where(t => t.Description != null && t.Description.Contains(taskCriteria.Description.Value));
		}
		if (taskCriteria.CreatedAt is not null)
		{
			query = query.Where(t => t.CreateAt >= taskCriteria.CreatedAt.Value.From &&
				t.CreateAt <= taskCriteria.CreatedAt.Value.To);
		}
		if (taskCriteria.Deadline is not null)
		{
			query = query.Where(t => t.Deadline >= taskCriteria.Deadline.Value.From &&
				t.Deadline <= taskCriteria.Deadline.Value.To);
		}
		return query;
	}
	public async Task<IEnumerable<TodoTask>> FindAsync(TaskCriteria taskCriteria)
	{
		var query = _context.Tasks.AsQueryable();
		query = await ApplyCriteriaAsync(query, taskCriteria);
		return (await query.ToArrayAsync()).Select(t => t.ToDomain()).ToArray();
	}

	public async Task<TodoTask?> FindSingleAsync(TaskCriteria taskCriteria)
	{
		var query = _context.Tasks.AsQueryable();
		query = await ApplyCriteriaAsync(query, taskCriteria);
		var result = await query.FirstOrDefaultAsync();
		return result?.ToDomain();
	}

	public async Task<bool> ExistsAsync(TaskCriteria taskCriteria)
	{
		var query = _context.Tasks.AsQueryable();
		query = await ApplyCriteriaAsync(query, taskCriteria);
		return await query.AnyAsync();
	}

	public async Task<int> CountAsync(TaskCriteria taskCriteria)
	{
		var query = _context.Tasks.AsQueryable();
		query = await ApplyCriteriaAsync(query, taskCriteria);
		return await query.CountAsync();
	}
}