namespace TodoList
{
    class Program
    {

        public static void Main()
        {
            Console.WriteLine("Работу выполнили: Вдовиченко и Кравец");

            Console.Write("Введите ваше имя: ");
            string userName = Console.ReadLine();
            Console.Write("Введите вашу фамилию: ");
            string userSurname = Console.ReadLine();
            Console.Write("Введите ваш год рождения: ");
            int birthYear = int.Parse(Console.ReadLine());

            int userAge = DateTime.Now.Year - birthYear;
            Console.WriteLine($"Добавлен пользователь {userName} {userSurname}, возраст - {userAge}");

            string[] todos = new string[2];
            bool[] statuses = new bool[2];
            DateTime[] dates = new DateTime[2];
            int taskCount = 0;

            while (true)
            {
                Console.Write("\nВведите команду: ");
                string command = Console.ReadLine();

                if (command.StartsWith("add "))
                    AddTask(command, ref todos, ref statuses, ref dates, ref taskCount);
                else if (command.StartsWith("view"))
                    ViewTasks(command, todos, statuses, dates, taskCount);
                else if (command == "help")
                    ShowHelp();
                else if (command == "profile")
                    Console.WriteLine($"{userName} {userSurname} — {userAge} лет");
                else if (command.StartsWith("done "))
                    MarkTaskDone(command, statuses, dates);
                else if (command.StartsWith("delete "))
                    DeleteTask(command, ref todos, ref statuses, ref dates, ref taskCount);
                else if (command.StartsWith("update "))
                    UpdateTask(command, todos, dates);
                else if (command == "exit")
                {
                    Console.WriteLine("Выход из программы.");
                    break;
                }
            }
        }

        private static void ShowHelp()
        {
            Console.WriteLine("\nКоманды:");
            Console.WriteLine("add <текст> — добавить задачу");
            Console.WriteLine("view — показать задачи");
            Console.WriteLine("done <номер> — отметить выполненной");
            Console.WriteLine("delete <номер> — удалить задачу");
            Console.WriteLine("update <номер> <новый текст> — изменить текст");
            Console.WriteLine("profile — профиль пользователя");
            Console.WriteLine("exit — выход");
        }

        private static void AddTask(string command, ref string[] todos, ref bool[] statuses, ref DateTime[] dates, ref int count)
        {
            bool isMultiline = command.Contains("--multiline") || command.Contains("-m");

            if (isMultiline)
            {
                Console.WriteLine("Введите строки задачи (каждая с префиксом '>'). Для завершения введите '!end':");
                System.Collections.Generic.List<string> lines = new System.Collections.Generic.List<string>();

                while (true)
                {
                    Console.Write("> ");
                    string line = Console.ReadLine();

                    if (line == "!end")
                        break;

                    if (!string.IsNullOrWhiteSpace(line))
                        lines.Add(line);
                }

                if (lines.Count == 0)
                {
                    Console.WriteLine("Задача не добавлена: текст пуст.");
                    return;
                }

                string text = string.Join("\n", lines);

                if (count == todos.Length)
                    ExpandArrays(ref todos, ref statuses, ref dates);

                todos[count] = text;
                statuses[count] = false;
                dates[count] = DateTime.Now;
                count++;

                Console.WriteLine($"Добавлена многострочная задача ({lines.Count} строк)");
            }
            else
            {
                string text = command.Substring(4).Trim();

                if (string.IsNullOrWhiteSpace(text))
                {
                    Console.WriteLine("Ошибка: текст задачи не может быть пустым.");
                    return;
                }

                if (count == todos.Length)
                    ExpandArrays(ref todos, ref statuses, ref dates);

                todos[count] = text;
                statuses[count] = false;
                dates[count] = DateTime.Now;
                count++;

                Console.WriteLine($"Добавлена задача: \"{text}\"");
            }
        }

        private static void ViewTasks(string command, string[] todos, bool[] statuses, DateTime[] dates, int count)
        {
            bool showIndex = command.Contains("--index") || command.Contains("-i");
            bool showStatus = command.Contains("--status") || command.Contains("-s");
            bool showDate = command.Contains("--update-date") || command.Contains("-d");
            bool showAll = command.Contains("--all") || command.Contains("-a");

            if (showAll)
            {
                showIndex = true;
                showStatus = true;
                showDate = true;
            }

            if (count == 0)
            {
                Console.WriteLine("Список задач пуст.");
                return;
            }

            int indexWidth = 5;
            int textWidth = 35;
            int statusWidth = 12;
            int dateWidth = 20;

            if (showIndex) Console.Write("| {0,-5} ", "N");
            Console.Write("| {0,-35} ", "Задача");
            if (showStatus) Console.Write("| {0,-12} ", "Статус");
            if (showDate) Console.Write("| {0,-20} ", "Дата");
            Console.WriteLine("|");

            if (showIndex) Console.Write($"+{new string('-', indexWidth + 2)}");
            Console.Write($"+{new string('-', textWidth + 2)}");
            if (showStatus) Console.Write($"+{new string('-', statusWidth + 2)}");
            if (showDate) Console.Write($"+{new string('-', dateWidth + 2)}");
            Console.WriteLine("+");

            for (int i = 0; i < count; i++)
            {
                if (showIndex)
                    Console.Write($"| {i + 1,-5} ");

                string taskText = todos[i].Length > 30
                    ? todos[i].Substring(0, 30) + "..."
                    : todos[i];
                taskText = taskText.Replace("\n", " ");
                Console.Write($"| {taskText,-35} ");

                if (showStatus)
                    Console.Write($"| {(statuses[i] ? "Сделано" : "Не сделано"),-12} ");
                string date = dates[i].ToString("dd.MM.yyyy HH:mm:ss");
                if (showDate)
                    Console.Write($"| {(date),-20} ");

                Console.WriteLine("|");
            }
        }

        private static void MarkTaskDone(string command, bool[] statuses, DateTime[] dates)
        {
            int index = int.Parse(command.Split(' ')[1]) - 1;
            statuses[index] = true;
            dates[index] = DateTime.Now;
            Console.WriteLine($"Задача #{index + 1} отмечена как выполненная.");
        }

        private static void DeleteTask(string command, ref string[] todos, ref bool[] statuses, ref DateTime[] dates, ref int count)
        {
            int index = int.Parse(command.Split(' ')[1]) - 1;

            for (int i = index; i < count - 1; i++)
            {
                todos[i] = todos[i + 1];
                statuses[i] = statuses[i + 1];
                dates[i] = dates[i + 1];
            }

            count--;
            Console.WriteLine($"Задача #{index + 1} удалена.");
        }

        private static void UpdateTask(string command, string[] todos, DateTime[] dates)
        {
            string[] parts = command.Split(' ', 3);
            int index = int.Parse(parts[1]) - 1;
            todos[index] = parts[2];
            dates[index] = DateTime.Now;
            Console.WriteLine($"Задача #{index + 1} обновлена.");
        }

        private static void ExpandArrays(ref string[] todos, ref bool[] statuses, ref DateTime[] dates)
        {
            int newSize = todos.Length * 2;
            Array.Resize(ref todos, newSize);
            Array.Resize(ref statuses, newSize);
            Array.Resize(ref dates, newSize);
        }
    }
}