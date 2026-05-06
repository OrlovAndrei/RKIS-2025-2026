using TodoList.Models;

namespace TodoList.Data
{
    public interface ITodoRepository
    {
        Task<IEnumerable<TodoItem>> GetAllAsync(Guid profileId);
        Task<TodoItem?> GetByIdAsync(int id, Guid profileId);
        Task<TodoItem> AddAsync(TodoItem item);
        Task<bool> UpdateAsync(TodoItem item);
        Task<bool> DeleteAsync(int id, Guid profileId);
        Task<bool> SetStatusAsync(int id, TodoStatus status, Guid profileId);
    }
}
