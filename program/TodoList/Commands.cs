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
		OpenFile file = new(TaskName);
		file.AddRowInFile(TaskTitle, TaskTypeData);
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
		OpenFile file = new(TaskName);
		file.AddRowInFile(TaskTitle, TaskTypeData);
		Print(file.GetLineFilePositionRow(file.GetLengthFile() - 1), file.GetLineFilePositionRow(0));
		return 1;
	}
	public static int AddConfUserData(string? fileName = "")
	{
		IfNull("Введите название для файла с данными: ", ref fileName);
		OpenFile configFile = new(fileName!, OpenFile.EnumTypeFile.Config);
		string fullPathConfig = configFile.CreatePath();
		bool askFile = true;
		string searchLastTitle = "";
		string searchLastDataType = "";
		if (File.Exists(fullPathConfig))
		{
			configFile.GetAllLine(out string[] rowsConfig);
			searchLastTitle = rowsConfig[0];
			searchLastDataType = rowsConfig[1];
			Print(searchLastDataType, searchLastTitle);
			askFile = Bool($"Вы точно уверены, что хотите перезаписать конфигурацию?");
		}
		if (askFile)
		{
			FormatterRows titleRow = new(fileName!, FormatterRows.TypeEnum.title),
			dataTypeRow = new(fileName!, FormatterRows.TypeEnum.dataType);
			while (true)
			{
				string intermediateResultString =
					String("Введите название пункта титульного оформления файла: ");
				if (intermediateResultString == "exit" &&
				titleRow.GetLengthRow() != 0) break;
				else if (intermediateResultString == "exit")
					RainbowText("В титульном оформлении должен быть хотя бы один пункт: ", ConsoleColor.Red);
				else if (titleRow.Row!.Contains(intermediateResultString))
				{
					RainbowText("Объекты титульного оформления не должны повторятся", ConsoleColor.Red);
				}
				else titleRow.AddInRow(intermediateResultString);
			}
			foreach (string title in titleRow.Row!)
			{
				if (titleRow.GetFirstObject().Contains(title)) continue;
				else dataTypeRow.AddInRow(DataType($"Введите тип данных для строки {title}: "));
			}
			configFile.TitleRowWriter(titleRow.GetRow());
			string lastTitleRow = configFile.GetLineFilePositionRow(0);
			string lastDataTypeRow = configFile.GetLineFilePositionRow(1);
			bool ask = true;
			if ((lastTitleRow != titleRow.GetRow() && lastTitleRow.Length != 0) ||
			(lastDataTypeRow != dataTypeRow.GetRow() && lastDataTypeRow.Length != 0))
			{
				Console.WriteLine("Нынешний: ");
				Print(dataTypeRow.GetRow(), titleRow.GetRow());
				Console.WriteLine("Прошлый: ");
				Print(lastDataTypeRow, lastTitleRow);
				ask = Bool("Заменить?: ");
			}
			else
			{
				Print(dataTypeRow.GetRow(), titleRow.GetRow());
			}
			if (ask)
			{
				configFile.WriteFile(titleRow.GetRow(), false);
				configFile.WriteFile(dataTypeRow.GetRow());
			}
			return 1;
		}
		else
		{
			RainbowText("Будет использована конфигурация: ", ConsoleColor.Yellow);
			Print(searchLastDataType, searchLastTitle);
			return 0;
		}
	}
	public static int AddUserData(string? fileName = "")
	{
		IfNull("Введите название для файла с данными: ", ref fileName);
		OpenFile fileConf = new(fileName!, OpenFile.EnumTypeFile.Config);
		OpenFile file = new(fileName!);
		if (File.Exists(fileConf.fullPath))
		{
			string titleRow = fileConf.GetLineFilePositionRow(0);
			string dataTypeRow = fileConf.GetLineFilePositionRow(1);
			string[] titleRowArray = titleRow.Split(SeparRows);
			string[] dataTypeRowArray = dataTypeRow.Split(SeparRows);
			string row = RowOnTitleAndConfig(titleRowArray, dataTypeRowArray, fileName!);
			file.TitleRowWriter(titleRow);
			string testTitleRow = file.GetLineFilePositionRow(0);
			if (testTitleRow != titleRow)
				file.WriteFile(titleRow, false);
			file.WriteFile(row);
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
			OpenFile file = new(fileName!);
			if (File.Exists(file.fullPath))
			{
				file.WriteFile(file.GetLineFilePositionRow(0), false);
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
	public static int WriteColumn(OpenFile file, int start = 0)
	{
		string[] partsTitleRow = file.GetLineFilePositionRow(0).Split(SeparRows);
		var res = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title("Выберите в каком [green]столбце[/] проводить поиски?")
				.PageSize(10)
				// .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
				.AddChoices(partsTitleRow[start..]));
		for (int i = start; i < partsTitleRow.Length; ++i)
		{
			if (res == partsTitleRow[i])
			{
				return i;
			}
		}
		return start;
	}
	public static int ClearRow(string? fileName, string? requiredData = "")
	{
		IfNull("Введите название файла: ", ref fileName);
		OpenFile file = new(fileName!);
		if (File.Exists(file.fullPath))
		{
			IfNull("Поиск: ", ref requiredData);
			file.ClearRow(requiredData!, WriteColumn(file));
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
		OpenFile file = new(fileName!);
		if (File.Exists(file.fullPath))
		{
			IfNull("Поиск: ", ref requiredData);
			string modifiedData = String($"Введите на что {requiredData} поменять: ");
			file.EditingRow(requiredData!, modifiedData, WriteColumn(file, 2)); // 2 означает что мы пропускаем из вывода numbering и Bool
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
		OpenFile file = new(fileName!);
		if (File.Exists(file.fullPath))
		{
			IfNull("Поиск: ", ref requiredData);
			string modifiedData = Bool($"Введите на что {requiredData} поменять(true/false): ").ToString();
			file.EditingRow(requiredData!, modifiedData, WriteColumn(file), indexColumnWrite: 1); // 1 в indexColumnWrite это bool строка таска
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
		OpenFile file = new(fileName!);
		if (File.Exists(file.fullPath))
		{
			IfNull("Поиск: ", ref text);
			Console.WriteLine(string.Join("\n", file.GetLineFileDataOnPositionInRow(text!, WriteColumn(file))));
			return 1;
		}
		else
		{
			RainbowText(fileName + ": такого файла не существует.", ConsoleColor.Red);
			return 0;
		}
	}
	public static int Print(string row, string title)
	{
		var table = new Table();
		if (title.Length != 0 && row.Length != 0)
		{
			string[] titleArray = title.Split(SeparRows);
			string[] rowArray = row.Split(SeparRows);
			table.AddColumns(titleArray[0]);
			table.AddColumns(rowArray[0]);
			for (int i = 1; i < titleArray.Length; i++)
			{
				table.AddRow(titleArray[i], rowArray[i]);
			}
		}
		AnsiConsole.Write(table);
		return 1;
	}
	public static int PrintActivePriFile()
	{
		OpenFile file = new(ProfileName);
		Print(SearchActiveProfile(), file.GetLineFilePositionRow(0));
		return 1;
	}
	public static int PrintAll(string? fileName = "")
	{
		IfNull("Ведите название файла: ", ref fileName);
		OpenFile file = new(fileName!);
		try
		{
			using (StreamReader reader = new StreamReader(file.fullPath, Encoding.UTF8))
			{
				string? line;
				string[] titleRowArray = (reader.ReadLine() ?? "").Split(SeparRows);
				var table = new Table();
				table.Title(fileName!);
				foreach (string titleRow in titleRowArray)
				{
					table.AddColumns(titleRow);
				}
				while ((line = reader.ReadLine()) != null)
				{
					table.AddRow(line.Split(SeparRows));
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
	public static int PrintSpecific(string columnName, string? fileName = "")
	{
		IfNull("Ведите название файла: ", ref fileName);
		OpenFile file = new(fileName!);
		try
		{
			using (StreamReader reader = new StreamReader(file.fullPath, Encoding.UTF8))
			{
				string? line;
				string[] titleRowArray = (reader.ReadLine() ?? "").Split(SeparRows);
				var table = new Table();
				table.Title(fileName!);
				int columnId = 0;
				List<int> neededColumnsId = new List<int>();
				foreach (string titleRow in titleRowArray)
				{
					if (titleRow == columnName ||
					titleRow == TitleNumbingObject ||
					titleRow == TaskTitle[0] ||
					(columnName == TaskTitle[3] && titleRow == TaskTitle[2]))
					{
						neededColumnsId.Add(columnId++);
						table.AddColumns(titleRow);
					}
					else
					{
						columnId++;
						continue;
					}
				}
				while ((line = reader.ReadLine()) != null)
				{
					string[] lineArray = line.Split(SeparRows);
					List<string> neededRow = new();
					for (int i = 0; i < lineArray.Length; ++i)
					{
						if (neededColumnsId.Contains(i))
						{
							neededRow.Add(lineArray[i]);
						}
					}
					table.AddRow(neededRow.ToArray());
				}
				AnsiConsole.Write(table);
				return 1;
			}
		}
		catch (Exception e)
		{
			RainbowText($"Произошла ошибка при чтении файла {e.Message}", ConsoleColor.Red);
			return 0;
		}
	}
	public static int AddProfile()
	{
		OpenFile profileFile = new(ProfileName);
		profileFile.AddRowInFile(ProfileTitle, ProfileDataType);
		return 1;
	}
	public static int AddFirstProfile()
	{
		OpenFile profile = new(ProfileName);
		FormatterRows titleRow = new(ProfileName, FormatterRows.TypeEnum.title);
		titleRow.AddInRow(ProfileTitle);
		profile.TitleRowWriter(titleRow.GetRow());
		if (profile.GetLengthFile() == 1)
		{
			FormatterRows rowAdmin = new(ProfileName);
			rowAdmin.AddInRow(AdminProfile);
			profile.WriteFile(rowAdmin.GetRow());
			profile.EditingRow(false.ToString(), true.ToString(), 1);
			return 1;
		}
		return 0;
	}
	public static string SearchActiveProfile()
	{
		OpenFile profile = new(ProfileName);
		string[] activeProfile = profile.GetLineFileDataOnPositionInRow(true.ToString(), 1);
		if (activeProfile.Length == 0 || activeProfile.Length > 1)
		{
			UseActiveProfile();
		}
		return profile.GetLineFileDataOnPositionInRow(true.ToString(), 1)[0];
	}
	public static int UseActiveProfile()
	{
		OpenFile profile = new(ProfileName);
		if (File.Exists(profile.fullPath))
		{
			profile.EditingRow(true.ToString(), false.ToString(), 1, -1);
			string requiredData = Input.String("Поиск: ");
			string modifiedData = true.ToString();
			profile.EditingRow(requiredData, modifiedData, WriteColumn(profile), indexColumnWrite: 1); // 1 в indexColumnWrite это bool строка таска
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
				OpenFile logFile = new(LogName);
				logFile.AddRowInFile(LogTitle, LogDataType, false);
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
