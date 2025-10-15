
internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Работу выполнили: Амелина Яна и Кабанова Арина");
        string name, surname;
        int age, currentYear = 2025, yearOfBirth;
        AddUser(out name, out surname, currentYear, out yearOfBirth, out age);
        int arrayLength = 2;
        string[] todos = new string[arrayLength];
        bool[] statuses = new bool[arrayLength];
        DateTime[] dates = new DateTime[arrayLength];
        bool isOpen = true;
        int currentTaskID = 0;
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
                    if (currentTaskID == todos.Length)
                        AllArrayExpension(ref statuses, ref dates, ref todos);
                    if (addCommand.Contains("-m") || addCommand.Contains("--multiline"))
                        MultiLineAddTask(todos, statuses, dates, ref currentTaskID);
                    else
                        AddTask(todos, statuses, dates, ref currentTaskID, addCommand);
                    break;
                case string viewCommand when viewCommand.StartsWith("view"):
                    GetTodoInfo(todos, statuses, dates, viewCommand);
                    break;
                case string markTaskDone when markTaskDone.StartsWith("done "):
                    MarkTaskDone(statuses, dates, markTaskDone);
                    break;
                case string taskToDelete when taskToDelete.StartsWith("delete "):
                    DeleteTask(todos, statuses, dates, taskToDelete);
                    break;
                case string uppdateTaskText when uppdateTaskText.StartsWith("update "):
                    UpdateTask(todos, dates, uppdateTaskText);
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
    private static void GetTodoInfo(string[] todos, bool[] statuses, DateTime[] dates, string viewCommand)
    {
        bool showIndex = viewCommand.Contains("--index") || viewCommand.Contains("-i") ||
            viewCommand.Contains("--all") || viewCommand.Contains("-a");
        bool showStatus = viewCommand.Contains("--status") || viewCommand.Contains("-s") ||
            viewCommand.Contains("--all") || viewCommand.Contains("-a");
        bool showDate = viewCommand.Contains("--update-date") || viewCommand.Contains("-d") ||
            viewCommand.Contains("--all") || viewCommand.Contains("-a");
        if (!showIndex && !showStatus && !showDate)
        {
            Console.WriteLine("Ваш список задач:");
            for (int i = 0; i<todos.Length; i++)
            {
                if (!string.IsNullOrEmpty(todos[i]))
                {
                    string taskText = GetTruncatedTaskText(todos[i]);
                    Console.WriteLine(taskText);
                }
            }
            return;
        }
        Console.WriteLine("Ваш список задач:");
            PrintTableHeader(showIndex, showStatus, showDate);
            PrintTableSeparator(showIndex, showStatus, showDate);

        for (int i = 0; i < todos.Length; i++)
        {
            if (!string.IsNullOrEmpty(todos[i]))
            {
                PrintTaskRow(i, todos[i], statuses[i], dates[i], showIndex, showStatus, showDate);
            }
        }
    }
    private static string GetTruncatedTaskText(string taskText)
    {
        if (taskText.Length <= 30)
            return taskText;
        return taskText.Substring(0, 27) + "...";
    }
    private static void PrintTableHeader(bool showIndex, bool showStatus, bool showDate)
    {
        List<string> headers = new List<string>();
        if (showIndex)
            headers.Add("Индекс");
        headers.Add("Задача");
        if (showStatus)
            headers.Add("Статус");
        if (showDate)
            headers.Add("Дата изменения");
        Console.WriteLine("| " + string.Join(" | ", headers) + " |");
    }
    private static void PrintTableSeparator(bool showIndex, bool showStatus, bool showDate)
    {
        List<string> separators = new List<string>();
        if (showIndex)
            separators.Add(new string('-', 7));
        separators.Add(new string('-', 30));
        if (showStatus)
            separators.Add(new string('-', 8));
        if (showDate)
            separators.Add(new string('-', 21));
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
            columns.Add($"{(status ? "Выполнена" : "Не выполнена"),-8}");
        if (showDate)
            columns.Add($"{date:dd.MM.yyyy HH:mm:ss}");
        Console.WriteLine("| " + string.Join(" | ", columns) + " |");
    }
    private static void GetHelpInfo()
    {
        Console.WriteLine("help - выводит список всех доступных команд\n" +
                         "profile - выводит ваши данные\n" +
                         "add - добавляет новую задачу (add \"Новая задача\")\n" +
                         "view - просмотр задач (флаги: --index/-i, --status/-s, --update-date/-d, --all/-a)\n" +
                         "done - отмечает задачу выполненной\n" +
                         "delete - удаляет задачу по индексу\n" +
                         "update\"new_text\"- обновляет текст задачи\n" +
                         "exit - выйти");
    }
    static void AddUser (out string name, out string surname, int currentYear, out int yearOfBirth, out int age)
    {
        Console.WriteLine("Напишите ваше имя и фамилию:");
        string fullName = Console.ReadLine();
        string[] splitFullName = fullName.Split(' ', 2);
        name = splitFullName[0];
        surname = splitFullName[1];
        Console.WriteLine("Напишите свой год рождения:");
        yearOfBirth = int.Parse (Console.ReadLine());
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
    private static void AddTask (string[] todoArray, bool[] statuses, DateTime[] dates, ref int currentTaskID, string task)
    {
        string[] taskText = task.Split('\"', 3);
        todoArray[currentTaskID] = taskText[1];
        dates[currentTaskID] = DateTime.Now;
        statuses[currentTaskID] = false;
        currentTaskID++;

    }
    private static void MarkTaskDone (bool[] statuses, DateTime[] dates, string doneCommandText)
    {
        string[] taskDone = doneCommandText.Split(' ', 2);
        int userTaskID = int.Parse(taskDone[1]);
        statuses[userTaskID] = true;
        dates[userTaskID] = DateTime.Now;
    }
    private static void DeleteTask (string[] todoArray, bool[] statuses, DateTime[] dateArray, string deleteTaskText)
    {
        string[] splitDeleteTaskText = deleteTaskText.Split(' ', 2);
        int deleteTaskID = int.Parse(splitDeleteTaskText[1]);
        for (int i = deleteTaskID; i < todoArray.Length - 1; i++)
        {
            todoArray[i] = todoArray[i + 1];
            statuses[i] = statuses[i + 1];
            dateArray[i] = dateArray[i + 1];
        }
    }
    private static void UpdateTask (string[] todos, DateTime[] dateArray, string updateTasktext)
    {
        string[] splitUpdateTaskText = updateTasktext.Split('\"', 3);
        string[] splitUpdateTaskID = updateTasktext.Split(' ');
        int taskID = int.Parse(splitUpdateTaskID[1]);
        todos[taskID] = splitUpdateTaskID[2];
        dateArray[taskID] = DateTime.Now;
    }
    private static void MultiLineAddTask(string[] todoArray, bool[] statuses, DateTime[] dates, ref int currentTaskID)
    {
        string userInput = "";
        bool isInput = true;
        string userTask = "";
        while (isInput)
        {
            userInput = Console.ReadLine();
            if (userInput == "!end")
                isInput = false;
            else
                userTask = userTask + "\n" + userInput;
        }
        todoArray[currentTaskID] = userTask;
        dates[currentTaskID] = DateTime.Now;
        statuses[currentTaskID] = false;
        currentTaskID++;

    }
}