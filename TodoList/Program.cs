using System;

public interface ICommand
{
    void Execute();
}
public class AddCommand : ICommand
{
    public bool IsMultiline { get; set; }
    public string Text { get; set; }
    public TodoList TodoList { get; set; }

    public void Execute()
    {
        if (IsMultiline)
        {
            AddTodoMultiline();
        }
        else
        {
            AddTodoSingleLine();
        }
    }

    private void AddTodoSingleLine()
    {
        if (string.IsNullOrWhiteSpace(Text))
        {
            Console.WriteLine("Текст задачи не может быть пустым.");
            return;
        }

        TodoItem newItem = new TodoItem(Text);
        TodoList.Add(newItem);
        Console.WriteLine($"Задача добавлена: {Text} (всего задач: {TodoList.Count})");
    }

    private void AddTodoMultiline()
    {
        Console.WriteLine("Введите текст задачи (для завершения введите !end):");
        
        string multilineText = "";
        while (true)
        {
            Console.Write("> ");
            string line = Console.ReadLine();
            
            if (line == null)
                continue;

            if (line == "!end")
                break;
                
            multilineText += line + "\n";
        }
        
        multilineText = multilineText.Trim();
        if (string.IsNullOrWhiteSpace(multilineText))
        {
            Console.WriteLine("Текст задачи не может быть пустым.");
            return;
        }
        
        TodoItem newItem = new TodoItem(multilineText);
        TodoList.Add(newItem);
        Console.WriteLine($"Многострочная задача добавлена (всего задач: {TodoList.Count})");
    }
}
public class ViewCommand : ICommand
{
    public bool ShowIndex { get; set; }
    public bool ShowStatus { get; set; }
    public bool ShowDate { get; set; }
    public TodoList TodoList { get; set; }

    public void Execute()
    {
        TodoList.View(ShowIndex, ShowStatus, ShowDate);
    }
}
public class DoneCommand : ICommand
{
    public int TaskNumber { get; set; }
    public TodoList TodoList { get; set; }

    public void Execute()
    {
        int taskIndex = TaskNumber - 1;
        try
        {
            TodoItem item = TodoList.GetItem(taskIndex);
            item.MarkDone();
            Console.WriteLine($"Задача '{item.Text}' отмечена как выполненная");
        }
        catch (System.ArgumentOutOfRangeException)
        {
            Console.WriteLine($"Задачи с номером {TaskNumber} не существует.");
        }
    }
}

public class DeleteCommand : ICommand
{
    public int TaskNumber { get; set; }
    public TodoList TodoList { get; set; }

    public void Execute()
    {
        int taskIndex = TaskNumber - 1;
        try
        {
            TodoList.Delete(taskIndex);
            Console.WriteLine($"Задача удалена");
        }
        catch (System.ArgumentOutOfRangeException)
        {
            Console.WriteLine($"Задачи с номером {TaskNumber} не существует.");
        }
    }
}

public class UpdateCommand : ICommand
{
    public int TaskNumber { get; set; }
    public string NewText { get; set; }
    public TodoList TodoList { get; set; }

    public void Execute()
    {
        int taskIndex = TaskNumber - 1;
        try
        {
            TodoItem item = TodoList.GetItem(taskIndex);
            item.UpdateText(NewText);
            Console.WriteLine($"Задача обновлена");
        }
        catch (System.ArgumentOutOfRangeException)
        {
            Console.WriteLine($"Задачи с номером {TaskNumber} не существует.");
        }
    }
}

public class ReadCommand : ICommand
{
    public int TaskNumber { get; set; }
    public TodoList TodoList { get; set; }

    public void Execute()
    {
        int taskIndex = TaskNumber - 1;
        try
        {
            TodoItem item = TodoList.GetItem(taskIndex);
            Console.WriteLine($"=== Задача #{TaskNumber} ===");
            Console.WriteLine(item.GetFullInfo());
        }
        catch (System.ArgumentOutOfRangeException)
        {
            Console.WriteLine($"Задачи с номером {TaskNumber} не существует.");
        }
    }
}

public class ProfileCommand : ICommand
{
    public Profile Profile { get; set; }

    public void Execute()
    {
        Console.WriteLine(Profile.GetInfo());
    }
}

public class HelpCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine(@"СПРАВКА ПО КОМАНДАМ:
    help                    - вывести список команд
    profile                 - показать данные пользователя
    add ""текст""            - добавить задачу
    add --multiline (-m)    - добавить задачу в многострочном режиме
    view                    - показать только текст задач
    view --index (-i)       - показать с индексами
    view --status (-s)      - показать со статусами
    view --update-date (-d) - показать с датами
    view --all (-a)         - показать всю информацию
    read <номер>            - просмотреть полный текст задачи
    done <номер>            - отметить задачу выполненной
    delete <номер>          - удалить задачу
    update <номер> ""текст"" - обновить текст задачи
    exit                    - выйти из программы");
    }
}

public class ExitCommand : ICommand
{
    public void Execute()
    {
        System.Environment.Exit(0);
    }
}
public static class CommandParser
{
    public static ICommand Parse(string inputString, TodoList todoList, Profile profile)
    {
        if (string.IsNullOrWhiteSpace(inputString))
        {
            return new HelpCommand();
        }

        string[] parts = inputString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
        {
            return new HelpCommand();
        }

        string commandName = parts[0].ToLower();

        switch (commandName)
        {
            case "add":
                return ParseAddCommand(inputString, todoList);
            case "view":
                return ParseViewCommand(inputString, todoList);
            case "done":
                return ParseDoneCommand(inputString, todoList);
            case "delete":
                return ParseDeleteCommand(inputString, todoList);
            case "update":
                return ParseUpdateCommand(inputString, todoList);
            case "read":
                return ParseReadCommand(inputString, todoList);
            case "profile":
                return new ProfileCommand { Profile = profile };
            case "help":
                return new HelpCommand();
            case "exit":
                return new ExitCommand();
            default:
                return new HelpCommand();
        }
    }

    private static ICommand ParseAddCommand(string input, TodoList todoList)
    {
        var command = new AddCommand { TodoList = todoList };

        if (input.Contains("--multiline") || input.Contains("-m"))
        {
            command.IsMultiline = true;
            return command;
        }

        string[] parts = input.Split('"');
        if (parts.Length >= 2)
        {
            command.Text = parts[1].Trim();
        }

        return command;
    }

    private static ICommand ParseViewCommand(string input, TodoList todoList)
    {
        var command = new ViewCommand { TodoList = todoList };

        string flags = input.Length > 4 ? input.Substring(4).Trim() : "";

        bool showAll = flags.Contains("-a") || flags.Contains("--all");
        command.ShowIndex = flags.Contains("--index") || flags.Contains("-i") || showAll;
        command.ShowStatus = flags.Contains("--status") || flags.Contains("-s") || showAll;
        command.ShowDate = flags.Contains("--update-date") || flags.Contains("-d") || showAll;

        if (flags.Contains("-") && flags.Length > 1 && !flags.Contains("--"))
        {
            string shortFlags = flags.Replace("-", "").Replace(" ", "");
            command.ShowIndex = command.ShowIndex || shortFlags.Contains("i");
            command.ShowStatus = command.ShowStatus || shortFlags.Contains("s");
            command.ShowDate = command.ShowDate || shortFlags.Contains("d");
            if (shortFlags.Contains("a"))
            {
                command.ShowIndex = true;
                command.ShowStatus = true;
                command.ShowDate = true;
            }
        }

        return command;
    }

    private static ICommand ParseDoneCommand(string input, TodoList todoList)
    {
        var command = new DoneCommand { TodoList = todoList };
        string[] parts = input.Split(' ');
        
        if (parts.Length >= 2 && int.TryParse(parts[1], out int taskNumber))
        {
            command.TaskNumber = taskNumber;
        }

        return command;
    }

    private static ICommand ParseDeleteCommand(string input, TodoList todoList)
    {
        var command = new DeleteCommand { TodoList = todoList };
        string[] parts = input.Split(' ');
        
        if (parts.Length >= 2 && int.TryParse(parts[1], out int taskNumber))
        {
            command.TaskNumber = taskNumber;
        }

        return command;
    }

    private static ICommand ParseUpdateCommand(string input, TodoList todoList)
    {
        var command = new UpdateCommand { TodoList = todoList };
        string[] parts = input.Split('"');
        
        if (parts.Length >= 2)
        {
            command.NewText = parts[1].Trim();
            
            string indexPart = parts[0].Replace("update", "").Trim();
            if (int.TryParse(indexPart, out int taskNumber))
            {
                command.TaskNumber = taskNumber;
            }
        }

        return command;
    }

    private static ICommand ParseReadCommand(string input, TodoList todoList)
    {
        var command = new ReadCommand { TodoList = todoList };
        string[] parts = input.Split(' ');
        
        if (parts.Length >= 2 && int.TryParse(parts[1], out int taskNumber))
        {
            command.TaskNumber = taskNumber;
        }

        return command;
    }
}
class Program
{
    private static TodoList _todoList = new TodoList();
    private static Profile _userProfile;

    static void Main()
    {
        InitializeUserProfile();
        RunTodoApplication();
    }
    static void InitializeUserProfile()
    {
        // Запрос данных
        Console.Write("Введите имя: ");
        string firstName = Console.ReadLine();

        Console.Write("Введите фамилию: ");
        string lastName = Console.ReadLine();

        Console.Write("Введите год рождения: ");
        string yearInput = Console.ReadLine();

        int birthYear;
        if (!int.TryParse(yearInput, out birthYear))
        {
            Console.WriteLine("Неверный формат года. Установлен 2000 год по умолчанию.");
            birthYear = 2000;
        }
        _userProfile = new Profile(firstName, lastName, birthYear);

        int currentYear = DateTime.Now.Year;
        int age = currentYear - birthYear;

        Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст - {age}");
    }
    static void RunTodoApplication()
    {
        Console.WriteLine("Добро пожаловать в TodoList! Введите 'help' для списка команд.");

        while (true)
        {
            string command = Console.ReadLine();
            ICommand command = CommandParser.Parse(input, _todoList, _userProfile);
            command.Execute();
        }
    }
    static void ProcessCommand(string command)
    {
        if (string.IsNullOrWhiteSpace(command))
            return;

        switch (command.Split(' ')[0])
        {
            case "add":
                AddTodo(command);
                break;
            case "done":
                MarkTaskAsDone(command);
                break;
            case "delete":
                DeleteTask(command);
                break;
            case "update":
                UpdateTask(command);
                break;
            case "view":
                string flags = command.Length > 4 ? command.Substring(4).Trim() : "";
                ViewTodos(flags);
                break;
            case "read":
                ReadTask(command);
                break;
            case "help":
                ShowHelp();
                break;
            case "profile":
                ShowProfile();
                break;
            case "exit":
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Неизвестная команда. Введите 'help' для списка команд.");
                break;
        }

    }
    static void ShowHelp()
    {
        Console.WriteLine(@"СПРАВКА ПО КОМАНДАМ:
        help                    - вывести список команд
        profile                 - показать данные пользователя
        add ""текст""            - добавить задачу
        add --multiline (-m)    - добавить задачу в многострочном режиме
        view                    - показать только текст задач
        view --index (-i)       - показать с индексами
        view --status (-s)      - показать со статусами
        view --update-date (-d) - показать с датами
        view --all (-a)         - показать всю информацию
        read <номер>            - просмотреть полный текст задачи
        done <номер>            - отметить задачу выполненной
        delete <номер>          - удалить задачу
        update <номер> ""текст"" - обновить текст задачи
        exit                    - выйти из программы");
    }

    static void ShowProfile()
    {
        Console.WriteLine(_userProfile.GetInfo());
    }

    static void AddTodo(string command)
    {
        if (string.IsNullOrWhiteSpace(command))
        {
            Console.WriteLine("Команда не может быть пустой.");
            return;
        }
        if (command.Contains("--multiline") || command.Contains("-m"))
        {
            AddTodoMultiline();
            return;
        }
        // Извлекаем текст 
        string[] parts = command.Split('"');
        if (parts.Length < 2)
        {
            Console.WriteLine("Неверный формат. Используйте: add \"текст задачи\"");
            return;
        }

        string todoText = parts[1].Trim();
        if (string.IsNullOrWhiteSpace(todoText))
        {
            Console.WriteLine("Текст задачи не может быть пустым.");
            return;
        }

        TodoItem newItem = new TodoItem(todoText);
        _todoList.Add(newItem);

        Console.WriteLine($"Задача добавлена: {todoText} (всего задач: {_todoList.Count})");
    }

    static void AddTodoMultiline()
    {
        Console.WriteLine("Введите текст задачи (для завершения введите !end):");

        string multilineText = "";
        while (true)
        {
            Console.Write("> ");
            string line = Console.ReadLine();

            if (line == null)
                continue;

            if (line == "!end")
                break;

            if (!string.IsNullOrEmpty(multilineText))
                multilineText += "\n";

            multilineText += line;
        }

        if (string.IsNullOrWhiteSpace(multilineText))
        {
            Console.WriteLine("Текст задачи не может быть пустым.");
            return;
        }
        TodoItem newItem = new TodoItem(multilineText);
        _todoList.Add(newItem);

        Console.WriteLine($"Многострочная задача добавлена (всего задач: {_todoList.Count})");
    }
    static void ViewTodos(string flags)
    {
        //Обработка флагов для view
        bool showAll = flags.Contains("-a") || flags.Contains("--all");
        bool showIndex = flags.Contains("--index") || flags.Contains("-i") || showAll;
        bool showStatus = flags.Contains("--status") || flags.Contains("-s") || showAll;
        bool showDate = flags.Contains("--update-date") || flags.Contains("-d") || showAll;

        if (flags.Contains("-") && flags.Length > 1 && !flags.Contains("--"))
        {
            string shortFlags = flags.Replace("-", "").Replace(" ", "");
            showIndex = showIndex || shortFlags.Contains("i");
            showStatus = showStatus || shortFlags.Contains("s");
            showDate = showDate || shortFlags.Contains("d");
            if (shortFlags.Contains("a"))
            {
                showIndex = true;
                showStatus = true;
                showDate = true;
            }
        }
        _todoList.View(showIndex, showStatus, showDate);
    }
    static void MarkTaskAsDone(string command)
    {
        string[] parts = command.Split(' ');
        if (parts.Length < 2 || !int.TryParse(parts[1], out int taskNumber))
        {
            Console.WriteLine("Неверный формат. Используйте: done <номер_задачи>");
            return;
        }

        int taskIndex = taskNumber - 1;
        try
        {
            TodoItem item = _todoList.GetItem(taskIndex);
            item.MarkDone();
            Console.WriteLine($"Задача '{item.Text}' отмечена как выполненная");
        }
        catch (ArgumentOutOfRangeException)
        {
            Console.WriteLine($"Задачи с номером {taskNumber} не существует.");
        }
    }
    static void DeleteTask(string command)
    {
        string[] parts = command.Split(' ');
        if (parts.Length < 2 || !int.TryParse(parts[1], out int taskNumber))
        {
            Console.WriteLine("Неверный формат. Используйте: delete <номер_задачи>");
            return;
        }

        int taskIndex = taskNumber - 1;
        try
        {
            _todoList.Delete(taskIndex);
            Console.WriteLine($"Задача удалена");
        }
        catch (ArgumentOutOfRangeException)
        {
            Console.WriteLine($"Задачи с номером {taskNumber} не существует.");
        }
    }
    static void UpdateTask(string command)
    {
        if (string.IsNullOrWhiteSpace(command))
        {
            Console.WriteLine("Команда не может быть пустой.");
            return;
        }

        string[] parts = command.Split('"');
        if (parts.Length < 2)
        {
            Console.WriteLine("Неверный формат. Используйте: update <номер> \"новый текст\"");
            return;
        }

        string indexPart = parts[0].Replace("update", "").Trim();
        if (!int.TryParse(indexPart, out int taskNumber))
        {
            Console.WriteLine("Неверный номер задачи.");
            return;
        }

        int taskIndex = taskNumber - 1;
        string newText = parts[1].Trim();
        if (string.IsNullOrWhiteSpace(newText))
        {
            Console.WriteLine("Текст задачи не может быть пустым.");
            return;
        }
        try
        {
            TodoItem item = _todoList.GetItem(taskIndex);
            item.UpdateText(newText);
            Console.WriteLine($"Задача обновлена");
        }
        catch (ArgumentOutOfRangeException)
        {
            Console.WriteLine($"Задачи с номером {taskNumber} не существует.");
        }
    }

    static void ReadTask(string command)
    {
        string[] parts = command.Split(' ');
        if (parts.Length < 2 || !int.TryParse(parts[1], out int taskNumber))
        {
            Console.WriteLine("Неверный формат. Используйте: read <номер_задачи>");
            return;
        }

        int taskIndex = taskNumber - 1;
        try
        {
            TodoItem item = _todoList.GetItem(taskIndex);
            Console.WriteLine($"=== Задача #{taskNumber} ===");
            Console.WriteLine(item.GetFullInfo());
        }
        catch (ArgumentOutOfRangeException)
        {
            Console.WriteLine($"Задачи с номером {taskNumber} не существует.");
        }
    }
}