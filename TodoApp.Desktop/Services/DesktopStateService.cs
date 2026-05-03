using System.Collections.ObjectModel;
using TodoApp.Models;

namespace TodoApp.Desktop.Services;

public class DesktopStateService
{
	public DesktopStateService()
	{
		Profiles = new ObservableCollection<Profile>();
		Tasks = new ObservableCollection<TodoItem>();
		SeedDemoData();
	}

	public ObservableCollection<Profile> Profiles { get; }
	public ObservableCollection<TodoItem> Tasks { get; }
	public Profile? CurrentProfile { get; set; }

	public IEnumerable<TodoItem> CurrentProfileTasks =>
		CurrentProfile == null
			? Enumerable.Empty<TodoItem>()
			: Tasks.Where(task => task.ProfileId == CurrentProfile.Id);

	private void SeedDemoData()
	{
		var demoProfile = new Profile("demo", "demo", "Иван", "Петров", 2002);
		Profiles.Add(demoProfile);

		Tasks.Add(new TodoItem
		{
			Id = 1,
			Text = "Подготовить архитектуру WPF-приложения",
			Status = TodoStatus.InProgress,
			CreatedAt = DateTime.Now.AddDays(-2),
			LastUpdated = DateTime.Now.AddHours(-3),
			ProfileId = demoProfile.Id
		});

		Tasks.Add(new TodoItem
		{
			Id = 2,
			Text = "Сделать форму авторизации",
			Status = TodoStatus.NotStarted,
			CreatedAt = DateTime.Now.AddDays(-1),
			LastUpdated = DateTime.Now.AddHours(-1),
			ProfileId = demoProfile.Id
		});

		Tasks.Add(new TodoItem
		{
			Id = 3,
			Text = "Добавить поиск и фильтрацию",
			Status = TodoStatus.Completed,
			CreatedAt = DateTime.Now.AddDays(-4),
			LastUpdated = DateTime.Now.AddDays(-1),
			ProfileId = demoProfile.Id
		});
	}
}
