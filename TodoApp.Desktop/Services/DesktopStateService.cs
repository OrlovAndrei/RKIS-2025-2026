using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Desktop.Services;

public class DesktopStateService
{
	private readonly ProfileRepository _profileRepository;
	private readonly TodoRepository _todoRepository;

	public DesktopStateService()
	{
		using var context = new AppDbContext();
		context.Database.Migrate();

		_profileRepository = new ProfileRepository();
		_todoRepository = new TodoRepository();

		Profiles = new ObservableCollection<Profile>();
		Tasks = new ObservableCollection<TodoItem>();

		ReloadProfiles();
	}

	public ObservableCollection<Profile> Profiles { get; }
	public ObservableCollection<TodoItem> Tasks { get; }
	public Profile? CurrentProfile { get; private set; }

	public bool Login(string login, string password)
	{
		var profile = _profileRepository.GetByCredentials(login, password);
		if (profile == null)
		{
			return false;
		}

		CurrentProfile = profile;
		ReloadProfiles();
		ReloadTasksForCurrentProfile();
		return true;
	}

	public void Logout()
	{
		CurrentProfile = null;
		Tasks.Clear();
	}

	public Profile RegisterProfile(string login, string password, string firstName, string? lastName, int birthYear)
	{
		var profile = new Profile(login, password, firstName, lastName, birthYear);
		_profileRepository.Add(profile);
		ReloadProfiles();
		CurrentProfile = _profileRepository.GetByCredentials(login, password) ?? profile;
		ReloadTasksForCurrentProfile();
		return CurrentProfile;
	}

	public void ReloadProfiles()
	{
		ReplaceCollection(Profiles, _profileRepository.GetAll());
	}

	public void ReloadTasksForCurrentProfile()
	{
		if (CurrentProfile == null)
		{
			Tasks.Clear();
			return;
		}

		ReplaceCollection(Tasks, _todoRepository.GetAllByProfile(CurrentProfile.Id));
	}

	public TodoItem AddTask(string text, TodoStatus status)
	{
		if (CurrentProfile == null)
		{
			throw new InvalidOperationException("Нужно войти в профиль.");
		}

		var item = new TodoItem
		{
			Id = Tasks.Any() ? Tasks.Max(task => task.Id) + 1 : 1,
			Text = text.Trim(),
			Status = status,
			CreatedAt = DateTime.Now,
			LastUpdated = DateTime.Now,
			ProfileId = CurrentProfile.Id
		};

		_todoRepository.Add(item);
		ReloadTasksForCurrentProfile();

		return Tasks.OrderByDescending(task => task.Id).First();
	}

	public void UpdateTask(TodoItem task, string newText, TodoStatus newStatus)
	{
		task.Text = newText.Trim();
		task.Status = newStatus;
		task.LastUpdated = DateTime.Now;

		_todoRepository.Update(task);
		ReloadTasksForCurrentProfile();
	}

	public void DeleteTask(TodoItem task)
	{
		_todoRepository.Delete(task.Id);
		ReloadTasksForCurrentProfile();
	}

	public void UpdateTaskStatus(TodoItem task, TodoStatus status)
	{
		_todoRepository.SetStatus(task.Id, status);
		ReloadTasksForCurrentProfile();
	}

	private static void ReplaceCollection<T>(ObservableCollection<T> target, IEnumerable<T> source)
	{
		target.Clear();
		foreach (var item in source)
		{
			target.Add(item);
		}
	}
}
