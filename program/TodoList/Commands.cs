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
		OpenFile.AddRowInFile(TaskName, TaskTitle, TaskTypeData);
		return 1;
	}
	public static int MultiAddTask()
	{
		int num = 0;
		while (true)
		{
			OpenFile.AddRowInFile(TaskName, TaskTitle, TaskTypeData);
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
		OpenFile.AddRowInFile(TaskName, TaskTitle, TaskTypeData);
		Print(file.GetLineFilePositionRow(file.GetLengthFile() - 1), file.GetLineFilePositionRow(0));
		return 1;
	}
	public static int AddConfUserData(string fileName = "")
	{
		IfNull("Введите название для файла с данными: ", ref fileName);
		fileName = fileName + PrefConfigFile;
		OpenFile file = new(fileName);
		string fullPathConfig = file.CreatePath();
		bool askFile = true;
		string searchLastTitle = "";
		string searchLastDataType = "";
		if (File.Exists(fullPathConfig))
		{
			file.GetAllLine(out string[] rowsConfig);
			searchLastTitle = rowsConfig[0];
			searchLastDataType = rowsConfig[1];
			Print(searchLastDataType, searchLastTitle);
			askFile = Bool($"Вы точно уверены, что хотите перезаписать конфигурацию?");
		}
		if (askFile)
		{
			FormatterRows titleRow = new(fileName, FormatterRows.TypeEnum.title), dataTypeRow = new(fileName, FormatterRows.TypeEnum.dataType);
			while (true)
			{
				string intermediateResultString =
					String("Введите название пункта титульного оформления файла: ");
				if (intermediateResultString == "exit" &&
				titleRow.GetLengthRow() != 0) break;
				else if (intermediateResultString == "exit")
					RainbowText("В титульном оформлении должен быть хотя бы один пункт: ", ConsoleColor.Red);
				else if (titleRow.Row.ToString().Split(SeparRows).Contains(intermediateResultString))
				{
					RainbowText("Объекты титульного оформления не должны повторятся", ConsoleColor.Red);
				}
				else titleRow.AddInRow(intermediateResultString);
			}
			string[] titleRowArray = titleRow.Row.ToString().Split(SeparRows);
			foreach (string title in titleRowArray)
			{
				if (title == TitleNumbingObject ||
				title == TitleBoolObject) continue;
				else dataTypeRow.AddInRow(DataType($"Введите тип данных для строки {title}: "));
			}
			file.TitleRowWriter(titleRow.Row.ToString());
			Print(titleRow.Row.ToString(), dataTypeRow.Row.ToString());
			string lastTitleRow = file.GetLineFilePositionRow(0);
			string lastDataTypeRow = file.GetLineFilePositionRow(1);
			bool ask = true;
			if ((lastTitleRow != titleRow.Row.ToString() && lastTitleRow.Length != 0) ||
			(lastDataTypeRow != dataTypeRow.Row.ToString() && lastDataTypeRow.Length != 0))
			{
				Console.WriteLine("Нынешний: ");
				Print(titleRow.Row.ToString(), dataTypeRow.Row.ToString());
				Console.WriteLine("Прошлый: ");
				Print(lastTitleRow, lastDataTypeRow);
				ask = Bool("Заменить?");
			}
			if (ask)
			{
				file.WriteFile(titleRow.Row.ToString(), false);
				file.WriteFile(dataTypeRow.Row.ToString());
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
	public static int AddUserData(string fileName = "")
	{
		IfNull("Введите название для файла с данными: ", ref fileName);
		OpenFile fileConf = new(fileName + PrefConfigFile);
		OpenFile file = new(fileName);
		if (File.Exists(fileConf.fullPath))
		{
			string titleRow = fileConf.GetLineFilePositionRow(0);
			string dataTypeRow = fileConf.GetLineFilePositionRow(1);
			string[] titleRowArray = titleRow.Split(SeparRows);
			string[] dataTypeRowArray = dataTypeRow.Split(SeparRows);
			string row = RowOnTitleAndConfig(titleRowArray, dataTypeRowArray, fileName);
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
	public static int ClearAllFile(string fileName = "")
	{
		IfNull("Введите название файла: ", ref fileName);
		if (Bool($"Вы уверены что хотите очистить весь файл {fileName}?"))
		{
			OpenFile file = new(fileName);
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
	public static int ClearRow(string fileName, string requiredData = "")
	{
		IfNull("Введите название файла: ", ref fileName);
		OpenFile file = new(fileName);
		if (File.Exists(file.fullPath))
		{
			IfNull("Поиск: ", ref requiredData);
			file.ClearRow(requiredData, WriteColumn(file));
			return 1;
		}
		else
		{
			RainbowText("Такого файла не существует: ", ConsoleColor.Yellow);
			return 0;
		}
	}
	public static int EditRow(string fileName, string requiredData = "")
	{
		IfNull("Введите название файла: ", ref fileName);
		OpenFile file = new(fileName);
		if (File.Exists(file.fullPath))
		{
			IfNull("Поиск: ", ref requiredData);
			string modifiedData = String($"Введите на что {requiredData} поменять: ");
			file.EditingRow(requiredData, modifiedData, WriteColumn(file, 2)); // 2 означает что мы пропускаем из вывода numbering и Bool
			return 1;
		}
		else
		{
			RainbowText("Такого файла не существует: ", ConsoleColor.Yellow);
			return 0;
		}
	}
	public static int EditBoolRow(string fileName, string requiredData = "")
	{
		IfNull("Введите название файла: ", ref fileName);
		OpenFile file = new(fileName);
		if (File.Exists(file.fullPath))
		{
			IfNull("Поиск: ", ref requiredData);
			string modifiedData = Bool($"Введите на что {requiredData} поменять(true/false): ").ToString();
			file.EditingRow(requiredData, modifiedData, WriteColumn(file), indexColumnWrite: 1); // 1 в indexColumnWrite это bool строка таска
			return 1;
		}
		else
		{
			RainbowText("Такого файла не существует: ", ConsoleColor.Yellow);
			return 0;
		}
	}
	public static int SearchPartData(string fileName = "", string text = "")
	{
		IfNull("Ведите название файла: ", ref fileName);
		OpenFile file = new(fileName);
		if (File.Exists(file.fullPath))
		{
			IfNull("Поиск: ", ref text);
			Console.WriteLine(string.Join("\n", file.GetLineFileDataOnPositionInRow(text, WriteColumn(file))));
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
	public static int PrintAll(string fileName = "")
	{
		IfNull("Ведите название файла: ", ref fileName);
		OpenFile file = new(fileName);
		try
		{
			using (StreamReader reader = new StreamReader(file.fullPath, Encoding.UTF8))
			{
				string? line;
				string[] titleRowArray = (reader.ReadLine() ?? "").Split(SeparRows);
				var table = new Table();
				table.Title(fileName);
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
	public static int AddProfile()
	{
		OpenFile.AddRowInFile(ProfileName, ProfileTitle, ProfileDataType);
		return 1;
	}
	public static int AddFirstProfile()
	{
		OpenFile profile = new(ProfileName);
		FormatterRows titleRow = new(ProfileName, FormatterRows.TypeEnum.title);
		titleRow.AddInRow(ProfileTitle);
		profile.TitleRowWriter(titleRow.Row.ToString());
		if (profile.GetLengthFile() == 1)
		{
			FormatterRows rowAdmin = new(ProfileName);
			rowAdmin.AddInRow(AdminProfile);
			profile.WriteFile(rowAdmin.Row.ToString());
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
			string requiredData = "";
			IfNull("Поиск: ", ref requiredData);
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
			if (Survey.commandLineGlobal != null)
			{
				OpenFile.AddRowInFile(LogName, LogTitle, LogDataType, false);
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
	public static int FixingIndexing(string fileName)
	{
		IfNull("Введите название файла: ", ref fileName);
		OpenFile file = new(fileName);
		file.ReIndexFile(true);
		return 1;
	}
	public static int ConsoleClear()
    {
        RainbowText("CCCCCCClear", ConsoleColor.Magenta);
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
	public static int ProfileHelp()
	{
		Text(
			"- `profile --help` — помощь",
			"- `profile --add` — добавить профиль",
			"- `profile --change` — сменить активный профиль",
			"- `profile --index` — переиндексация профилей",
			"- `profile` — показать активный профиль"
		);
		return 1;
	}
	public static int Help()
	{
		Text(
			"- `add` — добавление данных, задач или профилей",
			"- `profile` — работа с профилями",
			"- `print` — вывод информации",
			"- `search` — поиск по данным",
			"- `clear` — очистка данных",
			"- `edit` — редактирование данных",
			"- `help` — выводит общую справку по всем командам",
			"- `exit` — завершает выполнение программы"
		);
		return 1;
	}
	public static int AddHelp()
	{
		Text(
			"- `add --help` — помощь по добавлению",
			"- `add --task` — добавить новую задачу",
			"- `add --multi --task` — добавить несколько задач сразу",
			"- `add --task --print` — добавить задачу и сразу вывести её",
			"- `add --config <имя>` — добавить конфигурацию",
			"- `add --profile` — добавить профиль",
			"- `add <текст>` — добавить пользовательские данные"
		);
		return 1;
	}
	public static int PrintHelp()
	{
		Text(
			"- `print --help` — помощь",
			"- `print --task` — вывести все задачи",
			"- `print --config <имя>` — вывести конфигурацию",
			"- `print --profile` — вывести профили",
			"- `print --captions` — вывести заголовки",
			"- `print <имя>` — вывести данные по имени"
		);
		return 1;
	}
	public static int SearchHelp()
	{
		Text(
			"- `search --help` — помощь",
			"- `search --task <текст>` — поиск по задачам",
			"- `search --profile <текст>` — поиск по профилям",
			"- `search --numbering` — (в разработке)",
			"- `search <текст>` — общий поиск"
		);
		return 1;
	}
	public static int ClearHelp()
	{
		Text(
			"- `clear --help` — помощь",
			"- `clear --task <имя>` — удалить задачу",
			"- `clear --task --all` — очистить все задачи",
			"- `clear --profile <имя>` — удалить профиль",
			"- `clear --profile --all` — очистить все профили",
			"- `clear --console` — очистить консоль",
			"- `clear --all <текст>` — очистить все пользовательские данные"
		);
		return 1;
	}
	public static int EditHelp()
	{
		Text(
		   "- `edit --help` — помощь",
			"- `edit --task <имя>` — изменить задачу",
			"- `edit --task --index` — переиндексация задач",
			"- `edit --task --bool` — изменить главное логическое поле задачи",
			"- `edit --bool` — изменить главное логическое поле в данных",
			"- `edit --index` — переиндексация",
			"- `edit <имя>` — редактировать по имени"
		);
		return 1;
	}
}
