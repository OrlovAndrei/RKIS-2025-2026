using Microsoft.EntityFrameworkCore;
using TodoApp.Models;

namespace TodoApp.Data;

public sealed class TodoRepository
{
    private readonly Func<AppDbContext> _contextFactory;

    public TodoRepository(Func<AppDbContext>? contextFactory = null)
    {
        _contextFactory = contextFactory ?? (() => new AppDbContext());
    }

    public List<TodoItem> GetAll(Guid profileId)
    {
        if (profileId == Guid.Empty)
        {
            return new List<TodoItem>();
        }

        using AppDbContext context = _contextFactory();

        return context.Todos
            .AsNoTracking()
            .Where(todo => todo.ProfileId == profileId)
            .OrderBy(todo => todo.SortOrder)
            .ToList();
    }

    public TodoItem? GetById(Guid todoId)
    {
        if (todoId == Guid.Empty)
        {
            return null;
        }

        using AppDbContext context = _contextFactory();

        return context.Todos
            .AsNoTracking()
            .FirstOrDefault(todo => todo.Id == todoId);
    }

    public void Add(TodoItem todo)
    {
        ArgumentNullException.ThrowIfNull(todo);

        if (todo.ProfileId == Guid.Empty)
        {
            throw new ArgumentException("Для задачи должен быть указан профиль.", nameof(todo));
        }

        using AppDbContext context = _contextFactory();

        int lastSortOrder = context.Todos
            .Where(existing => existing.ProfileId == todo.ProfileId)
            .Select(existing => (int?)existing.SortOrder)
            .Max() ?? 0;

        todo.Id = todo.Id == Guid.Empty ? Guid.NewGuid() : todo.Id;
        todo.LastUpdate = DateTime.Now;
        todo.SortOrder = lastSortOrder + 1;

        context.Todos.Add(todo);
        context.SaveChanges();
    }

    public void Update(TodoItem todo)
    {
        ArgumentNullException.ThrowIfNull(todo);

        using AppDbContext context = _contextFactory();

        TodoItem? existing = context.Todos.FirstOrDefault(item => item.Id == todo.Id);
        if (existing == null)
        {
            throw new InvalidOperationException("Задача не найдена.");
        }

        existing.Text = todo.Text ?? string.Empty;
        existing.Status = todo.Status;
        existing.LastUpdate = DateTime.Now;
        existing.SortOrder = todo.SortOrder;
        existing.ProfileId = todo.ProfileId;

        context.SaveChanges();
    }

    public void Delete(Guid todoId)
    {
        if (todoId == Guid.Empty)
        {
            return;
        }

        using AppDbContext context = _contextFactory();

        TodoItem? existing = context.Todos.FirstOrDefault(todo => todo.Id == todoId);
        if (existing == null)
        {
            return;
        }

        Guid profileId = existing.ProfileId;
        int removedOrder = existing.SortOrder;

        context.Todos.Remove(existing);

        List<TodoItem> tasksToShift = context.Todos
            .Where(todo => todo.ProfileId == profileId && todo.SortOrder > removedOrder)
            .ToList();

        foreach (TodoItem task in tasksToShift)
        {
            task.SortOrder--;
        }

        context.SaveChanges();
    }

    public void SetStatus(Guid todoId, TodoStatus status)
    {
        using AppDbContext context = _contextFactory();

        TodoItem? existing = context.Todos.FirstOrDefault(todo => todo.Id == todoId);
        if (existing == null)
        {
            throw new InvalidOperationException("Задача не найдена.");
        }

        existing.Status = status;
        existing.LastUpdate = DateTime.Now;
        context.SaveChanges();
    }
}
