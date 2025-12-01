namespace TodoList;

internal class Program
{
    public static void Main()
    {
       Console.WriteLine("Работу  выполнили Лютов и Легатов 3832");
       Console.Write("Введите ваше имя: ");
       var firstName = Console.ReadLine();
       Console.Write("Введите вашу фамилию: ");
       var lastName = Console.ReadLine();

       Console.Write("Введите ваш год рождения: ");
       var year = int.Parse(Console.ReadLine());
       var age = DateTime.Now.Year - year;

       var text = "Добавлен пользователь " + firstName + " " + lastName + ", возраст - " + age;
       Console.WriteLine(text);

       var todos = new string[2];
       var statuses = new bool[2];
       var dates = new DateTime[2];
       var index = 0;

       while (true)
       {
          Console.Write("Введите команду: ");
          var command = Console.ReadLine();

          if (command == "help") HelpCommand();
          else if (command == "profile") ShowProfile(firstName, lastName, age);
          else if (command == "exit") break;
          else if (command.StartsWith("add ")) AddTodo(command, ref todos, ref statuses, ref dates, ref index);
          else if (command.StartsWith("done ")) DoneTodo(command, ref statuses, ref dates);
          else if (command.StartsWith("update ")) UpdateTodo(command, ref todos, ref dates);
          else if (command.StartsWith("delete ")) DeleteTodo(command, ref todos, ref statuses, ref dates, ref index);
          else if (command == "view") ViewTodo(todos, statuses, dates, index);
          else Console.WriteLine("Неизвестная команда.");
       }
    }

    private static void HelpCommand()
    {
       Console.WriteLine("Команды:");
       Console.WriteLine("help — выводит список всех доступных команд с кратким описанием");
       Console.WriteLine("profile — выводит данные пользователя");
       Console.WriteLine("add \"текст\" — добавляет новую задачу");
       Console.WriteLine("add --multiline (-m) — добавить задачу в многострочном режиме");
       Console.WriteLine("done index — отметить задачу выполненной");
       Console.WriteLine("delete index — удалить задачу");
       Console.WriteLine("update index — изменить текст задачи");
       Console.WriteLine("view — выводит все задачи");
       Console.WriteLine("exit — выход из программы");
    }

    private static void ShowProfile(string firstName, string lastName, int age)
    {
       Console.WriteLine(firstName + " " + lastName + ", - " + age);
    }

    private static void AddTodo(string command, ref string[] todos, ref bool[] statuses, ref DateTime[] dates, ref int index)
    {
       if (string.IsNullOrWhiteSpace(command))
       {
          Console.WriteLine("Команда не может быть пустой.");
          return;
       }

       if (command.Contains("--multiline") || command.Contains("-m"))
       {
          AddTodoMultiline(ref todos, ref statuses, ref dates, ref index);
          return;
       }

       var parts = command.Split('"');
       if (parts.Length < 2)
       {
          Console.WriteLine("Неверный формат. Используйте: add \"текст задачи\"");
          return;
       }

       var task = parts[1].Trim();
       if (string.IsNullOrWhiteSpace(task))
       {
          Console.WriteLine("Текст задачи не может быть пустым.");
          return;
       }

       if (index == todos.Length)
       {
          ExpandArrays(ref todos, ref statuses, ref dates);
       }

       todos[index] = task;
       statuses[index] = false;
       dates[index] = DateTime.Now;
       index++;

       Console.WriteLine("Добавлена задача: " + task);
    }

    private static void AddTodoMultiline(ref string[] todos, ref bool[] statuses, ref DateTime[] dates, ref int index)
    {
       Console.WriteLine("Введите текст задачи (для завершения введите !end):");

       var multilineText = "";
       while (true)
       {
          Console.Write("> ");
          var line = Console.ReadLine();

          if (line == null) continue;
          if (line == "!end") break;

          if (!string.IsNullOrEmpty(multilineText)) multilineText += "\n";

          multilineText += line;
       }

       if (string.IsNullOrWhiteSpace(multilineText))
       {
          Console.WriteLine("Текст задачи не может быть пустым.");
          return;
       }

       if (index == todos.Length)
       {
          ExpandArrays(ref todos, ref statuses, ref dates);
       }

       todos[index] = multilineText;
       statuses[index] = false;
       dates[index] = DateTime.Now;
       index++;

       Console.WriteLine("Многострочная задача добавлена");
    }

    private static void DoneTodo(string command, ref bool[] statuses, ref DateTime[] dates)
    {
       var parts = command.Split(' ', 2);
       var index = int.Parse(parts[1]);
       statuses[index] = true;
       dates[index] = DateTime.Now;

       Console.WriteLine("Задача отмечена выполненной");
    }

    private static void UpdateTodo(string command, ref string[] todos, ref DateTime[] dates)
    {
       var parts = command.Split(' ', 3);
       var index = int.Parse(parts[1]);
       var task = parts[2];

       todos[index] = task;
       dates[index] = DateTime.Now;

       Console.WriteLine("Задача обновлена");
    }

    private static void DeleteTodo(string command, ref string[] todos, ref bool[] statuses, ref DateTime[] dates, ref int idx)
    {
       var index = int.Parse(command.Split(' ')[1]);

       for (var i = index; i < index - 1; i++)
       {
          todos[i] = todos[i + 1];
          statuses[i] = statuses[i + 1];
          dates[i] = dates[i + 1];
       }

       idx--;
       Console.WriteLine($"Задача {index} удалена.");
    }

    private static void ViewTodo(string[] todos, bool[] statuses, DateTime[] dates, int index)
    {
       Console.WriteLine("Задачи:");
       for (var i = 0; i < index; i++)
       {
          var todo = todos[i];
          var status = statuses[i];
          var date = dates[i];

          Console.WriteLine($"{i}) {date} - {todo} статус:{status}");
       }
    }

    private static void ExpandArrays(ref string[] todos, ref bool[] statuses, ref DateTime[] dates)
    {
       var newSize = todos.Length * 2;
       Array.Resize(ref todos, newSize);
       Array.Resize(ref statuses, newSize);
       Array.Resize(ref dates, newSize);
    }
}