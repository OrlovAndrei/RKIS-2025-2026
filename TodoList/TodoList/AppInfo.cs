using System;
using System.Collections.Generic;
using System.Linq;
public static class AppInfo
{
	public static Stack<ICommand> UndoStack { get; } = new Stack<ICommand>();
	public static Stack<ICommand> RedoStack { get; } = new Stack<ICommand>();
	public static List<Profile> Profiles { get; set; } = new List<Profile>();
	public static Guid CurrentProfileId { get; set; } = Guid.Empty;
	public static TodoList CurrentUserTodos { get; set; } = new TodoList();
	public static string DataDir { get; set; } = "Data";
	public static bool ShouldLogout { get; set; } = false;
	private static IDataStorage _dataStorage;
	public static Profile CurrentProfile => Profiles.FirstOrDefault(p => p.Id == CurrentProfileId);
	public static void Initialize(IDataStorage dataStorage)
	{
		_dataStorage = dataStorage ?? throw new ArgumentNullException(nameof(dataStorage));
	}
	public static void LoadData()
	{
		if (_dataStorage == null)
			throw new InvalidOperationException("AppInfo не инициализирован.");

		Profiles = _dataStorage.LoadProfiles().ToList();

		if (CurrentProfileId != Guid.Empty && Profiles.Any(p => p.Id == CurrentProfileId))
		{
			CurrentUserTodos = new TodoList(_dataStorage.LoadTodos(CurrentProfileId).ToList());
		}
		else if (Profiles.Any())
		{
			CurrentProfileId = Profiles.First().Id;
			CurrentUserTodos = new TodoList(_dataStorage.LoadTodos(CurrentProfileId).ToList());
		}
		else
		{
			CurrentUserTodos = new TodoList();
		}
	}
	public static void SaveData()
	{
		if (_dataStorage == null)
			throw new InvalidOperationException("AppInfo не инициализирован.");

		_dataStorage.SaveProfiles(Profiles);
		if (CurrentProfileId != Guid.Empty)
		{
			_dataStorage.SaveTodos(CurrentProfileId, CurrentUserTodos);
		}
	}
}
