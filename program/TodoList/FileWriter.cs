// This file contains everything related to generating and reading paths, files - PoneMaurice
using System.Text;
namespace Task;

public class OpenFile
{
	public string fullPath;
	public string nameFile;
	public OpenFile(string fileName)
	{
		nameFile = fileName;
		fullPath = CreatePath();
	}
	public string CreatePath(string extension = "csv") // Function for creating file path - PoneMaurice
	{
		// /*Создание актульного пути под каждый нужный файл находящийся в деректории с конфигами*/
		// string dataPath = "/.config/RKIS-TodoList/"; // Расположение файла для UNIX и MacOSX
		// string winDataPath = "\\RKIS-todoList\\"; // Расположение файла для Win32NT
		// string fullPath;
		// string? homePath = (Environment.OSVersion.Platform == PlatformID.Unix || // Если платформа UNIX или MacOSX, то homePath = $HOME
		// 	   Environment.OSVersion.Platform == PlatformID.MacOSX)
		// 	   ? Environment.GetEnvironmentVariable("HOME")
		// 	   : Environment.ExpandEnvironmentVariables("%APPDATA%");   // Если платформа Win32NT, то homepath = \users\<username>\Documents 
		// if (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
		// 	fullPath = Path.Join(homePath, dataPath); // Если платформа UNIX или MacOSX, то мы соединяем homePath и dataPath
		// else
		// 	fullPath = Path.Join(homePath, winDataPath); // Если платформа Win32NT, то мы соединяем homePath и winDataPath
		// DirectoryInfo? directory = new DirectoryInfo(fullPath); // Инициализируем объект класса для создания директории
		// if (!directory.Exists) Directory.CreateDirectory(fullPath); // Если директория не существует, то мы её создаём по пути fullPath
		// fullPath = Path.Join(fullPath, nameFile + ".csv");
		// return fullPath;
		return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), $"{nameFile}.{extension}");
	}
	public static string GetPathToZhopa()
	{
		string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
		string[] huis = baseDirectory.Split("/");
		StringBuilder huiBolshoy = new();
		foreach (string indexHui in huis)
		{
			if (indexHui != "bin")
			{
				huiBolshoy.Append(indexHui + "/");
			}
			else
			{
				break;
			}
		}
		return huiBolshoy.ToString();
	}
	public static string StringFromFileInMainFolder(string fileName)
	{
		string huiBolshoy = OpenFile.GetPathToZhopa();
		string sex = Path.Join(huiBolshoy, fileName);
		return File.ReadAllText(sex);
	}
	public string TitleRowWriter(string titleRow) // Function for writing rows in tasks titles - PoneMaurice
	{
		/*Создаёт титульное оформление в файле 
            при условии что это новый файл*/
		string fullPath = CreatePath();
		if (!File.Exists(fullPath))
			using (var fs = new FileStream(fullPath, FileMode.CreateNew,
			FileAccess.Write, FileShare.Read))
			{
				using (var sw = new System.IO.StreamWriter(fs, Encoding.UTF8))
				{
					sw.WriteLine(titleRow);
				}
			}
		return fullPath;
	}
	public void WriteFile(string dataFile, bool noRewrite = true)
	{
		/*Запись строки в конец файла при условии что 
            аргумент "noRewrite" равен true, а иначе файл будет перезаписан*/
		try
		{
			using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fullPath, noRewrite, Encoding.UTF8))
			{
				sw.WriteLine(dataFile);
			}
		}
		catch (Exception)
		{
			WriteToConsole.RainbowText("В мире произошло что то плохое", ConsoleColor.Red);
		}
	}
	public string[] GetLineFileDataOnPositionInRow(string dataFile, int positionInRow, int count = 1)
	{
		/*Возвращает строку если ее элемент по заданной позиции 
            соответствует введенным нами данным*/
		List<string> searchLine = new();
		try
		{
			using (StreamReader reader = new StreamReader(fullPath, Encoding.UTF8))
			{
				string? line;
				int counter = 0;
				string[] titleRow = (reader.ReadLine() ?? "").Split(Const.SeparRows);
				if (titleRow.Length > positionInRow)
				{
					while ((line = reader.ReadLine()) != null)
					{
						string[] pathLine = line.Split(Const.SeparRows);
						if (counter < count && pathLine[positionInRow] == dataFile)
						{
							searchLine.Add(line);
							++counter;
						}
						else if (counter == count)
						{
							break;
						}
					}
				}
			}
		}
		catch (Exception)
		{
			WriteToConsole.RainbowText("Разраб отдыхает, прошу понять", ConsoleColor.Red);
			WriteToConsole.RainbowText("^если что там ошибка чтения файла", ConsoleColor.Red);
		}
		return searchLine.ToArray();
	}
	public string GetLineFilePositionRow(int positionRow)
	{
		/*Возвращает строку если ее элемент по заданной позиции 
            соответствует введенным нами данным*/
		try
		{
			using (StreamReader reader = new StreamReader(fullPath, Encoding.UTF8))
			{
				string? line;
				int numLine = 0;
				while ((line = reader.ReadLine()) != null)
				{
					if (numLine == positionRow)
					{
						return line;
					}
					++numLine;
				}
			}
		}
		catch (Exception)
		{
			WriteToConsole.RainbowText("не найдено, что именно я тоже не знаю", ConsoleColor.Red);
		}
		return "";
	}
	public string GetLineFileData(string dataFile)
	{
		/*перегрузка метода только без позиции*/
		try
		{
			using (StreamReader reader = new StreamReader(fullPath, Encoding.UTF8))
			{
				string? line;
				while ((line = reader.ReadLine()) != null)
				{
					if (line == dataFile)
					{
						return line;
					}
				}
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"{ex}\n");
		}
		return "";
	}
	public int GetLengthFile()
	{
		int numLine = 0;
		try
		{
			if (File.Exists(fullPath))
			{
				using (StreamReader reader = new StreamReader(fullPath, Encoding.UTF8))
				{
					string? line;
					while ((line = reader.ReadLine()) != null)
					{
						++numLine;
					}
				}
			}
			else return 1;
		}
		catch (Exception ex)
		{
			System.Console.WriteLine(ex.Message);
		}
		return numLine;
	}
	public string ReIndexFile(bool message = false)
	{
		if (File.Exists(fullPath))
		{
			try
			{
				OpenFile tempFile = new(nameFile + Const.PrefIndex + Const.PrefTemporaryFile);
				using (StreamReader reader = new StreamReader(fullPath, Encoding.UTF8))
				{
					string? line;
					int numLine = 1;
					string titleRow = reader.ReadLine() ?? "";
					tempFile.WriteFile(titleRow, false);
					while ((line = reader.ReadLine()) != null)
					{
						List<string> partLine = line.Split(Const.SeparRows).ToList();
						partLine[0] = numLine.ToString();
						FormatterRows newLine = new FormatterRows(nameFile, FormatterRows.TypeEnum.old);
						newLine.AddInRow(partLine.ToArray());
						tempFile.WriteFile(newLine.Row.ToString());
						++numLine;
					}
				}
				using (StreamReader reader = new StreamReader(tempFile.fullPath, Encoding.UTF8))
				{
					string? line;
					WriteFile(reader.ReadLine() ?? "", false);
					while ((line = reader.ReadLine()) != null)
					{
						WriteFile(line);
					}
				}
				File.Delete(tempFile.fullPath);
			}
			catch (Exception)
			{
				WriteToConsole.RainbowText("не найдено, что именно я тоже не знаю", ConsoleColor.Red);
			}
		}
		else { WriteToConsole.RainbowText($"Файл под названием {nameFile}, не найден.", ConsoleColor.Red); }
		return "";
	}
	public static void AddRowInFile(string nameFile, string[] titleRowArray, string[] dataTypeRowArray, bool message = true)
	{
		try
		{
			OpenFile file = new(nameFile);
			FormatterRows titleRow = new(nameFile, FormatterRows.TypeEnum.title);
			string row = Input.RowOnTitleAndConfig(titleRowArray, dataTypeRowArray, nameFile);
			titleRow.AddInRow(titleRowArray);
			file.TitleRowWriter(titleRow.Row.ToString());
			file.WriteFile(row);
			if (message) { WriteToConsole.RainbowText("Задание успешно записано", ConsoleColor.Green); }
		}
		catch (Exception)
		{
			WriteToConsole.RainbowText("Произошла ошибка при записи файла", ConsoleColor.Red);
		}
	}
	public void RecordingData(string[] rows)
	{
		string titleRow = rows[0];
		WriteFile(titleRow, false);
		for (int i = 1; i < rows.Count(); ++i) // i = 1 что бы не дублировалось титульное оформление
		{
			if (rows[i].Length != 0)
			{
				WriteFile(rows[i]);
			}
		}
	}
	public void EditingRow(string requiredData, string modifiedData, int indexColumn,
	int numberOfIterations = 1, int indexColumnWrite = -1)
	{
		if (indexColumnWrite == -1) { indexColumnWrite = indexColumn; }
		bool maxCounter = false;
		if (numberOfIterations == -1)
		{
			maxCounter = true;
		}
		int counter = 0;
		if (File.Exists(fullPath))
		{
			try
			{
				OpenFile tempFile = new(nameFile + Const.PrefTemporaryFile);
				using (StreamReader reader = new StreamReader(fullPath, Encoding.UTF8))
				{
					string? line;
					string titleRow = reader.ReadLine() ?? "";
					if (indexColumn < titleRow.Split(Const.SeparRows).Length)
					{
						tempFile.WriteFile(titleRow, false);
						while ((line = reader.ReadLine()) != null)
						{
							List<string> partLine = line.Split(Const.SeparRows).ToList();
							if ((counter < numberOfIterations || maxCounter) && partLine[indexColumn] == requiredData)
							{
								partLine[indexColumnWrite] = modifiedData;
								FormatterRows newLine = new FormatterRows(nameFile, FormatterRows.TypeEnum.old);
								newLine.AddInRow(partLine.ToArray());
								tempFile.WriteFile(newLine.Row.ToString());
								++counter;
							}
							else { tempFile.WriteFile(line); }
						}
						WriteToConsole.RainbowText($"Было перезаписано '{counter}' строк", ConsoleColor.Green);
					}
					else { WriteToConsole.RainbowText($"Index слишком большой максимальное значение.", ConsoleColor.Red); }
				}
				using (StreamReader reader = new StreamReader(tempFile.fullPath, Encoding.UTF8))
				{
					string? line;
					WriteFile(reader.ReadLine() ?? "", false);
					while ((line = reader.ReadLine()) != null)
					{
						WriteFile(line);
					}
				}
				File.Delete(tempFile.fullPath);
			}
			catch (Exception)
			{
				WriteToConsole.RainbowText("не найдено, что именно я тоже не знаю", ConsoleColor.Red);
			}
		}
		else { WriteToConsole.RainbowText($"Файл под названием {nameFile}, не найден.", ConsoleColor.Red); }
	}
	public void ClearRow(string requiredData, int indexColumn, int numberOfIterations = 1)
	{
		int counter = 0;
		if (File.Exists(fullPath))
		{
			try
			{
				OpenFile tempFile = new(nameFile + Const.PrefTemporaryFile);
				using (StreamReader reader = new StreamReader(fullPath, Encoding.UTF8))
				{
					string? line;
					string titleRow = reader.ReadLine() ?? "";
					if (indexColumn < titleRow.Split(Const.SeparRows).Length)
					{
						tempFile.WriteFile(titleRow, false);
						while ((line = reader.ReadLine()) != null)
						{
							List<string> partLine = line.Split(Const.SeparRows).ToList();
							if (counter < numberOfIterations && partLine[indexColumn] == requiredData)
							{
								++counter;
							}
							else { tempFile.WriteFile(line); }
						}
						WriteToConsole.RainbowText($"Было перезаписано '{counter}' строк", ConsoleColor.Green);
					}
					else { WriteToConsole.RainbowText($"Index слишком большой максимальное значение.", ConsoleColor.Red); }
				}
				using (StreamReader reader = new StreamReader(tempFile.fullPath, Encoding.UTF8))
				{
					string? line;
					WriteFile(reader.ReadLine() ?? "", false);
					while ((line = reader.ReadLine()) != null)
					{
						WriteFile(line);
					}
				}
				File.Delete(tempFile.fullPath);
				ReIndexFile();
			}
			catch (Exception ex)
			{
				WriteToConsole.RainbowText("не найдено, что именно я тоже не знаю", ConsoleColor.Red);
				System.Console.WriteLine(ex);
			}
		}
		else { WriteToConsole.RainbowText($"Файл под названием {nameFile}, не найден.", ConsoleColor.Red); }
	}
	public void GetAllLine(out string[] configFile)
	{
		configFile = File.Exists(fullPath) 
		? File.ReadAllText(fullPath).Split("\n") 
		: Const.StringArrayNull;
	}
}