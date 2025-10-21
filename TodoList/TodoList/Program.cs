

internal class Program
{
	private static void Main(string[] args)
    {
        Console.WriteLine("Работу выполнили: Амелина Яна и Кабанова Арина");
        string name, surname;
		int age, currentYear = 2025, yearOfBirth;
		AddUser(out name, out surname, currentYear, out yearOfBirth, out age);
		List<TodoItem> todos = new List<TodoItem>();
		bool isOpen = true;
		Console.ReadKey();
        while (isOpen)
        {
            Console.Clear();
            string userCommand = "";
            Console.WriteLine("Введите команду:\nдля помощи напиши команду help");
            userCommand = Console.ReadLine();
            switch (userCommand)
            {
                case "help":
                    GetHelpInfo();
                    break;
                case "profile":
                    GetUserInfo("Пользователь: ", name, surname, age);
                    break;
				case string addCommand when addCommand.StartsWith("add"):
					if (addCommand.Contains("-m") || addCommand.Contains("--multiline"))
						MultiLineAddTask(todos);
					else
						AddTask(todos, addCommand);
					break;
				case string viewCommand when viewCommand.StartsWith("view"):
					GetTodoInfo(todos, viewCommand);
					break;
				case string markTaskDone when markTaskDone.StartsWith("done "):
					MarkTaskDone(todos, markTaskDone);
					break;
				case string taskToDelete when taskToDelete.StartsWith("delete "):
					DeleteTask(todos, taskToDelete);
					break;
				case string updateTaskText when updateTaskText.StartsWith("update "):
					UpdateTask(todos, updateTaskText);
					break;
				case string readTaskText when readTaskText.StartsWith("read"):
					ReadFullTask(todos, readTaskText);
					break;
				case "exit":
                    isOpen = false;
                    break;
                default:
                    Console.WriteLine("Неправильно введена команда");
                    break;
            }
            Console.ReadKey();
        }
    }
	private static void GetTodoInfo(List<TodoItem> todos, string viewCommand)
	{
		bool allOutput = viewCommand.Contains("--all");
		bool showIndex = viewCommand.Contains("--index");
		bool showStatus = viewCommand.Contains("--status");
		bool showDate = viewCommand.Contains("--update-date");
		if (!viewCommand.Contains("--"))
		{
			int indexOfShortFlag = viewCommand.IndexOf("-");
			if (indexOfShortFlag >= 0)
			{
				for (int i = indexOfShortFlag + 1; i < viewCommand.Length; i++)
				{
					if (viewCommand[i] == 'i')
						showIndex = true;
					if (viewCommand[i] == 'd')
						showDate = true;
					if (viewCommand[i] == 's')
						showStatus = true;
					if (viewCommand[i] == 'a')
						allOutput = true;
				}
			}
		}
		if (viewCommand.Contains("-a"))
		{
			showIndex = true;
			showStatus = true;
			showDate = true;
		}
		if (!showIndex && !showStatus && !showDate)
		{
			Console.WriteLine("Ваш список задач:");
			foreach (var todo in todos)
			{
				Console.WriteLine(todo.GetShortInfo());
			}
			return;
		}
		Console.WriteLine("Ваш список задач:");
		PrintTableHeader(showIndex, showStatus, showDate);
		PrintTableSeparator(showIndex, showStatus, showDate);
		for (int i = 0; i < todos.Count; i++)
		{
			var todo = todos[i];
			PrintTaskRow(i, todo.Text, todo.IsDone, todo.LastUpdate, showIndex, showStatus, showDate);
		}
	}
	private static string GetTruncatedTaskText(string taskText)
    {
        if (string.IsNullOrEmpty(taskText))
            return "";
        taskText = taskText.Replace("\r", " ").Replace("\n", " ");
        while (taskText.Contains("  "))
            taskText = taskText.Replace("  ", " ");
        if (taskText.Length <= 30)
            return taskText;
        return taskText.Substring(0, 27) + "...";
    }
    private static void PrintTableHeader(bool showIndex, bool showStatus, bool showDate)
    {
        List<string> headers = new List<string>();
        if (showIndex)
            headers.Add($"{"Индекс",-6}");
        headers.Add($"{"Задача",-30}");
        if (showStatus)
            headers.Add($"{"Статус",-10}");
        if (showDate)
            headers.Add($"{"Дата изменения",-19}");
        Console.WriteLine("| " + string.Join(" | ", headers) + " |");
    }
    private static void PrintTableSeparator(bool showIndex, bool showStatus, bool showDate)
    {
        List<string> separators = new List<string>();
        if (showIndex)
            separators.Add(new string('-', 6));
        separators.Add(new string('-', 30));
        if (showStatus)
            separators.Add(new string('-', 10));
        if (showDate)
            separators.Add(new string('-', 19));
        Console.WriteLine("|-" + string.Join("-|-", separators) + "-|");
    }
    private static void PrintTaskRow(int index, string task, bool status, DateTime date, bool showIndex, bool showStatus, bool showDate)
    {
        List<string> columns = new List<string>();
        if (showIndex)
            columns.Add($"{index,6}");
        string taskText = GetTruncatedTaskText(task);
        columns.Add($"{taskText,-30}");
        if (showStatus)
            columns.Add($"{(status ? "Выполнена" : "Не выполнена"),-10}");
        if (showDate)
            columns.Add($"{date:dd.MM.yyyy HH:mm:ss}");
        Console.WriteLine("| " + string.Join(" | ", columns) + " |");
    }
	private static void ReadFullTask(List<TodoItem> todos, string readCommand)
	{
		string[] splitCommand = readCommand.Split(' ');
		if (splitCommand.Length < 2 || !int.TryParse(splitCommand[1], out int taskId) || taskId < 0 || taskId >= todos.Count)
		{
			Console.WriteLine("Неверный индекс задачи");
			return;
		}
		Console.WriteLine(todos[taskId].GetFullInfo());
	}
	private static void GetHelpInfo()
    {
        Console.WriteLine("help - выводит список всех доступных команд\n" +
                         "profile - выводит ваши данные\n" +
                         "add - добавляет новую задачу (add \"Новая задача\")(флаги: add --multiline/-m, !end)\n" +
                         "view - просмотр задач (флаги: --index/-i, --status/-s, --update-date/-d, --all/-a)\n" +
                         "read idx - просмотр полного текста задач\n" +
                         "done - отмечает задачу выполненной\n" +
                         "delete - удаляет задачу по индексу\n" +
                         "update\"new_text\"- обновляет текст задачи\n" +
                         "exit - выйти");
    }
    static void AddUser (out string name, out string surname, int currentYear, out int yearOfBirth, out int age)
    {
    WrongNameMark:
        Console.WriteLine("Напишите ваше имя и фамилию:");
        string fullName = Console.ReadLine();
        if (string.IsNullOrEmpty(fullName))
        {
            Console.WriteLine("Вы ничего не ввели");
            goto WrongNameMark;
        }
        string[] splitFullName = fullName.Split(' ', 2);
        name = splitFullName[0];
        surname = splitFullName[1];
        Console.WriteLine("Напишите свой год рождения:");
        yearOfBirth = int.Parse(Console.ReadLine());
        age = currentYear - yearOfBirth;
        string userAdded = "Добавлен пользователь: ";
        GetUserInfo (userAdded, name, surname, age);
    }
    private static void GetUserInfo (string userInfo, string name, string surname, int age)
    {
        Console.WriteLine(userInfo + name + " " + surname + ", возраст: " + age);
    }
    private static void AllArrayExpension(ref bool [] statusesArray, ref DateTime[] dateArray, ref string[] todoArray)
    {
        string[] tempArray = new string[todoArray.Length * 2];
        DateTime[] tempDateArray = new DateTime[todoArray.Length * 2];
        bool[] tempStatusArray = new bool[todoArray.Length * 2];
        for (int i = 0; i < todoArray.Length; i++)
        {
            tempArray[i] = todoArray[i];
            tempDateArray[i] = dateArray[i];
            tempStatusArray[i] = statusesArray[i];
        }
            todoArray = tempArray;
        dateArray = tempDateArray;
        statusesArray = tempStatusArray;
    }
	private static void AddTask(List<TodoItem> todos, string task)
	{
		string[] taskText = task.Split('\"', 3);
		TodoItem newTodo = new TodoItem(taskText[1]);
		todos.Add(newTodo);
	}
	private static void MarkTaskDone(List<TodoItem> todos, string doneCommandText)
	{
		string[] taskDone = doneCommandText.Split(' ', 2);
		int userTaskID = int.Parse(taskDone[1]);
		todos[userTaskID].MarkDone();
	}
	private static void DeleteTask(List<TodoItem> todos, string deleteTaskText)
	{
		string[] splitDeleteTaskText = deleteTaskText.Split(' ', 2);
		int deleteTaskID = int.Parse(splitDeleteTaskText[1]);
		todos.RemoveAt(deleteTaskID);
	}
	private static void UpdateTask(List<TodoItem> todos, string updateTasktext)
	{
		string[] splitUpdateTaskText = updateTasktext.Split('\"', 3);
		string[] splitUpdateTaskID = updateTasktext.Split(' ');
		int taskID = int.Parse(splitUpdateTaskID[1]);
		todos[taskID].UpdateText(splitUpdateTaskText[1]);
	}
	private static void MultiLineAddTask(List<TodoItem> todos)
	{
		string userInput = "";
		bool isInput = true;
		string userTask = "";
		while (isInput)
		{
			Console.Write("> ");
			userInput = Console.ReadLine();
			if (userInput == "!end")
				isInput = false;
			else
				userTask = userTask + "\n" + userInput;
		}
		TodoItem newTodo = new TodoItem(userTask);
		todos.Add(newTodo);
	}
}