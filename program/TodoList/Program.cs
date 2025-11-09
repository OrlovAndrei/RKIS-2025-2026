// This is the main file, it contains cruical components of the program - PoneMaurice
using System.CommandLine;
using Task;
namespace scl;

public class Program
{
	public static int Main(string[] args)
	{
		
		RootCommand rootCommand = new("Приложение для работы с CSV файлами.");
		rootCommand.SetAction(parseResult => RunProgram());

		Option<int> limitArgument = new("--limit")
		{
			Description = "Определяет сколько будет совершено циклов.",
			DefaultValueFactory = parseResult => -1
		};

		Option<bool> config = new("--config", "-c")
		{
			Description = "Работа с конфигурацией."
		};

		Option<string> fileName = new("--name", "-n")
		{
			Description = "Название файла."
		};

		Option<bool> prin = new("--print", "-p")
		{
			Description = "Показать."
		};

		Option<bool> multi = new("--multi", "-m")
		{
			Description = "Зацикленное создание заданий."
		};

		Option<bool> change = new("--change", "-c")
		{
			Description = "Смена профиля."
		};
		Option<bool> task = new("--task", "-t")
		{
			Description = "Работа с файлами заданий"
		};
		Option<bool> profileOption = new("--profile", "-p")
		{
			Description = "Работа с профилями."
		};
		Option<bool> log = new("--log", "lp")
		{
			Description = "Работа с логами."
		};

		List<Option> printAllOptions =
		[
			task,
			profileOption,
			log,
			config,
			fileName
		];

		Command printAll = new("print-all", "Вывести весь файл на экран");
		foreach (var option in printAllOptions)
		{
			printAll.Options.Add(option);
		}
		printAll.SetAction(parseResult => PrintAll(
			parseResult.GetValue(task),
			parseResult.GetValue(profileOption),
			parseResult.GetValue(log),
			parseResult.GetValue(config),
			parseResult.GetValue(fileName)
			));

		Command addIn = new("add", "Добавление значений в файл.");
		addIn.Options.Add(fileName);
		addIn.Options.Add(config);
		addIn.SetAction(parseResult => AddInFile(
			parseResult.GetValue(fileName),
			parseResult.GetValue(config)
			));

		Command addTaskAndPrint = new("task-and-print", "Добавление нового задания и вывод.");
		addTaskAndPrint.Options.Add(prin);
		addTaskAndPrint.SetAction(parseResult => AddTask(
			parseResult.GetValue(prin)
			));
		
		Command addTask = new("task", "Добавление нового задания.");
		addTask.Options.Add(multi);
		addTask.SetAction(parseResult => AddTask(
			parseResult.GetValue(multi)
			));

		Command addProfile = new("profile", "Добавление нового профиля.");
		addProfile.SetAction(parseResult => AddProfile());

		addIn.Subcommands.Add(addTask);
		addIn.Subcommands.Add(addTaskAndPrint);
		addIn.Subcommands.Add(addProfile);

		Command profile = new("profile", "Работа с профилями");
		profile.Options.Add(change);
		profile.SetAction(parseResult => Profile(
			parseResult.GetValue(change)
			));
		
		Command runProgram = new("run", "Запускает полноценную программу.");
		runProgram.Options.Add(limitArgument);
		runProgram.SetAction(parseResult => RunProgram(
			parseResult.GetValue(limitArgument)
			));

		rootCommand.Subcommands.Add(runProgram);
		rootCommand.Subcommands.Add(addIn);
		rootCommand.Subcommands.Add(profile);
		rootCommand.Subcommands.Add(printAll);

		return rootCommand.Parse(args).Invoke();
	}
	
	internal static void RunProgram(int limitCycle = -1)
	{
		int cycle = 0;
		Console.Clear();
		do
		{
			if (cycle == 0)
			{
				Commands.AddFirstProfile();
			}
			else if (cycle == limitCycle)
			{
				Environment.Exit(0);
			}
			Survey.GlobalCommand(Const.PrintInTerminal);
			Commands.AddLog();
			++cycle;
		}
		while (true);
	}
	internal static void AddInFile(string? nameFile, bool addConfig)
	{
		if (addConfig)
		{
			Commands.AddConfUserData(nameFile);
		}
		else
		{
			Commands.AddUserData(nameFile);
		}
	}
	internal static void AddProfile()
	{
		Commands.AddProfile();
	}
	internal static void AddTask(bool multi = false, bool print = false)
	{
		if (multi)
		{
			Commands.MultiAddTask();
		}
		else if (print)
		{
			Commands.AddTaskAndPrint();
		}
		else
		{
			Commands.AddTask();
		}
	}
	internal static void Profile(bool change)
	{
		if (change)
		{
			Commands.UseActiveProfile();
		}
		else
		{
			Commands.PrintActivePriFile();
		}
	}
	internal static void PrintAll(bool task = false, bool profile = false, bool log = false,
		bool config = false, string? nameFile = null)
	{
		if (task)
		{
			Commands.PrintAll(Task.Task.Pattern.File.NameFile);
		}
		else if (profile)
		{
			Commands.PrintAll(Task.Profile.Pattern.File.NameFile);
		}
		else if (log)
		{
			Commands.PrintAll(Task.Log.Pattern.File.NameFile);
		}
		else if (config && nameFile is not null)
		{
			CSVFile fileCSV = new(nameFile);
			Commands.PrintAll(fileCSV.ConfigFile.NameFile);
		}
		else
		{
			Commands.PrintAll(nameFile);
		}
    }
}
