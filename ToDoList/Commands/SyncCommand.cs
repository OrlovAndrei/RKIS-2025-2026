using System;
using System.Linq;
using System.Net.Http;

namespace TodoList
{
	public class SyncCommand
	{
		private readonly ApiDataStorage _apiStorage;

		public SyncCommand(ApiDataStorage apiStorage)
		{
			_apiStorage = apiStorage;
		}

		public void Execute(string[] args)
		{
			try
			{
				if (args.Contains("--push"))
				{
					Console.WriteLine("Синхронизация: отправка данных...");
					var profiles = AppInfo.Storage.LoadProfiles();
					_apiStorage.SaveProfiles(profiles);

					if (AppInfo.CurrentProfile != null)
					{
						var tasks = AppInfo.Storage.LoadTodos(AppInfo.CurrentProfile.Id);
						_apiStorage.SaveTodos(AppInfo.CurrentProfile.Id, tasks);
					}
					Console.WriteLine("Готово: данные на сервере.");
				}
				else if (args.Contains("--pull"))
				{
					Console.WriteLine("Синхронизация: получение данных...");
					_apiStorage.LoadProfiles();

					if (AppInfo.CurrentProfile != null)
					{
						_apiStorage.LoadTodos(AppInfo.CurrentProfile.Id);
					}
					Console.WriteLine("Готово: данные обновлены.");
				}
				else
				{
					Console.WriteLine("Ошибка: используйте --push или --pull");
				}
			}
			catch (Exception)
			{
				Console.WriteLine("Ошибка: сервер недоступен.");
			}
		}
	}
}