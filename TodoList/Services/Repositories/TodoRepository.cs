using Microsoft.EntityFrameworkCore;
using TodoList.Data;
using TodoList.Models;

namespace TodoList.Services.Repositories;

public class TodoRepository : ITodoRepository
{
    public async Task<IEnumerable<TodoItem>> GetAllByProfileAsync(Guid profileId)
    {
        await using var ctx = new AppDbContext();
        return await ctx.Todos
            .Where(t => t.ProfileId == profileId)
            .OrderBy(t => t.Id)
            .ToListAsync();
    }

    public async Task<TodoItem?> GetByIdAsync(int id)
    {
        await using var ctx = new AppDbContext();
        return await ctx.Todos.FindAsync(id);
    }

    public async Task AddAsync(TodoItem item)
    {
        await using var ctx = new AppDbContext();
        await ctx.Todos.AddAsync(item);
        await ctx.SaveChangesAsync();
    }

    public async Task UpdateAsync(TodoItem item)
    {
        await using var ctx = new AppDbContext();
        ctx.Todos.Update(item);
        await ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await using var ctx = new AppDbContext();
        var todo = await ctx.Todos.FindAsync(id);
        if (todo != null)
        {
            ctx.Todos.Remove(todo);
            await ctx.SaveChangesAsync();
        }
    }
}