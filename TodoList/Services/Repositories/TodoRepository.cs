using Microsoft.EntityFrameworkCore;

public class TodoRepository : ITodoRepository
{
    public async Task<List<TodoItem>> GetAllByProfileAsync(Guid profileId)
    {
        using var ctx = new AppDbContext();
        return await ctx.Todos
            .AsNoTracking()
            .Where(t => t.ProfileId == profileId)
            .OrderBy(t => t.Id)
            .ToListAsync();
    }

    public async Task AddAsync(TodoItem item)
    {
        using var ctx = new AppDbContext();
        await ctx.Todos.AddAsync(item);
        await ctx.SaveChangesAsync();
    }

    public async Task UpdateAsync(TodoItem item)
    {
        using var ctx = new AppDbContext();
        ctx.Todos.Update(item);
        await ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        using var ctx = new AppDbContext();
        var todo = await ctx.Todos.FindAsync(id);
        if (todo != null)
        {
            ctx.Todos.Remove(todo);
            await ctx.SaveChangesAsync();
        }
    }

    public async Task SetStatusAsync(int id, TodoStatus status)
    {
        using var ctx = new AppDbContext();
        var todo = await ctx.Todos.FindAsync(id);
        if (todo == null)
            return;

        todo.SetStatus(status);
        await ctx.SaveChangesAsync();
    }
}
