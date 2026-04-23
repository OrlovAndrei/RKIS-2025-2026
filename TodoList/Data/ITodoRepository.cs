using TodoApp.Models;

namespace TodoApp.Data
{
    public interface ITodoRepository
    {
        Task<List<TodoItem>> GetAllAsync(Guid profileId);
        Task<TodoItem?> GetByIdAsync(int id);
        Task AddAsync(TodoItem item);
        Task UpdateAsync(TodoItem item);
        Task DeleteAsync(int id);
        Task SetStatusAsync(int id, TodoStatus status);
        Task<List<TodoItem>> SearchAsync(Guid profileId, string? searchText, TodoStatus? statusFilter, DateTime? dueDateFilter);
    }
}