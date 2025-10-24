using System;
using System.Diagnostics;
using System.Reflection;

namespace TodoList;
public class Program 
{

	private static Profile profile = new Profile();
	private static TodoList todoList = new TodoList();
    private static bool isProgramRunning = true;


    private const string CommandAddName = "add";
    private const string CommandProfileName = "profile";
    private const string CommandViewName = "view";
    private const string CommandExitName = "exit";
    private const string CommandHelpName = "help";
    private const string CommandDoneName = "done";
    private const string CommandDeleteName = "delete";
    private const string CommandUpdateName = "update";
    private const string CommandReadName = "read";

    private const string CommandAddEndMark = "!end";

    private static string[] CommandAddMultilineFlags = new string[]
    {
        "--multiline",
        "-m"
    };

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
		Console.WriteLine("Работу выполнили Чернова Юлия и Соловьев Иван 3833");

		InitializeUserData();

        while (isProgramRunning)
        {
            var commandLine = Console.ReadLine();
			ProcessCommand(commandLine);
		}
    }

    private static void ShowHelpInfo()
    {
		Console.WriteLine($"""
			****\tUserInfo Помощник\t****
			{CommandProfileName} — выводит данные пользователя в формате: <Имя> <Фамилия>, <Год рождения>.
			{CommandAddName} — добавляет новую задачу. Формат ввода: add \"текст задачи\",\n или мультистрочно при наличии флагов {CommandAddMultilineFlags[0]} или {CommandAddMultilineFlags[1]},\n чтобы завершить написание задачи введите {CommandAddEndMark}.

			{CommandViewName} — выводит все задачи из массива (только непустые элементы),
			{CommandViewIndexFlags[0]} или {CommandViewIndexFlags[1]} чтобы вывести индексы задач,
			{CommandViewUpdateFlags[0]} или {CommandViewUpdateFlags[1]} чтобы узреть дату внесения последнего изменения,
			{CommandViewAllFlags[0]} или {CommandViewAllFlags[1]} чтобы показать всю дополнительную информацию.

			{CommandExitName} — завершает цикл и останавливает выполнение программы.
			{CommandDoneName} — <idx> отмечает задачу выполненной.
			{CommandUpdateName} — <idx> \"new_text\" — обновляет текст задачи.
			{CommandReadName} — <idx> выводит полный текст задачи, ее статус, и дату последнего изменения.
			""");
    }

    private static void ExitProgram() => isProgramRunning = false;


    private static void InsertNewTask(string task)
    {
		todoList.Add(new Todoitem(task));
    }

	private static bool IsTaskValidToAdd(string task)
	{
		return task.Length > 0;
	}

	private static void AddNewTaskFromCommand(string command)
    {
        string newTask = "";
		bool multiline = false;
        var userEnteredTask = command.Split(' ', 2);

        // checking for flags
        foreach (var i in CommandAddMultilineFlags)
        {
            if (userEnteredTask.Contains(i))
            {
				multiline = true;
            }
        }

		if (multiline)
		{
			for (var line = Console.ReadLine(); line != CommandAddEndMark; line = Console.ReadLine())
			{
				newTask += line + "\n";
			}
		}
		else
		{
			if (userEnteredTask.Length < 2)
			{
				Console.WriteLine("Нечего добавлять, задачи то нет.");
				return;
			}

			if (IsTaskValidToAdd(userEnteredTask[1]))
				newTask = userEnteredTask[1];
			else
			{
				Console.WriteLine("Нечего добавлять, задачи то нет.");
				return;
			}
		}

		InsertNewTask(newTask);

        Console.WriteLine($"Добавлена новая задача:\n{newTask}");
    }

    private static void ShowProfileInfo()
    {
        Console.WriteLine($"Данные пользователя: {profile.GetInfo()}");
    }

	private static bool LineFlagsFounded(string command, string[] flags)
	{
		bool flag = false;

		foreach (var i in flags)
		{ 
			if (command.Contains(i))
				flag = true;
		}

		return flag;
	}

	private static void ViewTasksInfo(string command)
	{
		string[] userEnteredCommand = command.Split(' ');

		Console.WriteLine("===========================================================");
		Console.WriteLine("****\tИнформация о задачах\t****");

		// checking for flags
		bool indexed = LineFlagsFounded(command, CommandViewIndexFlags);
		bool statused = LineFlagsFounded(command, CommandViewStatusFlags);
		bool update = LineFlagsFounded(command, CommandViewUpdateFlags);
		bool all = LineFlagsFounded(command, CommandViewAllFlags);

		// checking for multiflags 
		foreach (var i in userEnteredCommand)
		{
			if (i.StartsWith("-"))
			{
				for (int j = 1;j < i.Length;j++)
				{
					if (i[j] == CommandViewIndexFlags[1][1])
						indexed = true;
					if (i[j] == CommandViewStatusFlags[1][1])
						statused = true;
					if (i[j] == CommandViewUpdateFlags[1][1])
						update = true;
					if (i[j] == CommandViewAllFlags[1][1])
						all = true;
				}
			}
		}

		// complete tasks info showing
		todoList.View(indexed, statused, update);

		Console.WriteLine("===========================================================");
    }


    private static int ReadIndexFromCommand(string commandName, string command, bool checkForNumOfTasks = true)
    {
        var items = command.Split(commandName);
        Debug.Assert(items.Length >= 2);

        bool indexValid = int.TryParse(items[1], out int index);
        Debug.Assert(indexValid);

		if (checkForNumOfTasks && (index < 0 || index >= todoList.Lenght))
		{
			throw new Exception($"ReadIndexFromCommand: Uncorrected index {index}!");
		}

        return index;
    }

    private static void DoneTask(string command)
    {
        int index = ReadIndexFromCommand(CommandDoneName, command);
        todoList.GetItem(index).MarkDone();
        Console.WriteLine($"Задача под номером {index} завершена.");
    }

    private static void DeleteTask(string command)
    {
		int index = ReadIndexFromCommand(CommandDeleteName, command);
		todoList.Delete(index);
		Console.WriteLine($"Задача под номером {index} удалена.");

		// TODO: implement todoList object methods

		//if (todosCount == 0)
		//{
		//    Console.WriteLine("Тут нечего удалять.");
		//    return;
		//}

		//int index = ReadIndexFromCommand(CommandDeleteName, command);

		//var newTodos = todos;
		//var newStatuses = statuses;
		//var newDates = dates;

		//for (int i = index;i< todosCount; i++)
		//{
		//    newTodos[i - 1] = todos[i];
		//    newStatuses[i - 1] = statuses[i];
		//    newDates[i - 1] = dates[i];
		//}

		//newTodos[todosCount - 1] = null;
		//newStatuses[todosCount - 1] = false;
		//newDates[todosCount - 1] = new DateTime();

		//todos = newTodos;
		//statuses = newStatuses;
		//dates = newDates;
		//todosCount--;
	}

    private static void UpdateTaskText(string command)
    {
		int index = ReadIndexFromCommand(CommandUpdateName, command);
		string newText = Console.ReadLine();
		todoList.GetItem(index).UpdateText(newText);

		// TODO: implement todoList object methods

		//      var args = command.Split(' ', 3);

		//      bool indexValid = int.TryParse(args[1], out int index);
		//      if (!indexValid)
		//      {
		//          Console.WriteLine($"{CommandUpdateName}: не верный индекс задачи.");
		//          return;
		//      }

		//      if (index < 0 && index > todosCount)
		//      { 
		//          Console.WriteLine($"{CommandUpdateName}: задачи под номером {index} не существует.");
		//          return;
		//      }
		//      todos[index] = args[2];
		//dates[index] = DateTime.Now;		
		//      Console.WriteLine($"Задача под номером {index} изменена на \"{todos[index]}\".");
	}

	private static void ReadFullTaskText(string command)
	{
		int index = ReadIndexFromCommand(CommandReadName, command);
		todoList.GetItem(index).GetFullInfo();
		// TODO: implement todoList object methods

		//int index = ReadIndexFromCommand(CommandReadName, command);

		//Console.WriteLine(todos[index]);

		//string statusText = statuses[index] ? "выполнена" : "не выполнена";
		//Console.WriteLine($"( {statusText} )");

		//Console.WriteLine($"Дата последнего изменения: {dates[index]}");
	}


	// Эта байда обрабатывает комманду введенную юзером
	private static void ProcessCommand(string command)
    {
		// Проверка комманд, если комманда опознана, то выполняется соответствующая процедупа

		if (command.StartsWith(CommandHelpName)) ShowHelpInfo();
		else if (command.StartsWith(CommandExitName)) ExitProgram();
		else if (command.StartsWith(CommandAddName)) AddNewTaskFromCommand(command);
		else if (command.StartsWith(CommandProfileName)) ShowProfileInfo();
		else if (command.StartsWith(CommandViewName)) ViewTasksInfo(command);
		else if (command.StartsWith(CommandDoneName)) DoneTask(command);
		else if (command.StartsWith(CommandDeleteName)) DeleteTask(command);
		else if (command.StartsWith(CommandUpdateName)) UpdateTaskText(command);
		else if (command.StartsWith(CommandReadName)) ReadFullTaskText(command);
		else Console.WriteLine("Неизвестная комманда!");
    }

	private static void GetUserAge()
	{
		Console.WriteLine("Введите год рождения:");
		string birthYearString = Console.ReadLine();

		bool isBirthYearValid = int.TryParse(birthYearString, out int birthYear);

		if (isBirthYearValid)
		{ 
			profile.BirthYear = birthYear;	
			return; 
		}
		else
		{
			Console.WriteLine("Ошибка: Некорректный год рождения, пожалуйста, введите целое число.");
			GetUserAge();
		}
	}

    // Получение данных пользователя и их обработка 
    private static void InitializeUserData()
    {
		Console.WriteLine("Введите имя:");
		profile.FirstName = Console.ReadLine();

        Console.WriteLine("Введите фамилию:");
		profile.LastName = Console.ReadLine();

		GetUserAge();

		int currentYear = DateTime.Now.Year;
		int age = currentYear - profile.BirthYear;

        Console.WriteLine($"Добавлен пользователь {profile.FirstName} {profile.LastName}, возраст - {age}");
    }

}