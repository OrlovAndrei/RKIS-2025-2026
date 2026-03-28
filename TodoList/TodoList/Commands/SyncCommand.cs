using System;
using System.Net.Http;
using TodoApp.Exceptions;
public class SyncCommand : ICommand
{
	public bool Pull { get; set; }
	public bool Push { get; set; }
	public void Execute()
	{
		try
		{
			if (!AppInfo.IsApiStorage)
			{
				Console.WriteLine("Ошибка: синхронизация доступна только в режиме сетевого хранилища.");
				Console.WriteLine("Для использования синхронизации выберите режим 2 при запуске программы.");
				return;
			}
			if (!IsServerAvailable())
			{
				Console.WriteLine("Ошибка: сервер недоступен.");
				Console.WriteLine("Убедитесь, что сервер запущен на порту 5000.");
				return;
			}
			if (Pull)
			{
				Console.WriteLine("Синхронизация: загрузка данных с сервера (pull)...");
				var currentProfileId = AppInfo.CurrentProfileId;
				AppInfo.LoadData();
				if (currentProfileId != Guid.Empty && AppInfo.Profiles.Any(p => p.Id == currentProfileId))
				{
					AppInfo.CurrentProfileId = currentProfileId;
					AppInfo.CurrentUserTodos = new TodoList(AppInfo.LoadTodos(currentProfileId).ToList());
				}
				Console.WriteLine("✓ Данные успешно загружены с сервера");
			}
			else if (Push)
			{
				Console.WriteLine("Синхронизация: отправка данных на сервер (push)...");
				AppInfo.SaveData();
				Console.WriteLine("✓ Данные успешно отправлены на сервер");
			}
			else
			{
				Console.WriteLine("Синхронизация: полная синхронизация...");
				AppInfo.SaveData();
				Console.WriteLine("  ✓ Данные отправлены на сервер");
				var currentProfileId = AppInfo.CurrentProfileId;
				AppInfo.LoadData();

				if (currentProfileId != Guid.Empty && AppInfo.Profiles.Any(p => p.Id == currentProfileId))
				{
					AppInfo.CurrentProfileId = currentProfileId;
					AppInfo.CurrentUserTodos = new TodoList(AppInfo.LoadTodos(currentProfileId).ToList());
				}

				Console.WriteLine("  ✓ Данные загружены с сервера");
				Console.WriteLine("✓ Полная синхронизация завершена");
			}
		}
		catch (StorageException ex)
		{
			Console.WriteLine($"✗ Ошибка синхронизации: {ex.Message}");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"✗ Неожиданная ошибка: {ex.Message}");
		}
	}
	private bool IsServerAvailable()
	{
		try
		{
			if (AppInfo.DataStorage is ApiDataStorage apiStorage)
			{
				return apiStorage.IsAvailable();
			}
			string[] possibleUrls = new[]
			{
				"http://localhost:5000/",
				"http://localhost:8080/",
				"http://127.0.0.1:5000/",
				"http://127.0.0.1:8080/"
			};
			using (var client = new HttpClient())
			{
				client.Timeout = TimeSpan.FromSeconds(3);

				foreach (var url in possibleUrls)
				{
					try
					{
						var response = client.GetAsync($"{url}health").Result;
						if (response.IsSuccessStatusCode)
						{
							return true;
						}
					}
					catch
					{
						continue;
					}
				}
			}
			return false;
		}
		catch
		{
			return false;
		}
	}
}