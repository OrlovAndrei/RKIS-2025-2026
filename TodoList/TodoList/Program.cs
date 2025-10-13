

using System.Security.Cryptography;

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
                        AllArrayExpension(statuses, dates, todos);
                    if (addCommand.Contains("-m") || addCommand.Contains("--multiline"))
                        MultiLineAddTask(todos, statuses, dates, ref currentTaskID);
                    else
                        AddTask(todos, statuses, dates, ref currentTaskID, addCommand);
                    break;
                case "view":
                    GetTodoInfo(todos, statuses, dates);
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
    private static void GetTodoInfo(string[] todos, bool[] statuses, DateTime[] dates)
    {
        Console.WriteLine("Ваш список задач:");
        for (int i = 0; i < todos.Length; i++)
        {
            if (!string.IsNullOrEmpty(todos[i]))
                Console.WriteLine($"{i} {todos[i]} {statuses[i]} {dates[i]}");
        }
    }
    private static void GetHelpInfo()
    {
        Console.WriteLine("help - выводит список всех доступных команд\nprofile - выводит ваши данные\nadd - добавляет новую задачу (add \"Новая задача\")\nview - просмотр задач\ndone - отмечает задачу выполненной\ndelete - удаляет задачу по индексу\nupdate\"new_text\"- обновляет текст задачи\nexit - выйти");
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
    private static void AllArrayExpension(bool [] statusesArray, DateTime[] dateArray, string[] todoArray)
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