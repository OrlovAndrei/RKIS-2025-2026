using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
public class FileManager : IDataStorage
{
	private readonly string _profilesFilePath;
	private readonly string _todosDirectoryPath;
	private readonly byte[] _encryptionKey;
	private readonly byte[] _encryptionIV;
	public FileManager(string profilesFilePath, string todosDirectoryPath, byte[] encryptionKey, byte[] encryptionIV)
	{
		_profilesFilePath = profilesFilePath ?? throw new ArgumentNullException(nameof(profilesFilePath));
		_todosDirectoryPath = todosDirectoryPath ?? throw new ArgumentNullException(nameof(todosDirectoryPath));
		_encryptionKey = encryptionKey ?? throw new ArgumentNullException(nameof(encryptionKey));
		_encryptionIV = encryptionIV ?? throw new ArgumentNullException(nameof(encryptionIV));
		if (!Directory.Exists(_todosDirectoryPath))
		{
			Directory.CreateDirectory(_todosDirectoryPath);
		}
	}
	private string GetTodoFilePath(Guid userId)
	{
		return Path.Combine(_todosDirectoryPath, $"{userId}_todos.json");
	}
	public void SaveProfiles(IEnumerable<Profile> profiles)
	{
		if (profiles == null) return;

		var jsonString = JsonSerializer.Serialize(profiles, new JsonSerializerOptions { WriteIndented = true });
		EncryptAndSaveToFile(_profilesFilePath, jsonString);
	}
	public IEnumerable<Profile> LoadProfiles()
	{
		if (!File.Exists(_profilesFilePath))
		{
			return Enumerable.Empty<Profile>();
		}
		try
		{
			var jsonString = DecryptAndLoadFromFile(_profilesFilePath);
			return JsonSerializer.Deserialize<IEnumerable<Profile>>(jsonString) ?? Enumerable.Empty<Profile>();
		}
		catch (JsonException ex)
		{
			throw new TodoApp.Exceptions.DataCorruptedException($"Ошибка десериализации данных профилей: {ex.Message}", ex);
		}
		catch (Exception ex) when (ex is TodoApp.Exceptions.DecryptionException || ex is IOException)
		{
			throw new TodoApp.Exceptions.StorageException($"Ошибка загрузки или расшифровки профилей: {ex.Message}", ex);
		}
	}
	public void SaveTodos(Guid userId, IEnumerable<TodoItem> todos)
	{
		if (todos == null) return;

		var todoFilePath = GetTodoFilePath(userId);
		var jsonString = JsonSerializer.Serialize(todos, new JsonSerializerOptions { WriteIndented = true });
		EncryptAndSaveToFile(todoFilePath, jsonString);
	}
	public IEnumerable<TodoItem> LoadTodos(Guid userId)
	{
		var todoFilePath = GetTodoFilePath(userId);
		if (!File.Exists(todoFilePath))
		{
			return Enumerable.Empty<TodoItem>();
		}
		try
		{
			var jsonString = DecryptAndLoadFromFile(todoFilePath);
			return JsonSerializer.Deserialize<IEnumerable<TodoItem>>(jsonString) ?? Enumerable.Empty<TodoItem>();
		}
		catch (JsonException ex)
		{
			throw new TodoApp.Exceptions.DataCorruptedException($"Ошибка десериализации данных задач для пользователя {userId}: {ex.Message}", ex);
		}
		catch (Exception ex) when (ex is TodoApp.Exceptions.DecryptionException || ex is IOException)
		{
			throw new TodoApp.Exceptions.StorageException($"Ошибка загрузки или расшифровки задач для пользователя {userId}: {ex.Message}", ex);
		}
	}
	private void EncryptAndSaveToFile(string filePath, string content)
	{
		string? directory = Path.GetDirectoryName(filePath);
		if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
		{
			Directory.CreateDirectory(directory);
		}
		using (Aes aesAlg = Aes.Create())
		{
			aesAlg.Key = _encryptionKey;
			aesAlg.IV = _encryptionIV;

			using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
			using (BufferedStream bs = new BufferedStream(fs))
			using (ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
			using (CryptoStream cs = new CryptoStream(bs, encryptor, CryptoStreamMode.Write))
			using (StreamWriter sw = new StreamWriter(cs))
			{
				sw.Write(content);
			}
		}
	}
	private string DecryptAndLoadFromFile(string filePath)
	{
		if (!File.Exists(filePath))
		{
			throw new FileNotFoundException($"Файл не найден: {filePath}");
		}
		try
		{
			using (Aes aesAlg = Aes.Create())
			{
				aesAlg.Key = _encryptionKey;
				aesAlg.IV = _encryptionIV;

				using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
				using (BufferedStream bs = new BufferedStream(fs))
				using (ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
				using (CryptoStream cs = new CryptoStream(bs, decryptor, CryptoStreamMode.Read))
				using (StreamReader sr = new StreamReader(cs))
				{
					return sr.ReadToEnd();
				}
			}
		}
		catch (CryptographicException ex)
		{
			throw new TodoApp.Exceptions.DecryptionException($"Ошибка расшифровки данных из файла {filePath}. Возможно, данные повреждены или неверный ключ/IV: {ex.Message}", ex);
		}
		catch (IOException ex)
		{
			throw new TodoApp.Exceptions.StorageException($"Ошибка доступа к файлу {filePath}: {ex.Message}", ex);
		}
		catch (Exception ex)
		{
			throw new TodoApp.Exceptions.StorageException($"Неизвестная ошибка при загрузке/расшифровке файла {filePath}: {ex.Message}", ex);
		}
	}
}