using Microsoft.EntityFrameworkCore;
using TodoApp.Models;

namespace TodoApp.Data;

public class TodoRepository
{
    public List<TodoItem> GetAllForUser(Guid profileId)
    {
        using var context = new AppDbContext();
        return context.Todos.Where(t => t.ProfileId == profileId).ToList();
    }

    public void Add(TodoItem item)
    {
        using var context = new AppDbContext();
        context.Todos.Add(item);
        context.SaveChanges();
    }

    public void Update(TodoItem item)
    {
        using var context = new AppDbContext();
        context.Todos.Update(item);
        context.SaveChanges();
    }

    public void Delete(int id)
    {
        using var context = new AppDbContext();
        var item = context.Todos.Find(id);
        if (item != null)
        {
            context.Todos.Remove(item);
            context.SaveChanges();
        }
    }

    public void SetStatus(int id, TodoStatus status)
    {
        using var context = new AppDbContext();
        var item = context.Todos.Find(id);
        if (item != null)
        {
            item.SetStatus(status);
            context.SaveChanges();
        }
    }

    public TodoItem? GetById(int id)
    {
        using var context = new AppDbContext();
        return context.Todos.Find(id);
    }
}