using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

public sealed class TodoRepository
{
    private readonly Func<AppDbContext> _contextFactory;

    public TodoRepository(Func<AppDbContext>? contextFactory = null)
    {
        _contextFactory = contextFactory ?? (() => new AppDbContext());
    }

    public List<TodoItem> GetAll()
    {
        using AppDbContext context = _contextFactory();
        return context.Todos
            .AsNoTracking()
            .OrderBy(t => t.ProfileId)
            .ThenBy(t => t.SortOrder)
            .Select(CloneTodo)
            .ToList();
    }

    public List<TodoItem> GetAll(Guid profileId)
    {
        if (profileId == Guid.Empty)
            return new List<TodoItem>();

        using AppDbContext context = _contextFactory();
        return context.Todos
            .AsNoTracking()
            .Where(t => t.ProfileId == profileId)
            .OrderBy(t => t.SortOrder)
            .Select(CloneTodo)
            .ToList();
    }

    public void Add(TodoItem todoItem)
    {
        if (todoItem == null) throw new ArgumentNullException(nameof(todoItem));
        if (todoItem.ProfileId == Guid.Empty)
            throw new ArgumentException("Profile ID is required.", nameof(todoItem));

        using AppDbContext context = _contextFactory();
        int nextOrder = context.Todos
            .Where(t => t.ProfileId == todoItem.ProfileId)
            .Select(t => (int?)t.SortOrder)
            .Max() ?? 0;

        TodoItem entity = CloneTodo(todoItem);
        if (entity.Id == Guid.Empty)
            entity.Id = Guid.NewGuid();
        if (entity.LastUpdate == default)
            entity.LastUpdate = DateTime.Now;
        entity.SortOrder = entity.SortOrder > 0 ? entity.SortOrder : nextOrder + 1;

        context.Todos.Add(entity);
        context.SaveChanges();
    }

    public void Update(TodoItem todoItem)
    {
        if (todoItem == null) throw new ArgumentNullException(nameof(todoItem));
        if (todoItem.Id == Guid.Empty) return;

        using AppDbContext context = _contextFactory();
        TodoItem? existing = context.Todos.FirstOrDefault(t => t.Id == todoItem.Id);
        if (existing == null) return;

        existing.Text = todoItem.Text ?? string.Empty;
        existing.Status = todoItem.Status;
        existing.LastUpdate = todoItem.LastUpdate == default ? DateTime.Now : todoItem.LastUpdate;
        existing.ProfileId = todoItem.ProfileId == Guid.Empty ? existing.ProfileId : todoItem.ProfileId;
        existing.SortOrder = todoItem.SortOrder <= 0 ? existing.SortOrder : todoItem.SortOrder;

        context.SaveChanges();
    }

    public void Delete(Guid todoId)
    {
        if (todoId == Guid.Empty) return;

        using AppDbContext context = _contextFactory();
        TodoItem? existing = context.Todos.FirstOrDefault(t => t.Id == todoId);
        if (existing == null) return;

        Guid profileId = existing.ProfileId;
        int removedOrder = existing.SortOrder;

        context.Todos.Remove(existing);

        List<TodoItem> itemsToShift = context.Todos
            .Where(t => t.ProfileId == profileId && t.SortOrder > removedOrder)
            .ToList();

        foreach (TodoItem item in itemsToShift)
        {
            item.SortOrder--;
        }

        context.SaveChanges();
    }

    public void SetStatus(Guid todoId, TodoStatus status)
    {
        if (todoId == Guid.Empty) return;

        using AppDbContext context = _contextFactory();
        TodoItem? existing = context.Todos.FirstOrDefault(t => t.Id == todoId);
        if (existing == null) return;

        existing.Status = status;
        existing.LastUpdate = DateTime.Now;
        context.SaveChanges();
    }

    public void ReplaceAll(Guid profileId, IEnumerable<TodoItem> todos)
    {
        if (profileId == Guid.Empty) throw new ArgumentException("Profile ID is required.", nameof(profileId));
        if (todos == null) throw new ArgumentNullException(nameof(todos));

        List<TodoItem> source = todos.ToList();

        using AppDbContext context = _contextFactory();
        List<TodoItem> existing = context.Todos
            .Where(t => t.ProfileId == profileId)
            .ToList();

        if (existing.Count > 0)
        {
            context.Todos.RemoveRange(existing);
            context.SaveChanges();
        }

        int order = 1;
        foreach (TodoItem item in source)
        {
            TodoItem entity = CloneTodo(item);
            entity.Id = entity.Id == Guid.Empty ? Guid.NewGuid() : entity.Id;
            entity.ProfileId = profileId;
            entity.LastUpdate = entity.LastUpdate == default ? DateTime.Now : entity.LastUpdate;
            entity.SortOrder = order++;
            context.Todos.Add(entity);
        }

        context.SaveChanges();
    }

    private static TodoItem CloneTodo(TodoItem source)
    {
        return new TodoItem(source.Text)
        {
            Id = source.Id,
            Status = source.Status,
            LastUpdate = source.LastUpdate,
            SortOrder = source.SortOrder,
            ProfileId = source.ProfileId
        };
    }
}
