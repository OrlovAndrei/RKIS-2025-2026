using Microsoft.EntityFrameworkCore;
using TodoApp.Models;

namespace TodoApp.Data
{
    public class TodoRepository
    {
        public async Task<List<TodoItem>> GetAllForProfileAsync(Guid profileId)
        {
            using var context = new AppDbContext();
            return await context.TodoItems
                .Where(t => t.ProfileId == profileId)
                .OrderBy(t => t.SortOrder)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<TodoItem?> GetByIdAsync(int id)
        {
            using var context = new AppDbContext();
            return await context.TodoItems.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddAsync(TodoItem item, Guid profileId)
        {
            using var context = new AppDbContext();
            int maxSortOrder = await context.TodoItems
                .Where(t => t.ProfileId == profileId)
                .MaxAsync(t => (int?)t.SortOrder) ?? 0;
            
            item.ProfileId = profileId;
            item.SortOrder = maxSortOrder + 1;
            item.LastUpdate = DateTime.Now;
            context.TodoItems.Add(item);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TodoItem item)
        {
            using var context = new AppDbContext();
            var existing = await context.TodoItems.FirstOrDefaultAsync(t => t.Id == item.Id);
            if (existing != null)
            {
                existing.Text = item.Text;
                existing.Status = item.Status;
                existing.LastUpdate = item.LastUpdate;
                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var context = new AppDbContext();
            var item = await context.TodoItems.FirstOrDefaultAsync(t => t.Id == id);
            if (item == null) return false;
            
            var profileId = item.ProfileId;
            context.TodoItems.Remove(item);
            await context.SaveChangesAsync();
            
            var remainingTodos = await context.TodoItems
                .Where(t => t.ProfileId == profileId)
                .OrderBy(t => t.SortOrder)
                .ToListAsync();
            
            for (int i = 0; i < remainingTodos.Count; i++)
            {
                remainingTodos[i].SortOrder = i + 1;
            }
            await context.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> SetStatusAsync(int id, TodoStatus status)
        {
            using var context = new AppDbContext();
            var item = await context.TodoItems.FirstOrDefaultAsync(t => t.Id == id);
            if (item == null) return false;
            
            item.Status = status;
            item.LastUpdate = DateTime.Now;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task ReplaceAllForProfileAsync(Guid profileId, IEnumerable<TodoItem> items)
        {
            using var context = new AppDbContext();
            var oldTodos = context.TodoItems.Where(t => t.ProfileId == profileId);
            context.TodoItems.RemoveRange(oldTodos);
            
            int order = 1;
            foreach (var todo in items)
            {
                todo.ProfileId = profileId;
                todo.SortOrder = order++;
                context.TodoItems.Add(todo);
            }
            await context.SaveChangesAsync();
        }
    }
}