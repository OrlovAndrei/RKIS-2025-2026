using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

public class SqliteDataStorage : IDataStorage, IDisposable
{
    private readonly TodoDbContext _context;
    private bool _disposed = false;

    public SqliteDataStorage(string connectionString = "Data Source=todo.db")
    {
        _context = new TodoDbContext();
        _context.Database.EnsureCreated();
    }

    public void SaveProfiles(IEnumerable<Profile> profiles)
    {
        _context.Profiles.RemoveRange(_context.Profiles);
        _context.Profiles.AddRange(profiles);
        _context.SaveChanges();
    }

    public IEnumerable<Profile> LoadProfiles()
    {
        return _context.Profiles.Include(p => p.Todos).ToList();
    }

    public void SaveTodos(Guid userId, IEnumerable<TodoItem> todos)
    {
        var oldTodos = _context.Todos.Where(t => t.ProfileId == userId);
        _context.Todos.RemoveRange(oldTodos);

        foreach (var todo in todos)
        {
            todo.ProfileId = userId;
            _context.Todos.Add(todo);
        }
        _context.SaveChanges();
    }

    public IEnumerable<TodoItem> LoadTodos(Guid userId)
    {
        return _context.Todos
            .Where(t => t.ProfileId == userId)
            .OrderBy(t => t.Id)
            .ToList();
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _context.Dispose();
            _disposed = true;
        }
    }
}