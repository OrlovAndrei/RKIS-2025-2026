// This is the main file, it contains cruical components of the program - PoneMaurice
using System.Text;
using Spectre.Console;
using static Task.Const;
using static Task.Input;
using static Task.WriteToConsole;
namespace Task;

public class Commands
{
	public static int AddTask()
	{
		/*программа запрашивает у пользователя все необходимые ей данные
            и записывает их в файл tasks.csv с нужным форматированием*/
		OpenFile file = Task.Pattern.File;
		file.AddRowInFile(Task.Pattern);
		return 1;
	}
	public static int MultiAddTask()
	{
		int num = 0;
		while (true)
		{
			AddTask();
			num++;
			if (!Bool($"{num} задание добавлено, желаете продолжить?"))
			{
				break;
			}
		}
		return 1;
	}
	public static int AddTaskAndPrint()
	{
		/*программа запрашивает у пользователя все необходимые ей данные
	        и записывает их в файл tasks.csv с нужным форматированием 
	        после чего выводит сообщение о добавлении данных дублируя их 
	        пользователю для проверки*/
		OpenFile file = Task.Pattern.File;
		file.AddRowInFile(Task.Pattern);
		Print(file.GetLinePositionRow(file.GetLengthFile() - 1), file.GetLinePositionRow(0));
		return 1;
	}
	public static int AddConfUserData(string? fileName = "")
	{
		IfNull("Введите название для файла с данными: ", ref fileName);
		CSVFile fileCSV = new(fileName!);
		CSVLine lastTitleRow = new(), lastDataTypeRow = new();
		bool askFile = true;
		if (File.Exists(fileCSV.ConfigFile.fullPath))
		{
			lastTitleRow = fileCSV.Title!;
			lastDataTypeRow = fileCSV.DataType!;
			Print(lastDataTypeRow, lastTitleRow);
			askFile = Bool($"Вы точно уверены, что хотите перезаписать конфигурацию?");
		}
		if (askFile)
		{
			FormatterRows titleRow = new(fileName!, TypeRow.title),
			dataTypeRow = new(fileName!, TypeRow.dataType);
			while (true)
			{
				string intermediateResultString =
					String("Введите название пункта титульного оформления файла: ");
				if (intermediateResultString == "exit" &&
				titleRow.GetLength() != 0) break;
				else if (intermediateResultString == "exit")
					RainbowText("В титульном оформлении должен быть хотя бы один пункт: ", ConsoleColor.Red);
				else if (titleRow.Items!.Contains(intermediateResultString))
				{
					RainbowText("Объекты титульного оформления не должны повторятся", ConsoleColor.Red);
				}
				else titleRow.AddInRow(intermediateResultString);
			}
			foreach (string title in titleRow.Items!)
			{
				if (titleRow.GetFirstObject().Contains(title)) continue;
				else dataTypeRow.AddInRow(DataType($"Введите тип данных для строки {title}: "));
			}
			OpenFile.AddFirst(fileCSV);
			bool ask = true;
			if ((lastTitleRow.Items != titleRow.Items && lastTitleRow.GetLength() != 0) ||
			(lastDataTypeRow.Items != dataTypeRow.Items && lastDataTypeRow.GetLength() != 0))
			{
				Console.WriteLine("Нынешний: ");
				Print(dataTypeRow, titleRow);
				Console.WriteLine("Прошлый: ");
				Print(lastDataTypeRow, lastTitleRow);
				ask = Bool("Заменить?: ");
			}
			else
			{
				Print(dataTypeRow, titleRow);
			}
			if (ask)
			{
				fileCSV.DataType = dataTypeRow;
				fileCSV.Title = titleRow;
				OpenFile.AddFirst(fileCSV, true);
			}
			return 1;
		}
		else
		{
			RainbowText("Будет использована конфигурация: ", ConsoleColor.Yellow);
			Print(fileCSV.DataType!, fileCSV.Title!);
			return 0;
		}
	}
	public static int AddUserData(string? fileName = "")
	{
		IfNull("Введите название для файла с данными: ", ref fileName);
		CSVFile fileCSV = new(fileName!);
		if (File.Exists(fileCSV.ConfigFile.fullPath))
		{
			RowOnTitleAndConfig(fileCSV, out CSVLine outLine);
			fileCSV.File.WriteFile(outLine);
			return 1;
		}
		else
		{
			RainbowText($"Сначала создайте конфигурацию или проверьте правильность написания названия => '{fileName}'", ConsoleColor.Red);
			return 0;
		}
	}
	public static int ClearAllFile(string? fileName = "")
	{
		IfNull("Введите название файла: ", ref fileName);
		if (Bool($"Вы уверены что хотите очистить весь файл {fileName}?"))
		{
			CSVFile fileCSV = new(fileName!);
			if (File.Exists(fileCSV.File.fullPath))
			{
				File.Delete(fileCSV.File.fullPath);
				return 1;
			}
			else
			{
				RainbowText(fileName + ": такого файла не существует.", ConsoleColor.Red);
				return 0;
			}
		}
		else
		{
			System.Console.WriteLine("Буде внимательны");
			return 0;
		}
	}
	public static int WriteColumn(string fileName, int start = 0)
	{
		CSVFile fileCSV = new(fileName);
		string[] option = fileCSV.Title!.Items[start..].ToArray()!;
		var res = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title("Выберите в каком [green]столбце[/] проводить поиски?")
				.PageSize(10)
				// .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
				.AddChoices(option));
		for (int i = start; i < fileCSV.Title.GetLength(); ++i)
		{
			if (res == fileCSV.Title.Items[i])
			{
				return i;
			}
		}
		return start;
	}
	public static int ClearRow(string? fileName, string? requiredData = "")
	{
		IfNull("Введите название файла: ", ref fileName);
		CSVFile fileCSV = new(fileName!);
		if (File.Exists(fileCSV.File.fullPath))
		{
			IfNull("Поиск: ", ref requiredData);
			fileCSV.File.ClearRow(requiredData!, WriteColumn(fileCSV.File.NameFile));
			return 1;
		}
		else
		{
			RainbowText("Такого файла не существует: ", ConsoleColor.Yellow);
			return 0;
		}
	}
	public static int EditRow(string? fileName, string? requiredData = "")
	{
		IfNull("Введите название файла: ", ref fileName);
		CSVFile fileCSV = new(fileName!);
		if (File.Exists(fileCSV.File.fullPath))
		{
			IfNull("Поиск: ", ref requiredData);
			string modifiedData = String($"Введите на что {requiredData} поменять: ");
			fileCSV.File.EditingRow(requiredData!, modifiedData, WriteColumn(fileCSV.File.NameFile, 2)); // 2 означает что мы пропускаем из вывода numbering и Bool
			return 1;
		}
		else
		{
			RainbowText("Такого файла не существует: ", ConsoleColor.Yellow);
			return 0;
		}
	}
	public static int EditBoolRow(string? fileName, string? requiredData = "")
	{
		IfNull("Введите название файла: ", ref fileName);
		CSVFile fileCSV = new(fileName!);
		if (File.Exists(fileCSV.File.fullPath))
		{
			IfNull("Поиск: ", ref requiredData);
			Key($"Введите на что {requiredData} поменять(true/false): ",
				out ConsoleKey key, ConsoleKey.T, ConsoleKey.F);
			string? modifiedData = key switch
			{
				ConsoleKey.T => true.ToString(),
				ConsoleKey.F => false.ToString(),
				_ => null
			};
			fileCSV.File.EditingRow(requiredData!, modifiedData!, WriteColumn(fileCSV.File.NameFile), indexColumnWrite: 1); // 1 в indexColumnWrite это bool строка таска
			return 1;
		}
		else
		{
			RainbowText("Такого файла не существует: ", ConsoleColor.Yellow);
			return 0;
		}
	}
	public static int SearchPartData(string? fileName = "", string? text = "")
	{
		IfNull("Ведите название файла: ", ref fileName);
		CSVFile fileCSV = new(fileName!);
		if (File.Exists(fileCSV.File.fullPath))
		{
			IfNull("Поиск: ", ref text);
			CSVFile searchFileCSV = fileCSV.File.GetLinePositionInRow(text!, WriteColumn(fileCSV.File.NameFile));
			var table = new Table();
			table.Title(fileName!);
			foreach (string? titleRow in searchFileCSV.Title!.Items)
			{
				table.AddColumns(titleRow!);
			}
			foreach (var line in searchFileCSV.Objects)
			{
				table.AddRow(line.Items!.ToArray<string>());
			}
			AnsiConsole.Write(table);
			return 1;
		}
		else
		{
			RainbowText(fileName + ": такого файла не существует.", ConsoleColor.Red);
			return 0;
		}
	}
	public static int Print(CSVLine row, CSVLine title)
	{
		var table = new Table();
		if (title.GetLength() != 0 && row.GetLength() != 0)
		{
			table.AddColumns(title.Items[0]!);
			table.AddColumns(row.Items[0]!);
			for (int i = 1; i < title.GetLength(); i++)
			{
				table.AddRow(title.Items[i]!, row.Items[i]!);
			}
		}
		AnsiConsole.Write(table);
		return 1;
	}
	public static int PrintActivePriFile()
	{
		Print(SearchActiveProfile(), Profile.Pattern.Title!);
		return 1;
	}
	public static int PrintAll(string? fileName = "")
	{
		IfNull("Ведите название файла: ", ref fileName);
		CSVFile fileCSV = new(fileName!);
		try
		{
			using (StreamReader reader = new StreamReader(fileCSV.File.fullPath, Encoding.UTF8))
			{
				CSVLine line;
				var table = new Table();
				table.Title(fileName!);
				foreach (string? titleRow in fileCSV.Title!.Items)
				{
					table.AddColumns(titleRow!);
				}
				while ((line = new(reader.ReadLine())).GetLength() != 0)
				{
					table.AddRow(line.Items!.ToArray<string>());
				}
				AnsiConsole.Write(table);
				return 1;
			}
		}
		catch (Exception)
		{
			RainbowText("Произошла ошибка при чтении файла", ConsoleColor.Red);
			return 0;
		}
	}
	public static int AddProfile()
	{
		OpenFile profileFile = Profile.Pattern.File;
		profileFile.AddRowInFile(Profile.Pattern);
		return 1;
	}
	public static int AddFirstProfile()
	{
		OpenFile.AddFirst(Profile.Pattern);
		OpenFile profile = Profile.Pattern.File;
		if (profile.GetLengthFile() == 1)
		{
			AddProfile();
			profile.EditingRow(false.ToString(), true.ToString(), 1);
			return 1;
		}
		return 0;
	}
	public static CSVLine SearchActiveProfile()
	{
		OpenFile profile = new(ProfileName);
		List<CSVLine> activeProfile = profile.GetLinePositionInRow(true.ToString(), 1).Objects;
		if (activeProfile.Count != 1)
		{
			UseActiveProfile();
		}
		return profile.GetLinePositionInRow(true.ToString(), 1).Objects[0];
	}
	public static int UseActiveProfile()
	{
		if (File.Exists(Profile.Pattern.File.fullPath))
		{
			Profile.Pattern.File.EditingRow(true.ToString(), false.ToString(), 1, -1);
			string requiredData = Input.String("Поиск: ");
			string modifiedData = true.ToString();
			Profile.Pattern.File.EditingRow(requiredData, modifiedData,
			WriteColumn(Profile.Pattern.File.NameFile), indexColumnWrite: 1); // 1 в indexColumnWrite это bool строка таска
			SearchActiveProfile();
			return 1;
		}
		else
		{
			AddFirstProfile();
			return 1;
		}
	}
	public static int AddLog()
	{
		try
		{
			if (Survey.CommandLineGlobal != null)
			{
				Log.Pattern.File.AddRowInFile(Log.Pattern, false);
				return 1;
			}
		}
		catch (Exception)
		{
			RainbowText("Произошла ошибка при записи файла", ConsoleColor.Red);
			return 0;
		}
		return 0;
	}
	public static int FixingIndexing(string? fileName)
	{
		IfNull("Введите название файла: ", ref fileName);
		OpenFile file = new(fileName!);
		file.ReIndexFile(true);
		return 1;
	}
	public static int ConsoleClear()
	{
		Console.Clear();
		return 1;
	}
	public static int WriteCaption()
	{
		/*спрашивает и выводит текст субтитров созданный 
	        методом CompText*/
		Text(
			"За работу ответственны:",
			"\tШевченко Э. - README, исходный код;",
			"\tТитов М. - github, некоторый аспекты исходного кода, help команды;"
		);
		return 1;
	}
}
