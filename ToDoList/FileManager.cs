using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using TodoList.Interfaces;
using ToDoList;

namespace TodoList
{
	public class FileManager : IDataStorage
	{
		private readonly byte[] _key;
		private readonly byte[] _iv;
		private const string ProfilesFile = "profiles.dat";

		public FileManager(string key, string iv)
		{
			_key = Encoding.UTF8.GetBytes(key);
			_iv = Encoding.UTF8.GetBytes(iv);
		}

		public void SaveProfiles(IEnumerable<Profile> profiles)
		{
			try
			{
				using var fs = new FileStream(ProfilesFile, FileMode.Create, FileAccess.Write);
				using var bs = new BufferedStream(fs);

				using var aes = Aes.Create();
				aes.Key = _key;
				aes.IV = _iv;

				using var encryptor = aes.CreateEncryptor();
				using var cs = new CryptoStream(bs, encryptor, CryptoStreamMode.Write);
				using var writer = new StreamWriter(cs);

				string json = JsonSerializer.Serialize(profiles);
				writer.Write(json);
			}
			catch (IOException ex)
			{
				throw new StorageException("Ошибка записи в файл профилей", ex);
			}
			catch (Exception ex)
			{
				throw new StorageException("Непредвиденная ошибка при сохранении", ex);
			}
		}


		public IEnumerable<Profile> LoadProfiles()
		{
			if (!File.Exists(ProfilesFile))
			{
				return new List<Profile>();
			}

			try
			{
				using var fs = new FileStream(ProfilesFile, FileMode.Open, FileAccess.Read);
				using var bs = new BufferedStream(fs);

				using var aes = Aes.Create();
				aes.Key = _key;
				aes.IV = _iv;

				using var decryptor = aes.CreateDecryptor();
				using var cs = new CryptoStream(bs, decryptor, CryptoStreamMode.Read);
				using var reader = new StreamReader(cs);

				string json = reader.ReadToEnd();
				return JsonSerializer.Deserialize<List<Profile>>(json) ?? new List<Profile>();
			}
			catch (CryptographicException ex)
			{
				throw new StorageException("Ошибка расшифровки данных. Проверьте ключи.", ex);
			}
			catch (Exception ex)
			{
				throw new StorageException("Не удалось загрузить профили из файла", ex);
			}
		}

		public void SaveTodos(Guid userId, IEnumerable<TodoItem> todos)
		{
			string fileName = $"{userId}.dat";
			try
			{
				using var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
				using var bs = new BufferedStream(fs);

				using var aes = Aes.Create();
				aes.Key = _key;
				aes.IV = _iv;

				using var encryptor = aes.CreateEncryptor();
				using var cs = new CryptoStream(bs, encryptor, CryptoStreamMode.Write);
				using var writer = new StreamWriter(cs);

				string json = JsonSerializer.Serialize(todos);
				writer.Write(json);
			}
			catch (Exception ex)
			{
				throw new StorageException($"Ошибка при сохранении задач пользователя {userId}", ex);
			}
		}

		public IEnumerable<TodoItem> LoadTodos(Guid userId)
		{
			string fileName = $"{userId}.dat";
			if (!File.Exists(fileName)) return new List<TodoItem>();

			try
			{
				using var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
				using var bs = new BufferedStream(fs);

				using var aes = Aes.Create();
				aes.Key = _key;
				aes.IV = _iv;

				using var decryptor = aes.CreateDecryptor();
				using var cs = new CryptoStream(bs, decryptor, CryptoStreamMode.Read);
				using var reader = new StreamReader(cs);

				string json = reader.ReadToEnd();
				return JsonSerializer.Deserialize<List<TodoItem>>(json) ?? new List<TodoItem>();
			}
			catch (Exception ex)
			{
				throw new StorageException("Не удалось загрузить профили", ex);
			}
		}
	}
}