using Domain;
using Application.Interfaces;
using Infrastructure.Database;
using Infrastructure.Mapper;

namespace Infrastructure.EfRepository;

public class EfTaskStateRepository : IStateRepository
{
	private readonly TodoContext _context;
	public EfTaskStateRepository(TodoContext context)
	{
		_context = context;
	}
	public void Add(TaskState status)
	{
		_context.StatesOfTask.Add(TaskStateMapper.ToEntity(status));
		_context.SaveChanges();
	}

	public void Delete(Guid id)
	{
		var state = _context.StatesOfTask.Find(id);
		if (state is not null)
		{
			_context.StatesOfTask.Remove(state);
			_context.SaveChanges();
		}
		else
		{
			throw new KeyNotFoundException($"TaskState with id {id} not found.");
		}
	}

	public IEnumerable<TaskState> GetAll()
	{
		return _context.StatesOfTask.Select(s => TaskStateMapper.ToDomain(s)).ToList();
	}

	public TaskState? GetById(Guid id)
	{
		return _context.StatesOfTask.Find(id) is var stateEntity && stateEntity is not null
			? TaskStateMapper.ToDomain(stateEntity)
			: null;
	}

	public void Update(TaskState status)
	{
		var existingState = _context.StatesOfTask.Find(status.StateId);
		if (existingState is not null)
		{
			existingState.Name = status.Name;
			existingState.Description = status.Description;
			existingState.IsCompleted = status.IsCompleted;
			_context.SaveChanges();
		}
		else
		{
			throw new KeyNotFoundException($"TaskState with id {status.StateId} not found.");
		}
	}
}