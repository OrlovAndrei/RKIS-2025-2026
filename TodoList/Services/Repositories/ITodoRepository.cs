public interface ITodoRepository
{
    Task<List<TodoItem>> GetAllByProfileAsync(Guid profileId);
    Task AddAsync(TodoItem item);
    Task UpdateAsync(TodoItem item);
    Task DeleteAsync(int id);
    Task SetStatusAsync(int id, TodoStatus status);
}