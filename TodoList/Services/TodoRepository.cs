using Microsoft.EntityFrameworkCore;
using TodoList.Data;
using TodoList.Models;

namespace TodoList.Services
{
    public class TodoRepository
    {
        public async Task<List<TodoItem>> GetAllForProfileAsync(Guid profileId)
        {
            using var context = new AppDbContext();
            return await context.TodoItems
                .Where(t => t.ProfileId == profileId)
                .OrderBy(t => t.Id)
                .ToListAsync();
        }

        public async Task<TodoItem?> GetByIdAsync(int id)
        {
            using var context = new AppDbContext();
            return await context.TodoItems.FindAsync(id);
        }

        public async Task AddAsync(TodoItem item)
        {
            using var context = new AppDbContext();
            context.TodoItems.Add(item);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TodoItem item)
        {
            using var context = new AppDbContext();
            context.TodoItems.Update(item);
            await context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var context = new AppDbContext();
            var item = await context.TodoItems.FindAsync(id);
            if (item == null) return false;
            context.TodoItems.Remove(item);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetStatusAsync(int id, TodoStatus status)
        {
            using var context = new AppDbContext();
            var item = await context.TodoItems.FindAsync(id);
            if (item == null) return false;
            item.SetStatus(status);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task ReplaceAllForProfileAsync(Guid profileId, IEnumerable<TodoItem> items)
        {
            using var context = new AppDbContext();
            var existing = context.TodoItems.Where(t => t.ProfileId == profileId);
            context.TodoItems.RemoveRange(existing);

            foreach (var item in items)
            {
                item.ProfileId = profileId;
                context.TodoItems.Add(item);
            }
            await context.SaveChangesAsync();
        }
    }
}