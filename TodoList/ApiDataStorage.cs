using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using TodoList.Models;
namespace TodoApp.Commands
{
	public class ApiDataStorage : IDataStorage
		{
			private readonly HttpClient _httpClient;
			private readonly string _baseUrl;
			private readonly byte[] _key;
			private readonly byte[] _iv;
			public ApiDataStorage(string baseUrl, byte[] key, byte[] iv)
			{
				_baseUrl = baseUrl;
				_key = key;
				_iv = iv;
				_httpClient = new HttpClient();
				_httpClient.Timeout = TimeSpan.FromSeconds(30);
			}
			public void SaveProfiles(IEnumerable<Profile> profiles)
			{
				try
				{
					string json = JsonSerializer.Serialize(profiles);
					byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
					byte[] encryptedData = EncryptData(jsonBytes);
					var content = new ByteArrayContent(encryptedData);
					var response = _httpClient.PostAsync($"{_baseUrl}/profiles", content).Result;
					response.EnsureSuccessStatusCode();
				}
				catch (Exception ex)
				{
					throw new DataStorageException($"Ошибка при сохранении профилей на сервер: {ex.Message}", ex);
				}
			}
			public IEnumerable<Profile> LoadProfiles()
			{
				try
				{
					var response = _httpClient.GetAsync($"{_baseUrl}/profiles").Result;
					response.EnsureSuccessStatusCode();
					byte[] encryptedData = response.Content.ReadAsByteArrayAsync().Result;
					if (encryptedData == null || encryptedData.Length == 0)
						return new List<Profile>();
					byte[] decryptedData = DecryptData(encryptedData);
					string json = Encoding.UTF8.GetString(decryptedData);
					return JsonSerializer.Deserialize<List<Profile>>(json) ?? new List<Profile>();
				}
				catch (Exception ex)
				{
					throw new DataStorageException($"Ошибка при загрузке профилей с сервера: {ex.Message}", ex);
				}
			}
			public void SaveTodos(Guid userId, IEnumerable<TodoItem> todos)
			{
				try
				{
					string json = JsonSerializer.Serialize(todos);
					byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
					byte[] encryptedData = EncryptData(jsonBytes);
					var content = new ByteArrayContent(encryptedData);
					var response = _httpClient.PostAsync($"{_baseUrl}/todos/{userId}", content).Result;
					response.EnsureSuccessStatusCode();
				}
				catch (Exception ex)
				{
					throw new DataStorageException($"Ошибка при сохранении задач на сервер: {ex.Message}", ex);
				}
			}
			public IEnumerable<TodoItem> LoadTodos(Guid userId)
			{
				try
				{
					var response = _httpClient.GetAsync($"{_baseUrl}/todos/{userId}").Result;
					response.EnsureSuccessStatusCode();
					byte[] encryptedData = response.Content.ReadAsByteArrayAsync().Result;
					if (encryptedData == null || encryptedData.Length == 0)
						return new List<TodoItem>();
					byte[] decryptedData = DecryptData(encryptedData);
					string json = Encoding.UTF8.GetString(decryptedData);
					return JsonSerializer.Deserialize<List<TodoItem>>(json) ?? new List<TodoItem>();
				}
				catch (Exception ex)
				{
					throw new DataStorageException($"Ошибка при загрузке задач с сервера: {ex.Message}", ex);
				}
			}
			private byte[] EncryptData(byte[] data)
			{
				using (var aes = Aes.Create())
				{
					aes.Key = _key;
					aes.IV = _iv;
					using (var memoryStream = new MemoryStream())
					{
						using (var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
						{
							cryptoStream.Write(data, 0, data.Length);
							cryptoStream.FlushFinalBlock();
							return memoryStream.ToArray();
						}
					}
				}
			}
			private byte[] DecryptData(byte[] encryptedData)
			{
				using (var aes = Aes.Create())
				{
					aes.Key = _key;
					aes.IV = _iv;
					using (var memoryStream = new MemoryStream(encryptedData))
					using (var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
					{
						using (var resultStream = new MemoryStream())
						{
							cryptoStream.CopyTo(resultStream);
							return resultStream.ToArray();
						}
					}
				}
			}
			public void Dispose()
			{
				_httpClient?.Dispose();
			}
		}
}
