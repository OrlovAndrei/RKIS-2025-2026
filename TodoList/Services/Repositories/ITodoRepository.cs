using TodoList.Models;

namespace TodoList.Services.Repositories;

public interface ITodoRepository
{
    Task<IEnumerable<TodoItem>> GetAllByProfileAsync(Guid profileId);
    Task<TodoItem?> GetByIdAsync(int id);
    Task AddAsync(TodoItem item);
    Task UpdateAsync(TodoItem item);
    Task DeleteAsync(int id);
}