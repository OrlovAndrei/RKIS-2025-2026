using Microsoft.EntityFrameworkCore;
using TodoApp.Models;

namespace TodoApp.Data
{
    public class TodoRepository : ITodoRepository
    {
        private readonly AppDbContext _context;

        public TodoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TodoItem>> GetAllAsync(Guid profileId)
        {
            return await _context.Todos
                .Where(t => t.ProfileId == profileId)
                .OrderByDescending(t => t.LastUpdate)
                .ToListAsync();
        }

        public async Task<TodoItem?> GetByIdAsync(int id)
        {
            return await _context.Todos.FindAsync(id);
        }

        public async Task AddAsync(TodoItem item)
        {
            _context.Todos.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TodoItem item)
        {
            _context.Todos.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.Todos.FindAsync(id);
            if (item != null)
            {
                _context.Todos.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SetStatusAsync(int id, TodoStatus status)
        {
            var item = await _context.Todos.FindAsync(id);
            if (item != null)
            {
                item.Status = status;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<TodoItem>> SearchAsync(Guid profileId, string? searchText, TodoStatus? statusFilter, DateTime? dueDateFilter)
        {
            var query = _context.Todos.Where(t => t.ProfileId == profileId);

            if (!string.IsNullOrWhiteSpace(searchText))
                query = query.Where(t => t.Text.Contains(searchText));

            if (statusFilter.HasValue)
                query = query.Where(t => t.Status == statusFilter.Value);

            if (dueDateFilter.HasValue)
                query = query.Where(t => t.LastUpdate.Date == dueDateFilter.Value.Date);

            return await query.OrderByDescending(t => t.LastUpdate).ToListAsync();
        }
    }
}