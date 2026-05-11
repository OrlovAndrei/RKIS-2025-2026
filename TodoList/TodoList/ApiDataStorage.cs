using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Security.Cryptography;
using System.IO;
public class ApiDataStorage : IDataStorage
{
	private readonly HttpClient _httpClient;
	private readonly string _baseUrl;
	private readonly byte[] _encryptionKey;
	private readonly byte[] _encryptionIV;
	public ApiDataStorage(string baseUrl, byte[] encryptionKey, byte[] encryptionIV)
	{
		_baseUrl = baseUrl;
		_httpClient = new HttpClient();
		_encryptionKey = encryptionKey;
		_encryptionIV = encryptionIV;
	}
	public void SaveProfiles(IEnumerable<Profile> profiles)
	{
		try
		{
			var jsonString = JsonSerializer.Serialize(profiles, new JsonSerializerOptions { WriteIndented = true });
			var encryptedData = EncryptString(jsonString);
			var content = new ByteArrayContent(encryptedData);
			content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
			var response = _httpClient.PostAsync($"{_baseUrl}/profiles", content).Result;
			response.EnsureSuccessStatusCode();
			Console.WriteLine("Профили отправлены на сервер");
		}
		catch (Exception ex)
		{
			throw new TodoApp.Exceptions.StorageException($"Ошибка сохранения профилей на сервер: {ex.Message}", ex);
		}
	}
	public IEnumerable<Profile> LoadProfiles()
	{
		try
		{
			var response = _httpClient.GetAsync($"{_baseUrl}/profiles").Result;
			if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
			{
				return Enumerable.Empty<Profile>();
			}

			response.EnsureSuccessStatusCode();
			var encryptedData = response.Content.ReadAsByteArrayAsync().Result;
			if (encryptedData == null || encryptedData.Length == 0)
			{
				return Enumerable.Empty<Profile>();
			}
			var jsonString = DecryptBytes(encryptedData);
			return JsonSerializer.Deserialize<IEnumerable<Profile>>(jsonString) ?? Enumerable.Empty<Profile>();
		}
		catch (Exception ex)
		{
			throw new TodoApp.Exceptions.StorageException($"Ошибка загрузки профилей с сервера: {ex.Message}", ex);
		}
	}
	public void SaveTodos(Guid userId, IEnumerable<TodoItem> todos)
	{
		try
		{
			var jsonString = JsonSerializer.Serialize(todos, new JsonSerializerOptions { WriteIndented = true });
			var encryptedData = EncryptString(jsonString);
			var content = new ByteArrayContent(encryptedData);
			content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

			var response = _httpClient.PostAsync($"{_baseUrl}/todos/{userId}", content).Result;
			response.EnsureSuccessStatusCode();

			Console.WriteLine($"Задачи пользователя {userId} отправлены на сервер");
		}
		catch (Exception ex)
		{
			throw new TodoApp.Exceptions.StorageException($"Ошибка сохранения задач на сервер: {ex.Message}", ex);
		}
	}
	public IEnumerable<TodoItem> LoadTodos(Guid userId)
	{
		try
		{
			var response = _httpClient.GetAsync($"{_baseUrl}/todos/{userId}").Result;

			if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
			{
				return Enumerable.Empty<TodoItem>();
			}
			response.EnsureSuccessStatusCode();
			var encryptedData = response.Content.ReadAsByteArrayAsync().Result;

			if (encryptedData == null || encryptedData.Length == 0)
			{
				return Enumerable.Empty<TodoItem>();
			}
			var jsonString = DecryptBytes(encryptedData);
			return JsonSerializer.Deserialize<IEnumerable<TodoItem>>(jsonString) ?? Enumerable.Empty<TodoItem>();
		}
		catch (Exception ex)
		{
			throw new TodoApp.Exceptions.StorageException($"Ошибка загрузки задач с сервера: {ex.Message}", ex);
		}
	}
	private byte[] EncryptString(string plainText)
	{
		using (Aes aesAlg = Aes.Create())
		{
			aesAlg.Key = _encryptionKey;
			aesAlg.IV = _encryptionIV;

			using (var ms = new MemoryStream())
			{
				using (var cs = new CryptoStream(ms, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
				using (var sw = new StreamWriter(cs))
				{
					sw.Write(plainText);
				}
				return ms.ToArray();
			}
		}
	}
	private string DecryptBytes(byte[] cipherText)
	{
		using (Aes aesAlg = Aes.Create())
		{
			aesAlg.Key = _encryptionKey;
			aesAlg.IV = _encryptionIV;

			using (var ms = new MemoryStream(cipherText))
			using (var cs = new CryptoStream(ms, aesAlg.CreateDecryptor(), CryptoStreamMode.Read))
			using (var sr = new StreamReader(cs))
			{
				return sr.ReadToEnd();
			}
		}
	}
	public bool IsAvailable()
	{
		try
		{
			var response = _httpClient.GetAsync($"{_baseUrl}health").Result;
			return response.IsSuccessStatusCode;
		}
		catch
		{
			return false;
		}
	}

	public string GetBaseUrl() => _baseUrl;
}