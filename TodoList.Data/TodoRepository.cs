using Microsoft.EntityFrameworkCore;
using TodoList.Data;
using TodoList.Models;

namespace TodoList.Data
{
    public class TodoRepository : ITodoRepository
    {
        public async Task<IEnumerable<TodoItem>> GetAllAsync(Guid profileId)
        {
            using var context = new AppDbContext();
            return await context.Todos
                .Where(t => t.ProfileId == profileId)
                .OrderBy(t => t.Id)
                .ToListAsync();
        }

        public async Task<TodoItem?> GetByIdAsync(int id, Guid profileId)
        {
            using var context = new AppDbContext();
            return await context.Todos
                .FirstOrDefaultAsync(t => t.Id == id && t.ProfileId == profileId);
        }

        public async Task<TodoItem> AddAsync(TodoItem item)
        {
            using var context = new AppDbContext();
            context.Todos.Add(item);
            await context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> UpdateAsync(TodoItem item)
        {
            using var context = new AppDbContext();
            var existing = await context.Todos
                .FirstOrDefaultAsync(t => t.Id == item.Id && t.ProfileId == item.ProfileId);
            
            if (existing == null) return false;

            existing.Text = item.Text;
            existing.Status = item.Status;
            existing.LastUpdate = item.LastUpdate;

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id, Guid profileId)
        {
            using var context = new AppDbContext();
            var item = await context.Todos
                .FirstOrDefaultAsync(t => t.Id == id && t.ProfileId == profileId);
            
            if (item == null) return false;

            context.Todos.Remove(item);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetStatusAsync(int id, TodoStatus status, Guid profileId)
        {
            using var context = new AppDbContext();
            var item = await context.Todos
                .FirstOrDefaultAsync(t => t.Id == id && t.ProfileId == profileId);
            
            if (item == null) return false;

            item.Status = status;
            item.LastUpdate = DateTime.Now;
            await context.SaveChangesAsync();
            return true;
        }
    }
}
