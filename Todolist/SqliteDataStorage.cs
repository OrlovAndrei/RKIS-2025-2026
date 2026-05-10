using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Todolist.Exceptions;

public sealed class SqliteDataStorage : IDataStorage
{
    private readonly ProfileRepository _profileRepository;
    private readonly TodoRepository _todoRepository;

    public SqliteDataStorage()
        : this(new ProfileRepository(), new TodoRepository())
    {
    }

    public SqliteDataStorage(ProfileRepository profileRepository, TodoRepository todoRepository)
    {
        _profileRepository = profileRepository ?? throw new ArgumentNullException(nameof(profileRepository));
        _todoRepository = todoRepository ?? throw new ArgumentNullException(nameof(todoRepository));

        EnsureDatabase();
    }

    public void SaveProfiles(IEnumerable<Profile> profiles)
    {
        if (profiles == null) throw new ArgumentNullException(nameof(profiles));

        try
        {
            _profileRepository.ReplaceAll(profiles.ToList());
        }
        catch (StorageException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new StorageException("Failed to save profiles into SQLite.", ex);
        }
    }

    public IEnumerable<Profile> LoadProfiles()
    {
        try
        {
            return _profileRepository.GetAll();
        }
        catch (StorageException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new StorageException("Failed to load profiles from SQLite.", ex);
        }
    }

    public void SaveTodos(Guid userId, IEnumerable<TodoItem> todos)
    {
        if (userId == Guid.Empty) throw new ArgumentException("User ID is required.", nameof(userId));
        if (todos == null) throw new ArgumentNullException(nameof(todos));

        try
        {
            List<TodoItem> prepared = todos.ToList();
            foreach (TodoItem todo in prepared)
            {
                todo.ProfileId = userId;
            }

            _todoRepository.ReplaceAll(userId, prepared);
        }
        catch (StorageException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new StorageException($"Failed to save todos for user {userId} into SQLite.", ex);
        }
    }

    public IEnumerable<TodoItem> LoadTodos(Guid userId)
    {
        if (userId == Guid.Empty) throw new ArgumentException("User ID is required.", nameof(userId));

        try
        {
            return _todoRepository.GetAll(userId);
        }
        catch (StorageException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new StorageException($"Failed to load todos for user {userId} from SQLite.", ex);
        }
    }

    private static void EnsureDatabase()
    {
        try
        {
            using var context = new AppDbContext();
            context.Database.Migrate();
        }
        catch (Exception ex)
        {
            throw new StorageException("Failed to initialize SQLite database.", ex);
        }
    }
}
