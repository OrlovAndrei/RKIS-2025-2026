using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Net.Http;
namespace TodoApp.Commands
{
	public class SyncCommand : BaseCommand
	{
		private readonly IDataStorage _localStorage;
		private readonly ApiDataStorage _apiStorage;
		private readonly bool _pull;
		private readonly bool _push;
		public SyncCommand(IDataStorage localStorage, ApiDataStorage apiStorage, bool pull, bool push, Guid? currentProfileId)
		{
			_localStorage = localStorage;
			_apiStorage = apiStorage;
			_pull = pull;
			_push = push;
		}
		public override void Execute()
		{
			try
			{
				if (!IsServerAvailable())
				{
					Console.WriteLine("Ошибка: сервер недоступен.");
					return;
				}
				if (_pull && _push)
				{
					Console.WriteLine("Нельзя использовать оба флага --pull и --push одновременно.");
					return;
				}
				if (_pull)
				{
					PullData();
				}
				else if (_push)
				{
					PushData();
				}
				else
				{
					Console.WriteLine("Использование: sync --pull или sync --push");
				}
			}
			catch (Exception ex)
			{
				throw new DataStorageException($"Ошибка при синхронизации: {ex.Message}", ex);
			}
		}
		private bool IsServerAvailable()
		{
			try
			{
				using (var client = new HttpClient())
				{
					client.Timeout = TimeSpan.FromSeconds(5);
					var response = client.GetAsync("http://localhost:5000/profiles").Result;
					return true;
				}
			}
			catch
			{
				return false;
			}
		}
		private void PullData()
		{
			Console.WriteLine("Синхронизация: получение данных с сервера...");
			var serverProfiles = _apiStorage.LoadProfiles().ToList();
			if (CurrentProfileId.HasValue)
			{
				var serverTodos = _apiStorage.LoadTodos(CurrentProfileId.Value).ToList();
				_localStorage.SaveProfiles(serverProfiles);
				_localStorage.SaveTodos(CurrentProfileId.Value, serverTodos);
				Console.WriteLine($"Синхронизация завершена. Загружено профилей: {serverProfiles.Count}, задач: {serverTodos.Count}");
			}
			else
			{
				Console.WriteLine("Ошибка: не выбран профиль пользователя.");
			}
		}
		private void PushData()
		{
			Console.WriteLine("Синхронизация: отправка данных на сервер...");
			var localProfiles = _localStorage.LoadProfiles().ToList();
			_apiStorage.SaveProfiles(localProfiles);
			if (CurrentProfileId.HasValue)
			{
				var localTodos = _localStorage.LoadTodos(CurrentProfileId.Value).ToList();
				_apiStorage.SaveTodos(CurrentProfileId.Value, localTodos);
				Console.WriteLine($"Синхронизация завершена. Отправлено профилей: {localProfiles.Count}, задач: {localTodos.Count}");
			}
			else
			{
				Console.WriteLine("Ошибка: не выбран профиль пользователя.");
			}
		}
	}
}