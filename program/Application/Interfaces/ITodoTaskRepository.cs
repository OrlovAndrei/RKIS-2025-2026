using Domain;

namespace Application.Interfaces;

public interface ITodoTaskRepository
{
	void Add(TodoTask todo);
	void Update(TodoTask todo);
	void Delete(Guid id);
	TodoTask? GetById(Guid id);
	IEnumerable<TodoTask> GetAll();
}