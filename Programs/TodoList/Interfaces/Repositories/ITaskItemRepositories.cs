using TodoList.Entity;

namespace TodoList.Interfaces.Repositories;

public interface ITaskItemRepositories : IBaseCrudRepositories<TodoItem, uint>
{
    
}