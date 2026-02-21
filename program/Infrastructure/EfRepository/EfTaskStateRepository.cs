using Application.Interfaces;
using Domain;
using Infrastructure.Database;
using Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EfRepository;

public class EfTaskStateRepository(TodoContext context) : IStateRepository
{
	private readonly TodoContext _context = context;

	public async Task<int> AddAsync(TaskState status)
	{
		_context.StatesOfTask.Add(TaskStateMapper.ToEntity(status));
		return await _context.SaveChangesAsync();
	}

	public async Task<int> DeleteAsync(Guid id)
	{
		var state = _context.StatesOfTask.Find(id);
		if (state is not null)
		{
			_context.StatesOfTask.Remove(state);
			return await _context.SaveChangesAsync();
		}
		else
		{
			throw new KeyNotFoundException($"TaskState with id {id} not found.");
		}
	}

	public async Task<IEnumerable<TaskState>> GetAllAsync()
	{
		return await _context.StatesOfTask.Select(s => TaskStateMapper.ToDomain(s)).ToListAsync();
	}

	public async Task<TaskState?> GetByIdAsync(Guid id)
	{
		return await _context.StatesOfTask.FindAsync(id) is var stateEntity && stateEntity is not null
			? TaskStateMapper.ToDomain(stateEntity)
			: null;
	}

	public async Task<int> UpdateAsync(TaskState status)
	{
		var existingState = _context.StatesOfTask.Find(status.StateId);
		if (existingState is not null)
		{
			existingState.Name = status.Name;
			existingState.Description = status.Description;
			existingState.IsCompleted = status.IsCompleted;
			return await _context.SaveChangesAsync();
		}
		else
		{
			throw new KeyNotFoundException($"TaskState with id {status.StateId} not found.");
		}
	}
}