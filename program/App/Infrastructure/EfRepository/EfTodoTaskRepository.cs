using System.Linq.Expressions;
using Application.Interfaces;
using Application.Interfaces.Repository;
using Domain.Entities.TaskEntity;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EfRepository;

public class EfTodoTaskRepository(TodoContext context) : ITodoTaskRepository
{
	private readonly TodoContext _context = context;

	public async Task AddAsync(TodoTask todo)
	{
		_context.Tasks.Add(todo);
	}

	public async Task DeleteAsync(Guid id)
	{
		var task = _context.Tasks.Find(id);
		if (task is not null)
		{
			_context.Tasks.Remove(task);
		}
		else
		{
			throw new KeyNotFoundException($"TodoTask with id {id} not found.");
		}
	}

	public async Task<IEnumerable<TodoTask>> GetAllAsync(IUserContext userContext)
	{
		return _context.Tasks.AsParallel().Where(t => t.ProfileId == userContext.UserId).ToArray();
	}
	public async Task<TodoTask?> GetByIdAsync(Guid id)
	{
		return await _context.Tasks.FindAsync(id);
	}

	public async Task UpdateAsync(TodoTask todo)
	{
		var existingTask = _context.Tasks.First(t => t.TaskId == todo.TaskId);
		if (existingTask is not null)
		{
			existingTask.UpdateName(todo.Name);
			existingTask.UpdateDescription(todo.Description);
			existingTask.UpdateDeadline(todo.Deadline);
			existingTask.UpdateState(todo.State);
			existingTask.UpdatePriority(todo.Priority);
		}
		else
		{
			throw new KeyNotFoundException($"TodoTask with id {todo.TaskId} not found.");
		}
	}
	public async Task<IEnumerable<TodoTask>> FindAsync(Expression<Func<TodoTask, bool>> predicate)
	{
		var query = _context.Tasks.Where(predicate);
		return await query.ToArrayAsync();
	}

	public async Task<TodoTask?> FindSingleAsync(Expression<Func<TodoTask, bool>> predicate)
	{
		var query = _context.Tasks.Where(predicate);
		return await query.FirstOrDefaultAsync();
	}

	public async Task<bool> ExistsAsync(Expression<Func<TodoTask, bool>> predicate)
	{
		var query = _context.Tasks.Where(predicate);
		return await query.AnyAsync();
	}

	public async Task<int> CountAsync(Expression<Func<TodoTask, bool>> predicate)
	{
		var query = _context.Tasks.Where(predicate);
		return await query.CountAsync();
	}
}