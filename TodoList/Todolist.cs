using System.Diagnostics;

public class Program 
{
    private enum ProgramWorkMode
    {
        ProcessingCommands,
        AddingTask,
    }

    private static ProgramWorkMode programMode;

    struct UserData
    {
        public string firstName, lastName, birthYearString;
        public int currentYear, age, birthYear;
    }

    private static string[] todos;
    private static bool[] statuses;
    private static DateTime[] dates;

    private static int todosCount, todosLen;
    private static UserData userData = new UserData();
    private static bool isProgramRunning = true;
    private static int todosStartLen = 2;


    private const string CommandAddName = "add";
    private const string CommandProfileName = "profile";
    private const string CommandViewName = "view";
    private const string CommandExitName = "exit";
    private const string CommandHelpName = "help";
    private const string CommandDoneName = "done";
    private const string CommandDeleteName = "delete";
    private const string CommandUpdateName = "update";
    private const string CommandReadName = "read";

    private const string CommandEndName = "!end";

    private static string[] CommandAddMultilineFlags = new string[]
    {
        "--multiline",
        "-m"
    };
    private static string addingMultilineTask;

	private static string[] CommandViewIndexFlags = new string[]
	{
		"--index", "-i"
	};
	private static string[] CommandViewStatusFlags = new string[]
	{
		"--status", "-s"
	};
	private static string[] CommandViewUpdateFlags = new string[]
	{
		"--update-date", "-d"
	};
	private static string[] CommandViewAllFlags = new string[]
	{
		"--all", "-a"
	};

	private const int MaxTaskDisplayTextLen = 30;


	public static void Main(string[] args)
    {
        InitializeUserData();
        InitializeTasksData();

        while (isProgramRunning)
        {
            var commandLine = Console.ReadLine();

			switch (programMode)
            {
                case ProgramWorkMode.ProcessingCommands:
                    ProcessCommand(commandLine);
                    break;
                case ProgramWorkMode.AddingTask:
                    AddNewTaskLine(commandLine);
                    break;
            }

        }
    }


    private static void EnterDefaultProgramMode()
    {
        programMode = ProgramWorkMode.ProcessingCommands;
    }


    private static void ShowHelpInfo()
    {
        Console.WriteLine("****\tUserInfo Помощник\t****");

        Console.WriteLine($"{CommandProfileName} — выводит данные пользователя в формате: <Имя> <Фамилия>, <Год рождения> .");

        Console.WriteLine($"{CommandAddName}" +
			$" — добавляет новую задачу. Формат ввода: add \"текст задачи\",\n или мультистрочно при наличии флагов {CommandAddMultilineFlags[0]} или " +
			$"{CommandAddMultilineFlags[1]},\n чтобы завершить написание задачи введите {CommandEndName}.\n");

        Console.WriteLine($"{CommandViewName} — выводит все задачи из массива (только непустые элементы),\n {CommandViewIndexFlags[0]} или {CommandViewIndexFlags[1]} чтобы вывести индексы задач,\n" +
			$"{CommandViewStatusFlags[0]} или {CommandViewStatusFlags[1]} чтобы отобразть статусы задач,\n" +
			$"{CommandViewUpdateFlags[0]} или {CommandViewUpdateFlags[1]} чтобы узреть дату внесения последнего изменения\n" +
			$"{CommandViewAllFlags[0]} или {CommandViewAllFlags[1]} чтобы показать всю дополнительную информацию.\n");

        Console.WriteLine($"{CommandExitName} — завершает цикл и останавливает выполнение программы.");
        Console.WriteLine($"{CommandDoneName} — отмечает задачу выполненной.");
        Console.WriteLine($"{CommandDeleteName} — <idx> — удаляет задачу по индексу.");
        Console.WriteLine($"{CommandUpdateName} — <idx> \"new_text\" — обновляет текст задачи.");
        Console.WriteLine($"{CommandReadName} — <idx> выводит полный текст задачи, ее статус, и дату последнего изменения.");
    }

    private static void ExitProgram() => isProgramRunning = false;


    private static void InsertNewTask(string task)
    {
        int newTodosCount = todosCount + 1;

        // Расширение массива 'todos'
        if (newTodosCount > todosLen)
        {
            int newTodosLen = todosLen * 2;

            string[] newTodos = new string[newTodosLen];
            bool[] newStatuses = new bool[newTodosLen];
            DateTime[] newDates = new DateTime[newTodosLen];

            for (int i = 0; i < todosCount; i++)
            {
                newTodos[i] = todos[i];
                newStatuses[i] = statuses[i];
                newDates[i] = dates[i];
            }

            newTodos[todosCount] = task;
            newDates[todosCount] = DateTime.Now;
            newStatuses[todosCount] = false;

            todos = newTodos;
            statuses = newStatuses;
            dates = newDates;

            todosLen = newTodosLen;
        }
        else
        {
            todos[todosCount] = task;
            dates[todosCount] = DateTime.Now;
            statuses[todosCount] = false;
        }

        todosCount = newTodosCount;
    }


    private static void EnterMultilineTaskReadingMode()
    {
        programMode = ProgramWorkMode.AddingTask;
		addingMultilineTask = "";
	}

	private static void AddNewTaskFromCommand(string command)
    {
        string newTask = "";
        var userEnteredTask = command.Split($"{CommandAddName} ");

        // checking for flags
        foreach (var i in CommandAddMultilineFlags)
        {
            if (userEnteredTask.Contains(i))
            {
                EnterMultilineTaskReadingMode();
                return;
            }
        }

        foreach (var item in userEnteredTask)
        {
            newTask += item;
        }

        InsertNewTask(newTask);

        Console.WriteLine($"Добавлена новая задача: \"{newTask}\".");
    }

    private static void ShowProfileInfo()
    {
        Console.WriteLine($"Данные пользователя: \"{userData.firstName}\" \"{userData.lastName}\", {userData.birthYear}");
    }

    private static void ViewTasksInfo(string command)
    {
		bool indexed = false, statused = false, update = false, all = false;

        Console.WriteLine("===========================================================");
        Console.WriteLine("****\tИнформация о задачах\t****");

		foreach (var i in CommandViewIndexFlags)
			indexed = command.Contains(i);

		foreach (var i in CommandViewStatusFlags)
			statused = command.Contains(i);

		foreach (var i in CommandViewUpdateFlags)
			update = command.Contains(i);

		foreach (var i in CommandViewAllFlags)
			all = command.Contains(i);

		for (int i = 0; i < todosCount; i++)
		{
			string textOfView = "";

			if (indexed || all)
				textOfView += $"индекс: \"{i}\"";

			if (statused || all)
			{
				string isDone = statuses[i] ? "сделано" : "не сделано";
				textOfView += $"\tстатус: \"{isDone}\""; 
			}

			if (update || all)
				textOfView += $"\tдата обновления: \"{dates[i]}\"";

			string taskText = todos[i].Length <= MaxTaskDisplayTextLen ? todos[i] : (todos[i].Substring(0, MaxTaskDisplayTextLen) + "...");
			textOfView += $"\t задача: \"{taskText}\"";

			Console.WriteLine(textOfView);
		}

		Console.WriteLine("===========================================================");
    }


    private static int ReadIndexFromCommand(string commandName, string command, bool checkForNumOfTasks = true)
    {
        var items = command.Split(commandName);
        Debug.Assert(items.Length >= 2);

        bool indexValid = int.TryParse(items[1], out int index);
        Debug.Assert(indexValid);

		if (checkForNumOfTasks && (index < 0 || index >= todosCount))
		{
			throw new Exception($"ReadIndexFromCommand: Uncorrected index {index}!");
		}

        return index;
    }

    private static void DoneTask(string command)
    {
        int index = ReadIndexFromCommand(CommandDoneName, command);
        statuses[index] = true;
		dates[index] = DateTime.Now;
        Console.WriteLine($"Задача под номером {index} завершена.");
    }

    private static void DeleteTask(string command)
    {
        if (todosCount == 0)
        {
            Console.WriteLine("Тут нечего удалять.");
            return;
        }

        int index = ReadIndexFromCommand(CommandDeleteName, command);

        var newTodos = todos;
        var newStatuses = statuses;
        var newDates = dates;

        for (int i = index;i< todosCount; i++)
        {
            newTodos[i - 1] = todos[i];
            newStatuses[i - 1] = statuses[i];
            newDates[i - 1] = dates[i];
        }

        newTodos[todosCount - 1] = null;
        newStatuses[todosCount - 1] = false;
        newDates[todosCount - 1] = new DateTime();

        todos = newTodos;
        statuses = newStatuses;
        dates = newDates;
        todosCount--;
    }

    private static void UpdateTaskText(string command)
    {
        var args = command.Split(' ', 3);

        bool indexValid = int.TryParse(args[1], out int index);
        if (!indexValid)
        {
            Console.WriteLine($"{CommandUpdateName}: не верный индекс задачи.");
            return;
        }
        
        if (index < 0 && index > todosCount)
        { 
            Console.WriteLine($"{CommandUpdateName}: задачи под номером {index} не существует.");
            return;
        }

        todos[index] = args[2];
		dates[index] = DateTime.Now;

        Console.WriteLine($"Задача под номером {index} изменена на \"{todos[index]}\".");
    }

	private static void ReadFullTaskText(string command)
	{
		int index = ReadIndexFromCommand(CommandReadName, command);

		Console.WriteLine(todos[index]);

		string statusText = statuses[index] ? "выполнена" : "не выполнена";
		Console.WriteLine($"( {statusText} )");

		Console.WriteLine($"Дата последнего изменения: {dates[index]}");
	}


    // Эта байда обрабатывает комманду введенную юзером
    private static void ProcessCommand(string command)
    {
		// Проверка комманд, если комманда опознана, то выполняется соответствующая процедупа

		if (command.StartsWith(CommandHelpName))
		{
			ShowHelpInfo();
		}
		else if (command.StartsWith(CommandExitName))
		{
			ExitProgram();
		}
		else if (command.StartsWith(CommandAddName))
		{
			AddNewTaskFromCommand(command);
		}
		else if (command.StartsWith(CommandProfileName))
		{
			ShowProfileInfo();
		}
		else if (command.StartsWith(CommandViewName))
		{
			ViewTasksInfo(command);
		}
		else if (command.StartsWith(CommandDoneName))
		{
			DoneTask(command);
		}
		else if (command.StartsWith(CommandDeleteName))
		{
			DeleteTask(command);
		}
		else if (command.StartsWith(CommandUpdateName))
		{
			UpdateTaskText(command);
		}
		else if (command.StartsWith(CommandReadName))
		{
			ReadFullTaskText(command);
		}
		else
		{
			// Если ни одна комманда не распознана
			Console.WriteLine("Неизвестная комманда!");
		}
    }

    // Получение данных пользователя и их обработка 
    private static void InitializeUserData()
    {
        Console.WriteLine("Работу выполнили Чернова Юлия и Соловьев Иван 3833");

        Console.WriteLine("Введите имя:");
        userData.firstName = Console.ReadLine();

        Console.WriteLine("Введите фамилию:");
        userData.lastName = Console.ReadLine();
        Console.WriteLine("Введите год рождения:");
        userData.birthYearString = Console.ReadLine();

        bool isBirthYearValid = int.TryParse(userData.birthYearString, out userData.birthYear);

        if (!isBirthYearValid)
        {
            Console.WriteLine("Ошибка: Некорректный год рождения. Пожалуйста, введите целое число.");
            return;
        }

        userData.currentYear = DateTime.Now.Year;
        userData.age = userData.currentYear - userData.birthYear;

        Console.WriteLine($"Добавлен пользователь {userData.firstName} {userData.lastName}, возраст - {userData.age}");
    }


    private static void InitializeTasksData()
    {
        todosCount = 0;
        todosLen = todosStartLen;

        todos = new string[todosStartLen];
        statuses = new bool[todosStartLen];
        dates = new DateTime[todosStartLen];

        EnterDefaultProgramMode();
    }

    private static void AddNewTaskLine(string commandLine)
    {
        if (commandLine.StartsWith(CommandEndName))
        {
            AddNewTaskFromCommand(addingMultilineTask);
            EnterDefaultProgramMode();
        }

        addingMultilineTask += commandLine + "\n";
    }

}