using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using TodoList.Interfaces;
using ToDoList;

namespace TodoList
{
	public class ApiDataStorage : IDataStorage
	{
		private readonly HttpClient _client;
		private readonly FileManager _localManager;

		public ApiDataStorage(FileManager local)
		{
			_client = new HttpClient { BaseAddress = new Uri("http://localhost:5000/") };
			_localManager = local;
		}

		public void SaveProfiles(IEnumerable<Profile> profiles)
		{
			if (File.Exists("profiles.dat"))
			{
				byte[] data = File.ReadAllBytes("profiles.dat");
				var content = new ByteArrayContent(data);
				var response = _client.PostAsync("profiles", content).Result;
				if (!response.IsSuccessStatusCode) throw new Exception("Ошибка сервера");
			}
		}

		public IEnumerable<Profile> LoadProfiles()
		{
			var response = _client.GetAsync("profiles").Result;
			if (response.IsSuccessStatusCode)
			{
				byte[] data = response.Content.ReadAsByteArrayAsync().Result;
				File.WriteAllBytes("profiles.dat", data);
			}
			return _localManager.LoadProfiles();
		}

		public void SaveTodos(Guid userId, IEnumerable<TodoItem> todos)
		{
			string fileName = $"{userId}.dat";
			if (File.Exists(fileName))
			{
				byte[] data = File.ReadAllBytes(fileName);
				var content = new ByteArrayContent(data);
				var response = _client.PostAsync($"todos/{userId}", content).Result;
				if (!response.IsSuccessStatusCode) throw new Exception("Ошибка сервера");
			}
		}

		public IEnumerable<TodoItem> LoadTodos(Guid userId)
		{
			var response = _client.GetAsync($"todos/{userId}").Result;
			if (response.IsSuccessStatusCode)
			{
				byte[] data = response.Content.ReadAsByteArrayAsync().Result;
				File.WriteAllBytes($"{userId}.dat", data);
			}
			return _localManager.LoadTodos(userId);
		}
	}
}