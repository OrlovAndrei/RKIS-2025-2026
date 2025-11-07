// This file contains everything related to generating and reading paths, files
using System.Text;
using static Task.Const;
using static Task.WriteToConsole;
namespace Task;

public enum TypeFile
{
	Standard,
	Config,
	Temporary,
	Index,
	IndexAndTemporary
}

public static class Task
{
	private static readonly CSVLine title = new("Numbering", "Bool", "Task Name", "Description", "Creation date", "DeadLine");
	private static readonly CSVLine dataType = new("counter", "false", "s", "ls", "ndt", "dt");
	private static readonly string FileName = "Tasks";
	public static readonly CSVFile Pattern = new(FileName, title, dataType);
}
public static class Profile
{
	private static readonly CSVLine title = new("Numbering", "Bool", "Profile Name", "Creation date", "Birth");
	private static readonly CSVLine dataType = new("counter", "false", "s", "ndt", "d");
	private static readonly string FileName = "Profiles";
	public static readonly CSVFile Pattern = new(FileName, title, dataType);
}
public static class Log
{
    private static readonly CSVLine title = new("Numbering", "Bool", "ActiveProfile", "Date And Time", "Command", "Options", "TextCommand");
	private static readonly CSVLine dataType = new("counter", "lb", "prof", "ndt", "command", "option", "textline");
	private static readonly string FileName = "Log";
	public static readonly CSVFile Pattern = new(FileName, title, dataType);
}

public class OpenFile
{
	public string fullPath;
	public string NameFile { get; private set; }

	public OpenFile(string nameFile, TypeFile typeFile = TypeFile.Standard)
	{
		NameFile = typeFile switch
		{
			TypeFile.Standard => nameFile,
			TypeFile.Config => nameFile + PrefConfigFile,
			TypeFile.Temporary => nameFile + PrefTemporaryFile,
			TypeFile.Index => nameFile + PrefIndex,
			TypeFile.IndexAndTemporary => nameFile + PrefIndex + PrefTemporaryFile,
			_ => nameFile
		};
		fullPath = CreatePath();
	}
	public string CreatePath(string extension = "csv")
	{
		string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DirectoryName);
		DirectoryInfo? directory = new(path); // Инициализируем объект класса для создания директории
		if (!directory.Exists) Directory.CreateDirectory(path); // Если директория не существует, то мы её создаём по пути fullPath
		return Path.Combine(path, $"{NameFile}.{extension}");
	}
	public static string GetPathToZhopa()
	{
		string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
		string[] huis = baseDirectory.Split("/");
		List<string> huiBolshoy = new();
		foreach (string indexHui in huis)
		{
			if (indexHui != "bin")
			{
				huiBolshoy.Add(indexHui);
			}
			else
			{
				break;
			}
		}
		return string.Join('/', huiBolshoy);
	}
	public static string GaySex(string fileName)
	{
		string huiBolshoy = GetPathToZhopa();
		string sex = Path.Join(huiBolshoy, fileName);
		return File.ReadAllText(sex);
	}
	public static void AddFirst(CSVFile fileCSV, bool overwrite = false)
	{
		if (!File.Exists(fileCSV.ConfigFile.fullPath) || overwrite)
			using (FileStream fs = new(fileCSV.ConfigFile.fullPath, FileMode.OpenOrCreate,
			FileAccess.Write, FileShare.Read))
			{
				fileCSV.ConfigFile.WriteFile(fileCSV.Title!, false);
				fileCSV.ConfigFile.WriteFile(fileCSV.DataType!);
			}
	}
	public void TitleRowWriter(CSVLine titleRow) // Function for writing rows in tasks titles
	{
		/*Создаёт титульное оформление в файле 
            при условии что это новый файл*/
		OpenFile configFile = new(NameFile, TypeFile.Config);
		if (!File.Exists(configFile.fullPath))
			using (FileStream fs = new(configFile.fullPath, FileMode.CreateNew,
			FileAccess.Write, FileShare.Read))
			{
				WriteFile(titleRow);
			}
	}
	public void WriteFile(CSVLine dataFile, bool noRewrite = true)
	{
		/*Запись строки в конец файла при условии что 
            аргумент "noRewrite" равен true, а иначе файл будет перезаписан*/
		try
		{
			using (StreamWriter sw = new(fullPath, noRewrite, Encoding.UTF8))
			{
				sw.WriteLine(dataFile.Get());
			}
		}
		catch (Exception)
		{
			RainbowText("В мире произошло что то плохое", ConsoleColor.Red);
		}
	}
	public void WriteFile(List<CSVLine> dataFiles, bool noRewrite = true)
	{
		/*Запись строки в конец файла при условии что 
            аргумент "noRewrite" равен true, а иначе файл будет перезаписан*/
		foreach (var dataFile in dataFiles)
		{
			WriteFile(dataFile);
		}
	}
	private CSVLine GetTitleLine() => GetFromDataType(0);
	private CSVLine GetDataType() => GetFromDataType(1);
	private CSVLine GetFromDataType(int position = 1, TypeFile typeFile = TypeFile.Standard)
	{
		CSVLine line = new();
		OpenFile file = new(NameFile, TypeFile.Config);
		if (File.Exists(file.fullPath))
		{
			line = file.GetLinePositionRow(position);
		}
		return line;
	}
	public CSVFile GetLinePositionInRow(string dataFile, int positionInRow, int count = 1)
	{
		/*Возвращает строку если ее элемент по заданной позиции 
            соответствует введенным нами данным*/
		CSVFile fileCSV = new(NameFile);
		try
		{
			using (StreamReader reader = new(fullPath, Encoding.UTF8))
			{
				CSVLine line;
				CSVLine titleRow = GetTitleLine();
				int counter = 0;
				if (titleRow.GetLength() > positionInRow)
				{
					while ((line = new(reader.ReadLine())).GetLength() != 0)
					{
						if (counter < count && line.Items[positionInRow] == dataFile)
						{
							fileCSV.AddObject(line);
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
			RainbowText("Разраб отдыхает, прошу понять", ConsoleColor.Red);
			RainbowText("^если что там ошибка чтения файла", ConsoleColor.Red);
		}
		return fileCSV;
	}
	public CSVLine GetLinePositionRow(int positionRow)
	{
		/*Возвращает строку если ее элемент по заданной позиции 
            соответствует введенным нами данным*/
		CSVLine lineCSV = new();
		if (File.Exists(fullPath))
		{
			using (StreamReader reader = new StreamReader(fullPath, Encoding.UTF8))
			{
				int numLine = 0;
				while ((lineCSV = new(reader.ReadLine())).GetLength() != 0)
				{
					if (numLine == positionRow)
					{
						break;
					}
					++numLine;
				}
			}
		}
		else
		{
			RainbowText($"Файл '{NameFile}' не найден");
		}
		return lineCSV!;
	}
	public int GetLengthFile()
	{
		int numLine = 1;
		try
		{
			if (File.Exists(fullPath))
			{
				using (StreamReader reader = new StreamReader(fullPath, Encoding.UTF8))
				{
					while (reader.ReadLine() is not null)
					{
						++numLine;
					}
				}
			}
		}
		catch (Exception)
		{
			throw;
		}
		return numLine;
	}
	public void ReIndexFile(bool message = false)
	{
		if (File.Exists(fullPath))
		{
			try
			{
				OpenFile tempFile = new(NameFile, TypeFile.IndexAndTemporary);
				CSVLine line;
				using (StreamReader reader = new StreamReader(fullPath, Encoding.UTF8))
				{
					int numLine = 1;
					while ((line = new(reader.ReadLine())).GetLength() != 0)
					{
						line.Items[0] = numLine.ToString();
						tempFile.WriteFile(line);
						++numLine;
					}
				}
				using (StreamReader reader = new StreamReader(tempFile.fullPath, Encoding.UTF8))
				{
					WriteFile(new CSVLine(reader.ReadLine() ?? ""), false);
					while ((line = new(reader.ReadLine())).GetLength() != 0)
					{
						WriteFile(line);
					}
				}
				File.Delete(tempFile.fullPath);
			}
			catch (Exception)
			{
				RainbowText("не найдено, что именно я тоже не знаю", ConsoleColor.Red);
			}
		}
		else { RainbowText($"Файл под названием {NameFile}, не найден.", ConsoleColor.Red); }
	}
	public void AddRowInFile(CSVFile fileCSV, bool message = true)
	{
		try
		{
			AddFirst(fileCSV);
			Input.RowOnTitleAndConfig(fileCSV, out CSVLine outLine);
			fileCSV.File.WriteFile(outLine);
			if (message) { RainbowText("Задание успешно записано", ConsoleColor.Green); }
		}
		catch (Exception)
		{
			throw;
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
				OpenFile tempFile = new(NameFile, TypeFile.Temporary);
				CSVLine line;
				using (StreamReader reader = new StreamReader(fullPath, Encoding.UTF8))
				{
					while ((line = new(reader.ReadLine())).GetLength() != 0)
					{
						if ((counter < numberOfIterations || maxCounter) && line.Items[indexColumn] == requiredData)
						{
							line.Items[indexColumnWrite] = modifiedData;
							tempFile.WriteFile(line);
							++counter;
						}
						else { tempFile.WriteFile(line); }
					}
					RainbowText($"Было перезаписано '{counter}' строк", ConsoleColor.Green);

				}
				using (StreamReader reader = new StreamReader(tempFile.fullPath, Encoding.UTF8))
				{
					WriteFile(new CSVLine(reader.ReadLine() ?? ""), false);
					while ((line = new(reader.ReadLine())).GetLength() != 0)
					{
						WriteFile(line);
					}
				}
				File.Delete(tempFile.fullPath);
			}
			catch (Exception)
			{
				RainbowText("не найдено, что именно я тоже не знаю", ConsoleColor.Red);
			}
		}
		else { RainbowText($"Файл под названием {NameFile}, не найден.", ConsoleColor.Red); }
	}
	public void ClearRow(string requiredData, int indexColumn, int numberOfIterations = 1)
	{
		int counter = 0;
		if (File.Exists(fullPath))
		{
			try
			{
				OpenFile tempFile = new(NameFile, TypeFile.Temporary);
				CSVLine line;
				using (StreamReader reader = new StreamReader(fullPath, Encoding.UTF8))
				{
					while ((line = new(reader.ReadLine())).GetLength() != 0)
					{
						if (counter < numberOfIterations && line.Items[indexColumn] == requiredData)
						{
							++counter;
						}
						else { tempFile.WriteFile(line); }
					}
					RainbowText($"Было перезаписано '{counter}' строк", ConsoleColor.Green);
				}
				using (StreamReader reader = new StreamReader(tempFile.fullPath, Encoding.UTF8))
				{
					WriteFile(new CSVLine(reader.ReadLine() ?? ""), false);
					while ((line = new(reader.ReadLine())).GetLength() != 0)
					{
						WriteFile(line);
					}
				}
				File.Delete(tempFile.fullPath);
				ReIndexFile();
			}
			catch (Exception ex)
			{
				RainbowText("не найдено, что именно я тоже не знаю", ConsoleColor.Red);
				Console.WriteLine(ex);
			}
		}
		else { RainbowText($"Файл под названием {NameFile}, не найден.", ConsoleColor.Red); }
	}
	public void GetAllLine(out List<string> configFile)
	{
		configFile = File.Exists(fullPath)
		? File.ReadAllText(fullPath).Split("\n").ToList<string>()
		: [];
	}
}