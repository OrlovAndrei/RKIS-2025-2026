// This file contains everything related to generating and reading paths, files
using System.Text;
using static Task.Const;
using static Task.WriteToConsole;
namespace Task;

public class OpenFile
{
	public string fullPath;
	public string NameFile { get; private set; }
	public enum EnumTypeFile
	{
		Standard,
		Config,
		Temporary,
		Index,
		IndexAndTemporary
	}

	public OpenFile(string nameFile, EnumTypeFile typeFile = EnumTypeFile.Standard)
	{
		NameFile = typeFile switch
		{
			EnumTypeFile.Standard => nameFile,
			EnumTypeFile.Config => nameFile + PrefConfigFile,
			EnumTypeFile.Temporary => nameFile + PrefTemporaryFile,
			EnumTypeFile.Index => nameFile + PrefIndex,
			EnumTypeFile.IndexAndTemporary => nameFile + PrefIndex + PrefTemporaryFile,
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
	public string TitleRowWriter(string titleRow) // Function for writing rows in tasks titles
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
			RainbowText("В мире произошло что то плохое", ConsoleColor.Red);
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
				string[] titleRow = (reader.ReadLine() ?? "").Split(SeparRows);
				if (titleRow.Length > positionInRow)
				{
					while ((line = reader.ReadLine()) != null)
					{
						string[] pathLine = line.Split(SeparRows);
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
			RainbowText("Разраб отдыхает, прошу понять", ConsoleColor.Red);
			RainbowText("^если что там ошибка чтения файла", ConsoleColor.Red);
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
			RainbowText("не найдено, что именно я тоже не знаю", ConsoleColor.Red);
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
				OpenFile tempFile = new(NameFile, EnumTypeFile.IndexAndTemporary);
				using (StreamReader reader = new StreamReader(fullPath, Encoding.UTF8))
				{
					string? line;
					int numLine = 1;
					string titleRow = reader.ReadLine() ?? "";
					tempFile.WriteFile(titleRow, false);
					while ((line = reader.ReadLine()) != null)
					{
						List<string> partLine = line.Split(SeparRows).ToList();
						partLine[0] = numLine.ToString();
						FormatterRows newLine = new FormatterRows(NameFile, FormatterRows.TypeEnum.old);
						newLine.AddInRow(partLine.ToArray());
						tempFile.WriteFile(newLine.GetRow());
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
				RainbowText("не найдено, что именно я тоже не знаю", ConsoleColor.Red);
			}
		}
		else { RainbowText($"Файл под названием {NameFile}, не найден.", ConsoleColor.Red); }
		return "";
	}
	public void AddRowInFile(string[] titleRowArray, string[] dataTypeRowArray, bool message = true, bool boolIs = false)
	{
		try
		{
			FormatterRows titleRow = new(NameFile, FormatterRows.TypeEnum.title);
			string row = Input.RowOnTitleAndConfig(titleRowArray, dataTypeRowArray, NameFile);
			titleRow.AddInRow(titleRowArray);
			TitleRowWriter(titleRow.GetRow());
			WriteFile(row);
			if (message) { RainbowText("Задание успешно записано", ConsoleColor.Green); }
		}
		catch (Exception)
		{
			RainbowText("Произошла ошибка при записи файла", ConsoleColor.Red);
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
				OpenFile tempFile = new(NameFile, EnumTypeFile.Temporary);
				using (StreamReader reader = new StreamReader(fullPath, Encoding.UTF8))
				{
					string? line;
					string titleRow = reader.ReadLine() ?? "";
					if (indexColumn < titleRow.Split(SeparRows).Length)
					{
						tempFile.WriteFile(titleRow, false);
						while ((line = reader.ReadLine()) != null)
						{
							List<string> partLine = line.Split(SeparRows).ToList();
							if ((counter < numberOfIterations || maxCounter) && partLine[indexColumn] == requiredData)
							{
								partLine[indexColumnWrite] = modifiedData;
								FormatterRows newLine = new FormatterRows(NameFile, FormatterRows.TypeEnum.old);
								newLine.AddInRow(partLine.ToArray());
								tempFile.WriteFile(newLine.GetRow());
								++counter;
							}
							else { tempFile.WriteFile(line); }
						}
						RainbowText($"Было перезаписано '{counter}' строк", ConsoleColor.Green);
					}
					else { RainbowText($"Index слишком большой максимальное значение.", ConsoleColor.Red); }
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
				OpenFile tempFile = new(NameFile, EnumTypeFile.Temporary);
				using (StreamReader reader = new StreamReader(fullPath, Encoding.UTF8))
				{
					string? line;
					string titleRow = reader.ReadLine() ?? "";
					if (indexColumn < titleRow.Split(SeparRows).Length)
					{
						tempFile.WriteFile(titleRow, false);
						while ((line = reader.ReadLine()) != null)
						{
							List<string> partLine = line.Split(SeparRows).ToList();
							if (counter < numberOfIterations && partLine[indexColumn] == requiredData)
							{
								++counter;
							}
							else { tempFile.WriteFile(line); }
						}
						RainbowText($"Было перезаписано '{counter}' строк", ConsoleColor.Green);
					}
					else { RainbowText($"Index слишком большой максимальное значение.", ConsoleColor.Red); }
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
				RainbowText("не найдено, что именно я тоже не знаю", ConsoleColor.Red);
				Console.WriteLine(ex);
			}
		}
		else { RainbowText($"Файл под названием {NameFile}, не найден.", ConsoleColor.Red); }
	}
	public void GetAllLine(out string[] configFile)
	{
		configFile = File.Exists(fullPath) 
		? File.ReadAllText(fullPath).Split("\n") 
		: StringArrayNull;
	}
}