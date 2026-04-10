using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using TodoList.Models;

namespace TodoApp.Commands
{
	public class FileManager : IDataStorage
	{
		private readonly string _baseDirectory;
		private readonly byte[] _key;
		private readonly byte[] _iv;

		public FileManager(string baseDirectory, byte[] key, byte[] iv)
		{
			_baseDirectory = baseDirectory;
			_key = key;
			_iv = iv;
			Directory.CreateDirectory(_baseDirectory);
		}

		public void SaveProfiles(IEnumerable<Profile> profiles)
		{
			string filePath = Path.Combine(_baseDirectory, "profiles.dat");
			WriteEncryptedFile(filePath, writer =>
			{
				foreach (var profile in profiles)
				{
					string line = $"{profile.Id};{EscapeCsvField(profile.Login)};{EscapeCsvField(profile.Password)};{EscapeCsvField(profile.FirstName)};{EscapeCsvField(profile.LastName)};{profile.BirthYear}";
					writer.WriteLine(line);
				}
			});
		}

		public IEnumerable<Profile> LoadProfiles()
		{
			string filePath = Path.Combine(_baseDirectory, "profiles.dat");
			if (!File.Exists(filePath))
				return new List<Profile>();

			var profiles = new List<Profile>();
			try
			{
				ReadEncryptedFile(filePath, reader =>
				{
					string line;
					while ((line = reader.ReadLine()) != null)
					{
						if (string.IsNullOrWhiteSpace(line))
							continue;

						var parts = ParseCsvLine(line);
						if (parts.Length < 6)
							throw new DataStorageException($"Некорректная строка профиля: {line}");

						var profile = new Profile
						{
							Id = Guid.Parse(parts[0]),
							Login = UnescapeCsvField(parts[1]),
							Password = UnescapeCsvField(parts[2]),
							FirstName = UnescapeCsvField(parts[3]),
							LastName = UnescapeCsvField(parts[4]),
							BirthYear = int.Parse(parts[5])
						};
						profiles.Add(profile);
					}
				});
			}
			catch (CryptographicException ex)
			{
				throw new DataStorageException("Ошибка расшифровки файла профилей. Возможно, неверный ключ или IV.", ex);
			}
			catch (Exception ex) when (ex is not DataStorageException)
			{
				throw new DataStorageException("Ошибка при загрузке профилей: " + ex.Message, ex);
			}
			return profiles;
		}

		public void SaveTodos(Guid userId, IEnumerable<TodoItem> todos)
		{
			string filePath = Path.Combine(_baseDirectory, $"todos_{userId:N}.dat");
			WriteEncryptedFile(filePath, writer =>
			{
				foreach (var todo in todos)
				{
					string text = todo.Text.Replace("\"", "\"\"");
					string line = $"\"{text}\";{todo.IsDone};{todo.CreationDate:yyyy-MM-ddTHH:mm:ss};{todo.Status}";
					writer.WriteLine(line);
				}
			});
		}

		public IEnumerable<TodoItem> LoadTodos(Guid userId)
		{
			string filePath = Path.Combine(_baseDirectory, $"todos_{userId:N}.dat");
			if (!File.Exists(filePath))
				return new List<TodoItem>();

			var items = new List<TodoItem>();
			try
			{
				ReadEncryptedFile(filePath, reader =>
				{
					string line;
					while ((line = reader.ReadLine()) != null)
					{
						if (string.IsNullOrWhiteSpace(line))
							continue;

						var parts = ParseCsvLine(line);
						if (parts.Length < 4)
							throw new DataStorageException($"Недостаточно полей в строке задачи: {line}");

						string text = parts[0];
						bool isDone = bool.Parse(parts[1]);
						DateTime creationDate = DateTime.ParseExact(parts[2], "yyyy-MM-ddTHH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
						TodoStatus status = Enum.Parse<TodoStatus>(parts[3]);

						items.Add(new TodoItem(text, isDone, creationDate, status));
					}
				});
			}
			catch (CryptographicException ex)
			{
				throw new DataStorageException($"Ошибка расшифровки файла задач пользователя {userId}.", ex);
			}
			catch (Exception ex) when (ex is not DataStorageException)
			{
				throw new DataStorageException($"Ошибка при загрузке задач пользователя {userId}: " + ex.Message, ex);
			}
			return items;
		}

		private void WriteEncryptedFile(string filePath, Action<StreamWriter> writeAction)
		{
			using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
			using (var bufferedStream = new BufferedStream(fileStream))
			using (var aes = Aes.Create())
			{
				aes.Key = _key;
				aes.IV = _iv;
				using (var cryptoStream = new CryptoStream(bufferedStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
				using (var writer = new StreamWriter(cryptoStream, Encoding.UTF8))
				{
					writeAction(writer);
					writer.Flush();
					cryptoStream.FlushFinalBlock();
				}
			}
		}

		private void ReadEncryptedFile(string filePath, Action<StreamReader> readAction)
		{
			using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
			using (var bufferedStream = new BufferedStream(fileStream))
			using (var aes = Aes.Create())
			{
				aes.Key = _key;
				aes.IV = _iv;
				using (var cryptoStream = new CryptoStream(bufferedStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
				using (var reader = new StreamReader(cryptoStream, Encoding.UTF8))
				{
					readAction(reader);
				}
			}
		}

		private static string[] ParseCsvLine(string line)
		{
			var parts = new List<string>();
			var current = new StringBuilder();
			bool inQuotes = false;

			for (int i = 0; i < line.Length; i++)
			{
				char c = line[i];
				if (c == '"')
				{
					if (i + 1 < line.Length && line[i + 1] == '"')
					{
						current.Append('"');
						i++;
					}
					else
					{
						inQuotes = !inQuotes;
					}
				}
				else if (c == ';' && !inQuotes)
				{
					parts.Add(current.ToString());
					current.Clear();
				}
				else
				{
					current.Append(c);
				}
			}
			parts.Add(current.ToString());
			return parts.ToArray();
		}

		private static string EscapeCsvField(string field)
		{
			if (field.Contains(';') || field.Contains('"') || field.Contains('\n'))
				return "\"" + field.Replace("\"", "\"\"") + "\"";
			return field;
		}

		private static string UnescapeCsvField(string field)
		{
			if (field.StartsWith("\"") && field.EndsWith("\""))
			{
				field = field.Substring(1, field.Length - 2);
				field = field.Replace("\"\"", "\"");
			}
			return field;
		}
	}
}