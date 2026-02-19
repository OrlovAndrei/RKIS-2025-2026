using Domain;

namespace Application.Interfaces;

public interface IStateRepository
{
	void Add(TaskState status);
	void Update(TaskState status);
	void Delete(Guid id);
	TaskState? GetById(Guid id);
	IEnumerable<TaskState> GetAll();
}