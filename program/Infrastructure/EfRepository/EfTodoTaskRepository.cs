using Application.Interfaces;
using Domain;
using Infrastructure.Database;
using Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EfRepository;

public class EfTodoTaskRepository(TodoContext context) : ITodoTaskRepository
{
	private readonly TodoContext _context = context;

	public async Task<int> AddAsync(TodoTask todo)
	{
		_context.Tasks.Add(TodoTaskMapper.ToEntity(todo));
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
		return await _context.Tasks.Select(t => TodoTaskMapper.ToDomain(t)).ToListAsync();
	}

	public async Task<TodoTask?> GetByIdAsync(Guid id)
	{
		return await _context.Tasks.FindAsync(id) is var taskEntity && taskEntity is not null
			? TodoTaskMapper.ToDomain(taskEntity)
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
			existingTask.StateId = todo.StateId.ToString();
			return await _context.SaveChangesAsync();
		}
		else
		{
			throw new KeyNotFoundException($"TodoTask with id {todo.TaskId} not found.");
		}
	}
}