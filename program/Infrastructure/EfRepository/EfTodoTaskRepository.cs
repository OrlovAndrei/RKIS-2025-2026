using Domain;
using Application.Interfaces;
using Infrastructure.Database;
using Infrastructure.Mapper;

namespace Infrastructure.EfRepository;

public class EfTodoTaskRepository : ITodoTaskRepository
{
	private readonly TodoContext _context;
	public EfTodoTaskRepository(TodoContext context)
	{
		_context = context;
	}
	public void Add(TodoTask todo)
	{
		_context.Tasks.Add(TodoTaskMapper.ToEntity(todo));
		_context.SaveChanges();
	}

	public void Delete(Guid id)
	{
		var task = _context.Tasks.Find(id);
		if (task is not null)
		{
			_context.Tasks.Remove(task);
			_context.SaveChanges();
		}
		else
		{
			throw new KeyNotFoundException($"TodoTask with id {id} not found.");
		}
	}

	public IEnumerable<TodoTask> GetAll()
	{
		return _context.Tasks.Select(t => TodoTaskMapper.ToDomain(t)).ToList();
	}

	public TodoTask? GetById(Guid id)
	{
		return _context.Tasks.Find(id) is var taskEntity && taskEntity is not null
			? TodoTaskMapper.ToDomain(taskEntity)
			: null;
	}

	public void Update(TodoTask todo)
	{
		var existingTask = _context.Tasks.Find(todo.TaskId);
		if (existingTask is not null)
		{
			existingTask.Name = todo.Name;
			existingTask.Description = todo.Description;
			existingTask.Deadline = todo.Deadline;
			existingTask.StateId = todo.StateId.ToString();
			_context.SaveChanges();
		}
		else
		{
			throw new KeyNotFoundException($"TodoTask with id {todo.TaskId} not found.");
		}
	}
}