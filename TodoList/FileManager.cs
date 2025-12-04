using System.Text;

namespace TodoList;

public class FileManager
{
	public static void EnsureDataDirectory(string dirPath)
	{
		if (!Directory.Exists(dirPath))
		{
			Directory.CreateDirectory(dirPath);
			Console.WriteLine($"Создана директория: {dirPath}");
		}
	}

	public static void SaveProfile(Profile profile, string filePath)
	{
		try
		{
			string profileData = $"{profile.FirstName}|{profile.LastName}|{profile.BirthYear}";
			File.WriteAllText(filePath, profileData, Encoding.UTF8);
			Console.WriteLine("Профиль сохранен");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Ошибка сохранения профиля: {ex.Message}");
		}
	}

	public static Profile LoadProfile(string filePath)
	{
		try
		{
			if (!File.Exists(filePath))
			{
				Console.WriteLine("Файл профиля не найден");
				return null;
			}
			string line = File.ReadAllText(filePath, Encoding.UTF8);
			if (!string.IsNullOrEmpty(line))
			{
				string[] parts = line.Split('|');
				if (parts.Length == 3)
				{
					string firstName = parts[0];
					string lastName = parts[1];
					if (int.TryParse(parts[2], out int birthYear))
					{
						return new Profile(firstName, lastName, birthYear);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Ошибка загрузки профиля: {ex.Message}");
		}
		return null;
	}
}