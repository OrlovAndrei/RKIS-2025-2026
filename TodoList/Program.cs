namespace TodoList;

internal class Program
{
    private static string firstName, lastName;
    private static int age;

    private static string[] todos = new string[2];
    private static bool[] statuses = new bool[2];
    private static DateTime[] dates = new DateTime[2];
    private static int index;

    public static void Main()
    {
        Console.WriteLine("Выполнили курсанты группы 3833 Кучеренко и Мурза");
        AddUser();

        while (true)
        {
            Console.Write("Введите команду: ");
            var command = Console.ReadLine();

            if (command == "help") HelpCommand();
            else if (command == "profile") ShowProfile();
            else if (command == "exit") break;
            else if (command.StartsWith("add ")) AddTodo(command);
            else if (command.StartsWith("done ")) DoneTodo(command);
            else if (command.StartsWith("delete ")) DeleteTodo(command);
            else if (command.StartsWith("update ")) UpdateTodo(command);
            else if (command.StartsWith("view")) ViewTodo(command);
            else if (command.StartsWith("read ")) ReadTodo(command);
            else Console.WriteLine("Неизвестная команда.");
        }
    }

    private static void AddUser()
    {
        Console.Write("Введите ваше имя: ");
        firstName = Console.ReadLine();
        Console.Write("Введите вашу фамилию: ");
        lastName = Console.ReadLine();

        Console.Write("Введите ваш год рождения: ");
        var year = int.Parse(Console.ReadLine());
        age = DateTime.Now.Year - year;

        var text = "Добавлен пользователь " + firstName + " " + lastName + ", возраст - " + age;
        Console.WriteLine(text);
    }

	private static void HelpCommand()
	{
		Console.WriteLine("Команды:");
		Console.WriteLine("help — выводит список всех доступных команд с кратким описанием");
		Console.WriteLine("profile — выводит данные пользователя");
		Console.WriteLine("add \"текст задачи\" — добавляет новую задачу");
		Console.WriteLine("    -m, --multi — добавить многострочную задачу");
		Console.WriteLine("view — выводит все задачи");
		Console.WriteLine("    -a, --all — показать все поля");
		Console.WriteLine("    -i, --index — показать индекс");
		Console.WriteLine("    -s, --status — показать статус");
		Console.WriteLine("    -d, --update-date — показать дату");
		Console.WriteLine("read \"idx\" — вывод задачи без редактирования");
		Console.WriteLine("done \"idx\" — отметить задачу выполненной");
		Console.WriteLine("delete \"idx\" — удалить задачу");
		Console.WriteLine("update \"idx\" — изменить текст задачи");
		Console.WriteLine("exit — выход из программы");
	}

	private static void ShowProfile()
    {
        Console.WriteLine(firstName + " " + lastName + ", - " + age);
    }

    private static void AddTodo(string command)
    {
        var flags = ParseFlags(command);
        bool isMulti = flags.Contains("--multi") || flags.Contains("-m");

        string task = "";
        if (isMulti)
        {
            Console.WriteLine("Введите строки задачи (введите !exit для завершения):");
            List<string> lines = new List<string>();

            while (true)
            {
                Console.Write("> ");
                string line = Console.ReadLine();
                if (line == "!exit") break;
                lines.Add(line);
            }

            task = string.Join("\n", lines);
        }
        else
        {
            task = command.Split("add", 2)[1].Trim();
        }


        if (index == todos.Length)
            ExpandArrays();

        todos[index] = task;
        statuses[index] = false;
        dates[index] = DateTime.Now;

        Console.WriteLine("Добавлена задача: " + index + ") " + task);
        index++;
    }
    private static string[] ParseFlags(string command)
    {
		return command.Split(' ')
		.Where(p => p.StartsWith("--") || p.StartsWith("-"))
		.SelectMany(p =>
		{
			if (p.StartsWith("--"))
				return new[] { p.Trim() };

			return p.Skip(1)
					.Select(ch => "-" + ch);
		})
		.ToArray();
	}

    private static void DoneTodo(string command)
    {
        var parts = command.Split(' ', 2);
        var index = int.Parse(parts[1]);
        statuses[index] = true;
        dates[index] = DateTime.Now;

        Console.WriteLine("Задача " + todos[index] + " отмечена выполненной");
    }

    private static void DeleteTodo(string command)
    {
        var idx = int.Parse(command.Split(' ')[1]);

        for (var i = idx; i < index - 1; i++)
        {
            todos[i] = todos[i + 1];
            statuses[i] = statuses[i + 1];
            dates[i] = dates[i + 1];
        }

        Console.WriteLine($"Задача {idx} удалена.");
		index--;
	}

	private static void UpdateTodo(string command)
    {
        var parts = command.Split(' ', 3);
        var index = int.Parse(parts[1]);
        var task = parts[2];

        todos[index] = task;
        dates[index] = DateTime.Now;

        Console.WriteLine("Задача обновлена");
    }

    private static void ViewTodo(string command)
    {
        var flags = ParseFlags(command);

        bool showAll = flags.Contains("--all") || flags.Contains("-a");
        bool showIndex = flags.Contains("--index") || flags.Contains("-i") || showAll;
        bool showStatus = flags.Contains("--status") || flags.Contains("-s") || showAll;
        bool showDate = flags.Contains("--update-date") || flags.Contains("-d") || showAll;

        int indexWidth = 8;
        int textWidth = 34;
        int statusWidth = 16;
        int dateWidth = 16;

        string headerRow = "Текст задачи".PadRight(textWidth) + "|";
        if (showIndex) headerRow += "Индекс".PadRight(indexWidth) + "|";
        if (showStatus) headerRow += "Статус".PadRight(statusWidth) + "|";
        if (showDate) headerRow += "Дата обновления".PadRight(dateWidth) + "|";

        Console.WriteLine(headerRow);
        Console.WriteLine(new string('-', headerRow.Length));

        for (int i = 0; i < index; i++)
        {
            if (string.IsNullOrEmpty(todos[i])) continue;

            string text = todos[i].Replace("\r", " ").Replace("\n", " ");
            if (text.Length > 30) text = text.Substring(0, 30) + "...";

            string status = statuses[i] ? "выполнена" : "не выполнена";
            string date = dates[i].ToString("yyyy-MM-dd HH:mm");

            string row = text.PadRight(textWidth) + "|";
            if (showIndex) row += i.ToString().PadRight(indexWidth) + "|";
            if (showStatus) row += status.PadRight(statusWidth) + "|";
            if (showDate) row += date.PadRight(dateWidth) + "|";

            Console.WriteLine(row);
        }
    }
    
    private static void ReadTodo(string command)
    {
        var parts = command.Split(' ', 2);
        int idx = int.Parse(parts[1]);

        string status = statuses[idx] ? "выполнена" : "не выполнена";
        string date = dates[idx].ToString("yyyy-MM-dd HH:mm");

        Console.WriteLine($"Полный текст задачи {idx}:");
        Console.WriteLine(todos[idx]);
        Console.WriteLine($"Статус: {status}");
        Console.WriteLine($"Дата последнего изменения: {date}");
    }

    private static void ExpandArrays()
    {
        var newSize = todos.Length * 2;
        Array.Resize(ref todos, newSize);
        Array.Resize(ref statuses, newSize);
        Array.Resize(ref dates, newSize);
    }
}
