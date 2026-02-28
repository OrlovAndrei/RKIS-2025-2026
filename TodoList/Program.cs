namespace TodoList;

internal class Program
{
    private static string name;
    private static string surname;
    private static int age;

    private static string[] todos = new string[2];
    private static bool[] todosStatuses = new bool[2];
    private static DateTime[] todosDates = new DateTime[2];
    private static int index;

    public static void Main()
    {
        Console.WriteLine("Работу выполнили Зусикова и Кабачек 3833.9");
        Console.WriteLine("Введите ваше имя:");
        name = Console.ReadLine();
        Console.WriteLine("Введите вашу фамилию:");
        surname = Console.ReadLine();

        Console.WriteLine("Введите ваш год рождения:");
        var year = int.Parse(Console.ReadLine());
        age = DateTime.Now.Year - year;

        Console.WriteLine("Добавлен пользователь " + name + " " + surname + ", возраст - " + age);

        while (true)
        {
            Console.WriteLine("Введите команду:");
            var command = Console.ReadLine();

            if (command == "help")
            {
                PrintHelp();
            }
            else if (command == "profile")
            {
                PrintProfile();
            }
            else if (command == "exit")
            {
                Console.WriteLine("Выход из программы.");
                break;
            }
            else if (command.StartsWith("add "))
            {
                AddTaskCommand(command);
            }
            else if (command == "view")
            {
                PrintListOfTasks();
            }
            else if (command.StartsWith("done "))
            {
                MarkTaskAsDone(command);
            }
            else if (command.StartsWith("update "))
            {
                UpdateTaskCommand(command);
            }
            else if (command.StartsWith("delete "))
            {
                DeleteTaskCommand(command);
            }
            else
            {
                Console.WriteLine("Неизвестная команда.");
            }
        }
    }

    private static void PrintHelp()
    {
        Console.WriteLine("Команды:");
        Console.WriteLine("help — выводит список всех доступных команд с кратким описанием");
        Console.WriteLine("profile — выводит данные пользователя");
        Console.WriteLine("add \"текст задачи\" — добавляет новую задачу");
        Console.WriteLine("done - отмечает задачу как выполненную");
        Console.WriteLine("update - изменить задачу");
        Console.WriteLine("delete - удалить задачу");
        Console.WriteLine("view — выводит все задачи");
        Console.WriteLine("exit — выход из программы");
    }

    private static void PrintProfile()
    {
        Console.WriteLine(name + " " + surname + " - " + age);
    }

    private static void AddTaskCommand(string command)
    {
        string[] parts = command.Split(' ', 2);
        var text = parts[1];

        if (index == todos.Length)
            ExpandArrays();

        todos[index] = text;
        todosStatuses[index] = false;
        todosDates[index] = DateTime.Now;

        index++;
        Console.WriteLine("Добавлена задача: " + text);
    }

    private static void PrintListOfTasks()
    {
        Console.WriteLine("Задачи:");
        for (var i = 0; i < index; i++)
            Console.WriteLine($"{i + 1}) {todos[i]} статус:{todosStatuses[i]} {todosDates[i]}");
    }

    private static void ExpandArrays()
    {
        var newSize = todos.Length * 2;
        Array.Resize(ref todos, newSize);
        Array.Resize(ref todosStatuses, newSize);
        Array.Resize(ref todosDates, newSize);
    }

    private static void MarkTaskAsDone(string command)
    {
        string[] parts = command.Split(' ');
        var taskIndex = int.Parse(parts[1]) - 1;

        todosStatuses[taskIndex] = true;
        todosDates[taskIndex] = DateTime.Now;

        Console.WriteLine($"Задача {taskIndex + 1} отмечена как выполненная.");
    }

    private static void UpdateTaskCommand(string command)
    {
        string[] parts = command.Split(' ', 3);
        var taskIndex = int.Parse(parts[1]) - 1;

        var newText = parts[2];
        todos[taskIndex] = newText;
        todosDates[taskIndex] = DateTime.Now;
        Console.WriteLine($"Задача {taskIndex + 1} обновлена.");
    }

    private static void DeleteTaskCommand(string command)
    {
        string[] parts = command.Split(' ');
        var taskIndex = int.Parse(parts[1]) - 1;

        for (var i = taskIndex; i < index - 1; i++)
        {
            todos[i] = todos[i + 1];
            todosStatuses[i] = todosStatuses[i + 1];
            todosDates[i] = todosDates[i + 1];
        }

        index--;
        Console.WriteLine($"Задача {taskIndex + 1} удалена.");
    }
}