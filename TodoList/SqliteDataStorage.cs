using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

public class SqliteDataStorage : IDataStorage, IDisposable
{
    private readonly string _connectionString;
    private bool _disposed = false;

    public SqliteDataStorage(string databasePath = "todo.db")
    {
        _connectionString = $"Data Source={databasePath};Foreign Keys=True";
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var createProfiles = connection.CreateCommand();
        createProfiles.CommandText = @"
            CREATE TABLE IF NOT EXISTS Profiles (
                Id TEXT PRIMARY KEY,
                Login TEXT NOT NULL UNIQUE,
                Password TEXT NOT NULL,
                FirstName TEXT NOT NULL,
                LastName TEXT NOT NULL,
                BirthYear INTEGER NOT NULL
            )";
        createProfiles.ExecuteNonQuery();

        using var createTodos = connection.CreateCommand();
        createTodos.CommandText = @"
            CREATE TABLE IF NOT EXISTS Todos (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ProfileId TEXT NOT NULL,
                Text TEXT NOT NULL,
                Status INTEGER NOT NULL,
                LastUpdate TEXT NOT NULL,
                FOREIGN KEY (ProfileId) REFERENCES Profiles(Id) ON DELETE CASCADE
            )";
        createTodos.ExecuteNonQuery();
    }

    public void SaveProfiles(IEnumerable<Profile> profiles)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            using var deleteCmd = connection.CreateCommand();
            deleteCmd.CommandText = "DELETE FROM Profiles";
            deleteCmd.ExecuteNonQuery();

            foreach (var profile in profiles)
            {
                using var insertCmd = connection.CreateCommand();
                insertCmd.CommandText = @"
                    INSERT INTO Profiles (Id, Login, Password, FirstName, LastName, BirthYear)
                    VALUES (@id, @login, @password, @firstName, @lastName, @birthYear)";
                insertCmd.Parameters.AddWithValue("@id", profile.Id.ToString());
                insertCmd.Parameters.AddWithValue("@login", profile.Login);
                insertCmd.Parameters.AddWithValue("@password", profile.Password);
                insertCmd.Parameters.AddWithValue("@firstName", profile.FirstName);
                insertCmd.Parameters.AddWithValue("@lastName", profile.LastName);
                insertCmd.Parameters.AddWithValue("@birthYear", profile.BirthYear);
                insertCmd.ExecuteNonQuery();
            }

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public IEnumerable<Profile> LoadProfiles()
    {
        var profiles = new List<Profile>();

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, Login, Password, FirstName, LastName, BirthYear FROM Profiles";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var profile = new Profile(
                Guid.Parse(reader.GetString(0)),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3),
                reader.GetString(4),
                reader.GetInt32(5)
            );
            profiles.Add(profile);
        }

        return profiles;
    }

    public void SaveTodos(Guid userId, IEnumerable<TodoItem> todos)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            using var deleteCmd = connection.CreateCommand();
            deleteCmd.CommandText = "DELETE FROM Todos WHERE ProfileId = @profileId";
            deleteCmd.Parameters.AddWithValue("@profileId", userId.ToString());
            deleteCmd.ExecuteNonQuery();
            
            foreach (var todo in todos)
            {
                using var insertCmd = connection.CreateCommand();
                insertCmd.CommandText = @"
                    INSERT INTO Todos (ProfileId, Text, Status, LastUpdate)
                    VALUES (@profileId, @text, @status, @lastUpdate)";
                insertCmd.Parameters.AddWithValue("@profileId", userId.ToString());
                insertCmd.Parameters.AddWithValue("@text", todo.Text);
                insertCmd.Parameters.AddWithValue("@status", (int)todo.Status);
                insertCmd.Parameters.AddWithValue("@lastUpdate", todo.LastUpdate.ToString("o"));
                insertCmd.ExecuteNonQuery();
            }

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public IEnumerable<TodoItem> LoadTodos(Guid userId)
    {
        var todos = new List<TodoItem>();

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT Text, Status, LastUpdate FROM Todos WHERE ProfileId = @profileId ORDER BY Id";
        command.Parameters.AddWithValue("@profileId", userId.ToString());

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var todo = new TodoItem(reader.GetString(0));
            todo.SetStatus((TodoStatus)reader.GetInt32(1));
            todo.SetLastUpdate(DateTime.Parse(reader.GetString(2)));
            todos.Add(todo);
        }

        return todos;
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _disposed = true;
        }
    }
}