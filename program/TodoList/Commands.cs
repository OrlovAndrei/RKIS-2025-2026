// This is the main file, it contains cruical components of the program - PoneMaurice
using System.Text;
using Spectre.Console;
namespace Task;

public class Commands
{
	public static void AddTask()
	{
		/*программа запрашивает у пользователя все необходимые ей данные
            и записывает их в файл tasks.csv с нужным форматированием*/
		OpenFile.AddRowInFile(Const.TaskName, Const.TaskTitle, Const.TaskTypeData);
	}
	public static void MultiAddTask()
	{
		int num = 0;
		while (true)
		{
			OpenFile.AddRowInFile(Const.TaskName, Const.TaskTitle, Const.TaskTypeData);
			num++;
			if (!Input.Bool($"{num} задание добавлено, желаете продолжить?"))
			{
				break;
			}
		}
	}
	public static void AddTaskAndPrint()
	{
		/*программа запрашивает у пользователя все необходимые ей данные
            и записывает их в файл tasks.csv с нужным форматированием 
            после чего выводит сообщение о добавлении данных дублируя их 
            пользователю для проверки*/
		OpenFile file = new(Const.TaskName);
		OpenFile.AddRowInFile(Const.TaskName, Const.TaskTitle, Const.TaskTypeData);
		Print(file.GetLineFilePositionRow(file.GetLengthFile() - 1), file.GetLineFilePositionRow(0));
	}
	public static void AddConfUserData(string fileName = "")
	{
		Input.IfNull("Введите название для файла с данными: ", ref fileName);
		fileName = fileName + Const.PrefConfigFile;
		OpenFile file = new(fileName);
		string fullPathConfig = file.CreatePath();
		bool askFile = true;
		string searchLastTitle = "";
		string searchLastDataType = "";
		if (File.Exists(fullPathConfig))
		{
			file.GetConfigFile(out string[] rowsConfig);
			searchLastTitle = rowsConfig[0];
			searchLastDataType = rowsConfig[1];
			Print(searchLastDataType, searchLastTitle);
			askFile = Input.Bool($"Вы точно уверены, что хотите перезаписать конфигурацию?");
		}
		if (askFile)
		{
			FormatterRows titleRow = new(fileName, FormatterRows.TypeEnum.title), dataTypeRow = new(fileName, FormatterRows.TypeEnum.dataType);
			while (true)
			{
				string intermediateResultString =
					Input.String("Введите название пункта титульного оформления файла: ");
				if (intermediateResultString == "exit" &&
				titleRow.GetLengthRow() != 0) break;
				else if (intermediateResultString == "exit")
					WriteToConsole.RainbowText("В титульном оформлении должен быть хотя бы один пункт: ", ConsoleColor.Red);
				else if (titleRow.Row.ToString().Split(Const.SeparRows).Contains(intermediateResultString))
				{
					WriteToConsole.RainbowText("Объекты титульного оформления не должны повторятся", ConsoleColor.Red);
				}
				else titleRow.AddInRow(intermediateResultString);
			}
			string[] titleRowArray = titleRow.Row.ToString().Split(Const.SeparRows);
			foreach (string title in titleRowArray)
			{
				if (title == Const.TitleNumbingObject ||
				title == Const.TitleBoolObject) continue;
				else dataTypeRow.AddInRow(Input.DataType($"Введите тип данных для строки {title}: "));
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
				ask = Input.Bool("Заменить?");
			}
			if (ask)
			{
				file.WriteFile(titleRow.Row.ToString(), false);
				file.WriteFile(dataTypeRow.Row.ToString());
			}
		}
		else
		{
			WriteToConsole.RainbowText("Будет использована конфигурация: ", ConsoleColor.Yellow);
			Print(searchLastDataType, searchLastTitle);
		}
	}
	public static void AddUserData(string fileName = "")
	{
		Input.IfNull("Введите название для файла с данными: ", ref fileName);
		OpenFile fileConf = new(fileName + Const.PrefConfigFile);
		OpenFile file = new(fileName);
		string fullPathConfig = fileConf.CreatePathToConfig();
		if (File.Exists(fullPathConfig))
		{
			string titleRow = fileConf.GetLineFilePositionRow(0);
			string dataTypeRow = fileConf.GetLineFilePositionRow(1);
			string[] titleRowArray = titleRow.Split(Const.SeparRows);
			string[] dataTypeRowArray = dataTypeRow.Split(Const.SeparRows);
			string row = Input.RowOnTitleAndConfig(titleRowArray, dataTypeRowArray, fileName);
			file.TitleRowWriter(titleRow);
			string testTitleRow = file.GetLineFilePositionRow(0);
			if (testTitleRow != titleRow)
				file.WriteFile(titleRow, false);
			file.WriteFile(row);
		}
		else WriteToConsole.RainbowText($"Сначала создайте конфигурацию или проверьте правильность написания названия => '{fileName}'", ConsoleColor.Red);
	}
	public static void ClearAllFile(string fileName = "")
	{
		Input.IfNull("Введите название файла: ", ref fileName);
		if (Input.Bool($"Вы уверены что хотите очистить весь файл {fileName}?"))
		{
			OpenFile file = new(fileName);
			if (File.Exists(file.fullPath))
			{
				file.WriteFile(file.GetLineFilePositionRow(0), false);
			}
			else WriteToConsole.RainbowText(fileName + ": такого файла не существует.", ConsoleColor.Red);
		}
		else System.Console.WriteLine("Буде внимательны");
	}
	public static int WriteColumn(OpenFile file, int start = 0)
	{
		string[] partsTitleRow = file.GetLineFilePositionRow(0).Split(Const.SeparRows);
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
	public static void ClearRow(string fileName, string requiredData = "")
	{
		Input.IfNull("Введите название файла: ", ref fileName);
		OpenFile file = new(fileName);
		if (File.Exists(file.fullPath))
		{
			Input.IfNull("Поиск: ", ref requiredData);
			file.ClearRow(requiredData, WriteColumn(file));
		}
		else { WriteToConsole.RainbowText("Такого файла не существует: ", ConsoleColor.Yellow); }
	}
	public static void EditRow(string fileName, string requiredData = "")
	{
		Input.IfNull("Введите название файла: ", ref fileName);
		OpenFile file = new(fileName);
		if (File.Exists(file.fullPath))
		{
			Input.IfNull("Поиск: ", ref requiredData);
			string modifiedData = Input.String($"Введите на что {requiredData} поменять: ");
			file.EditingRow(requiredData, modifiedData, WriteColumn(file, 2)); // 2 означает что мы пропускаем из вывода numbering и Bool
		}
		else { WriteToConsole.RainbowText("Такого файла не существует: ", ConsoleColor.Yellow); }
	}
	public static void EditBoolRow(string fileName, string requiredData = "")
	{
		Input.IfNull("Введите название файла: ", ref fileName);
		OpenFile file = new(fileName);
		if (File.Exists(file.fullPath))
		{
			Input.IfNull("Поиск: ", ref requiredData);
			string modifiedData = Input.Bool($"Введите на что {requiredData} поменять(true/false): ").ToString();
			file.EditingRow(requiredData, modifiedData, WriteColumn(file), indexColumnWrite: 1); // 1 в indexColumnWrite это bool строка таска
		}
		else { WriteToConsole.RainbowText("Такого файла не существует: ", ConsoleColor.Yellow); }
	}
	public static void SearchPartData(string fileName = "", string text = "")
	{
		Input.IfNull("Ведите название файла: ", ref fileName);
		OpenFile file = new(fileName);
		if (File.Exists(file.fullPath))
		{
			Input.IfNull("Поиск: ", ref text);
			Console.WriteLine(string.Join("\n", file.GetLineFileDataOnPositionInRow(text, WriteColumn(file))));
		}
		else WriteToConsole.RainbowText(fileName + ": такого файла не существует.", ConsoleColor.Red);
	}
	public static void Print(string row, string title)
	{
		var table = new Table();
		if (title.Length != 0 && row.Length != 0)
		{
			string[] titleArray = title.Split(Const.SeparRows);
			string[] rowArray = row.Split(Const.SeparRows);
			table.AddColumns(titleArray[0]);
			table.AddColumns(rowArray[0]);
			for (int i = 1; i < titleArray.Length; i++)
			{
				table.AddRow(titleArray[i],rowArray[i]);
			}
		}
		AnsiConsole.Write(table);
    }
	public static void PrintAll(string fileName = "")
	{
		Input.IfNull("Ведите название файла: ", ref fileName);
		OpenFile file = new(fileName);
		try
		{
			using (StreamReader reader = new StreamReader(file.fullPath, Encoding.UTF8))
			{
				string? line;
				string[] titleRowArray = (reader.ReadLine() ?? "").Split(Const.SeparRows);
				var table = new Table();
				table.Title(fileName);
				foreach (string titleRow in titleRowArray)
				{
					table.AddColumns(titleRow);
				}
				while ((line = reader.ReadLine()) != null)
				{
					table.AddRow(line.Split(Const.SeparRows));
				}
				AnsiConsole.Write(table);
			}
		}
		catch (Exception)
		{
			WriteToConsole.RainbowText("Произошла ошибка при чтении файла", ConsoleColor.Red);
		}
	}
	public static void AddProfile()
	{
		OpenFile.AddRowInFile(Const.ProfileName, Const.ProfileTitle, Const.ProfileDataType);
	}
	public static void AddFirstProfile()
	{
		OpenFile profile = new(Const.ProfileName);
		FormatterRows titleRow = new(Const.ProfileName, FormatterRows.TypeEnum.title);
		titleRow.AddInRow(Const.ProfileTitle);
		profile.TitleRowWriter(titleRow.Row.ToString());
		if (profile.GetLengthFile() == 1)
		{
			FormatterRows rowAdmin = new(Const.ProfileName);
			rowAdmin.AddInRow(Const.AdminProfile);
			profile.WriteFile(rowAdmin.Row.ToString());
			profile.EditingRow(false.ToString(), true.ToString(), 1);
		}
	}
	public static string SearchActiveProfile()
	{
		OpenFile profile = new(Const.ProfileName);
		string[] activeProfile = profile.GetLineFileDataOnPositionInRow(true.ToString(), 1);
		if (activeProfile.Length == 0 || activeProfile.Length > 1)
		{
			UseActiveProfile();
		}
		return profile.GetLineFileDataOnPositionInRow(true.ToString(), 1)[0];
	}
	public static void UseActiveProfile()
	{
		OpenFile profile = new(Const.ProfileName);
		if (File.Exists(profile.fullPath))
		{
			profile.EditingRow(true.ToString(), false.ToString(), 1, -1);
			string requiredData = "";
			Input.IfNull("Поиск: ", ref requiredData);
			string modifiedData = true.ToString();
			profile.EditingRow(requiredData, modifiedData, WriteColumn(profile), indexColumnWrite: 1); // 1 в indexColumnWrite это bool строка таска
		}
		else { AddFirstProfile(); }
	}
	public static void AddLog()
	{
		try
		{
			if (Survey.commandLineGlobal != null)
			{
				OpenFile.AddRowInFile(Const.LogName, Const.LogTitle, Const.LogDataType, false);
			}
		}
		catch (Exception)
		{
			WriteToConsole.RainbowText("Произошла ошибка при записи файла", ConsoleColor.Red);
		}
	}
	public static void FixingIndexing(string fileName)
	{
		Input.IfNull("Введите название файла: ", ref fileName);
		OpenFile file = new(fileName);
		file.ReIndexFile(true);
	}
	public static void WriteCaption()
	{
		/*спрашивает и выводит текст субтитров созданный 
            методом CompText*/
		WriteToConsole.Text(
			"За работу ответственны:",
			"\tШевченко Э. - README, исходный код;",
			"\tТитов М. - github, некоторый аспекты исходного кода, help команды;"
		);
	}
	public static void ProfileHelp()
	{
		WriteToConsole.Text(
			"- `profile --help` — помощь",
			"- `profile --add` — добавить профиль",
			"- `profile --change` — сменить активный профиль",
			"- `profile --index` — переиндексация профилей",
			"- `profile` — показать активный профиль"
		);
	}
	public static void Help()
	{
		WriteToConsole.Text(
			"- `add` — добавление данных, задач или профилей",
			"- `profile` — работа с профилями",
			"- `print` — вывод информации",
			"- `search` — поиск по данным",
			"- `clear` — очистка данных",
			"- `edit` — редактирование данных",
			"- `help` — выводит общую справку по всем командам",
			"- `exit` — завершает выполнение программы"
		);
	}
	public static void AddHelp()
	{
		WriteToConsole.Text(
			"- `add --help` — помощь по добавлению",
			"- `add --task` — добавить новую задачу",
			"- `add --multi --task` — добавить несколько задач сразу",
			"- `add --task --print` — добавить задачу и сразу вывести её",
			"- `add --config <имя>` — добавить конфигурацию",
			"- `add --profile` — добавить профиль",
			"- `add <текст>` — добавить пользовательские данные"
		);
	}
	public static void PrintHelp()
	{
		WriteToConsole.Text(
			"- `print --help` — помощь",
			"- `print --task` — вывести все задачи",
			"- `print --config <имя>` — вывести конфигурацию",
			"- `print --profile` — вывести профили",
			"- `print --captions` — вывести заголовки",
			"- `print <имя>` — вывести данные по имени"
		);
	}
	public static void SearchHelp()
	{
		WriteToConsole.Text(
			"- `search --help` — помощь",
			"- `search --task <текст>` — поиск по задачам",
			"- `search --profile <текст>` — поиск по профилям",
			"- `search --numbering` — (в разработке)",
			"- `search <текст>` — общий поиск"
		);
	}
	public static void ClearHelp()
	{
		WriteToConsole.Text(
			"- `clear --help` — помощь",
			"- `clear --task <имя>` — удалить задачу",
			"- `clear --task --all` — очистить все задачи",
			"- `clear --profile <имя>` — удалить профиль",
			"- `clear --profile --all` — очистить все профили",
			"- `clear --console` — очистить консоль",
			"- `clear --all <текст>` — очистить все пользовательские данные"
		);
	}
	public static void EditHelp()
	{
		WriteToConsole.Text(
		   "- `edit --help` — помощь",
			"- `edit --task <имя>` — изменить задачу",
			"- `edit --task --index` — переиндексация задач",
			"- `edit --task --bool` — изменить главное логическое поле задачи",
			"- `edit --bool` — изменить главное логическое поле в данных",
			"- `edit --index` — переиндексация",
			"- `edit <имя>` — редактировать по имени"
		);
	}
}
