using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TodoList.Database;
using TodoList.Entity;
using TodoList.Interfaces;
using TodoList.Interfaces.Repositories;

namespace TodoList.Infrastructure;

public class EfTodoTaskRepository(ApplicationContext context) : ITaskItemRepositories
{
	private readonly ApplicationContext _context = context;

	public async Task AddAsync(TodoItem todo)
	{
		_context.Tasks.Add(todo);
	}

	public async Task DeleteAsync(uint id)
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

	public async Task<IEnumerable<TodoItem>> GetAllAsync(ICurrentProfile userContext)
	{
		return _context.Tasks.AsParallel().Where(t => t.ProfileId == userContext.Id).ToArray();
	}
	public async Task<TodoItem?> GetByIdAsync(uint id)
	{
		return await _context.Tasks.FindAsync(id);
	}

	public async Task UpdateAsync(TodoItem todo)
	{
		var existingTask = _context.Tasks.First(t => t.Id == todo.Id);
		if (existingTask is not null)
		{
			existingTask.UpdateText(todo.Text);
			existingTask.UpdateStatus(todo.Status);
		}
		else
		{
			throw new KeyNotFoundException($"TodoTask with id {todo.Id} not found.");
		}
	}
	public async Task<IEnumerable<TodoItem>> FindAsync(Expression<Func<TodoItem, bool>> predicate)
	{
		var query = _context.Tasks.Where(predicate);
		return await query.ToArrayAsync();
	}

	public async Task<TodoItem?> FindSingleAsync(Expression<Func<TodoItem, bool>> predicate)
	{
		var query = _context.Tasks.Where(predicate);
		return await query.FirstOrDefaultAsync();
	}

	public async Task<bool> ExistsAsync(Expression<Func<TodoItem, bool>> predicate)
	{
		var query = _context.Tasks.Where(predicate);
		return await query.AnyAsync();
	}

	public async Task<int> CountAsync(Expression<Func<TodoItem, bool>> predicate)
	{
		var query = _context.Tasks.Where(predicate);
		return await query.CountAsync();
	}
}