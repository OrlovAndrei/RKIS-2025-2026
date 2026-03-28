using System.Collections.Concurrent;
using System.Text.Json;
using TodoList.Server.Models;
namespace TodoList.Server.Storage;
public class ServerStorageManager
{
	private readonly string _dataDirectory;
	private readonly string _profilesFilePath;
	private readonly string _todosDirectoryPath;
	private readonly object _lockObject = new();

	private ConcurrentDictionary<Guid, List<TodoItemDto>> _todosCache = new();
	private List<ProfileDto> _profilesCache = new();
	public ServerStorageManager(string dataDirectory = "ServerData")
	{
		_dataDirectory = dataDirectory;
		_profilesFilePath = Path.Combine(_dataDirectory, "profiles.json");
		_todosDirectoryPath = Path.Combine(_dataDirectory, "todos");

		Directory.CreateDirectory(_dataDirectory);
		Directory.CreateDirectory(_todosDirectoryPath);

		LoadData();
	}
	private void LoadData()
	{
		lock (_lockObject)
		{
			if (File.Exists(_profilesFilePath))
			{
				var json = File.ReadAllText(_profilesFilePath);
				_profilesCache = JsonSerializer.Deserialize<List<ProfileDto>>(json) ?? new List<ProfileDto>();
			}
			foreach (var file in Directory.GetFiles(_todosDirectoryPath, "*.json"))
			{
				var fileName = Path.GetFileNameWithoutExtension(file);
				if (Guid.TryParse(fileName, out var userId))
				{
					var json = File.ReadAllText(file);
					var todos = JsonSerializer.Deserialize<List<TodoItemDto>>(json) ?? new List<TodoItemDto>();
					_todosCache[userId] = todos;
				}
			}
		}
	}
	public void SaveData()
	{
		lock (_lockObject)
		{
			var profilesJson = JsonSerializer.Serialize(_profilesCache, new JsonSerializerOptions { WriteIndented = true });
			File.WriteAllText(_profilesFilePath, profilesJson);

			foreach (var (userId, todos) in _todosCache)
			{
				var todosJson = JsonSerializer.Serialize(todos, new JsonSerializerOptions { WriteIndented = true });
				var todoFilePath = Path.Combine(_todosDirectoryPath, $"{userId}.json");
				File.WriteAllText(todoFilePath, todosJson);
			}
		}
	}
	public List<ProfileDto> GetAllProfiles()
	{
		lock (_lockObject)
		{
			return _profilesCache.ToList();
		}
	}
	public ProfileDto? GetProfileById(Guid id)
	{
		lock (_lockObject)
		{
			return _profilesCache.FirstOrDefault(p => p.Id == id);
		}
	}
	public ProfileDto? GetProfileByLogin(string login)
	{
		lock (_lockObject)
		{
			return _profilesCache.FirstOrDefault(p => p.Login == login);
		}
	}
	public void AddOrUpdateProfile(ProfileDto profile)
	{
		lock (_lockObject)
		{
			var existing = _profilesCache.FirstOrDefault(p => p.Id == profile.Id);
			if (existing != null)
			{
				var index = _profilesCache.IndexOf(existing);
				_profilesCache[index] = profile;
			}
			else
			{
				_profilesCache.Add(profile);
			}
			SaveData();
		}
	}
	public List<TodoItemDto> GetTodos(Guid userId)
	{
		lock (_lockObject)
		{
			return _todosCache.GetValueOrDefault(userId, new List<TodoItemDto>()).ToList();
		}
	}
	public void SaveTodos(Guid userId, List<TodoItemDto> todos)
	{
		lock (_lockObject)
		{
			_todosCache[userId] = todos.ToList();
			SaveData();
		}
	}
	public SyncResult SyncUserData(Guid userId, List<ProfileDto>? profiles, List<TodoItemDto>? todos)
	{
		lock (_lockObject)
		{
			var result = new SyncResult();

			if (profiles != null && profiles.Any())
			{
				var profile = profiles.First();
				var existingProfile = _profilesCache.FirstOrDefault(p => p.Id == profile.Id);

				if (existingProfile == null)
				{
					_profilesCache.Add(profile);
					result.ProfilesSynced = true;
					result.Message = "Профиль создан на сервере";
				}
				else
				{
					// Обновляем существующий профиль
					var index = _profilesCache.IndexOf(existingProfile);
					_profilesCache[index] = profile;
					result.ProfilesSynced = true;
					result.Message = "Профиль обновлен на сервере";
				}
			}
			if (todos != null)
			{
				_todosCache[userId] = todos.ToList();
				result.TodosSynced = true;
				result.Message += (string.IsNullOrEmpty(result.Message) ? "" : " ") + $"Задачи синхронизированы ({todos.Count})";
			}
			SaveData();
			return result;
		}
	}
}
public class SyncResult
{
	public bool ProfilesSynced { get; set; }
	public bool TodosSynced { get; set; }
	public string? Message { get; set; }
}