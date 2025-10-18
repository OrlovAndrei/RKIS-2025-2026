using System;
using System.Collections.Generic;
using System.Linq;

namespace TodoList
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Шелепов и Кузьменко");

            Console.WriteLine("Введите имя");
            string name = Console.ReadLine();

            Console.WriteLine("Введите фамилию");
            string surname = Console.ReadLine();

            Console.WriteLine("Введите год рождения");
            string birthYearInput = Console.ReadLine();

            if (!int.TryParse(birthYearInput, out int birthYear))
            {
                Console.WriteLine("Ошибка: введите корректный год рождения");
                return;
            }

            int currentYear = DateTime.Now.Year;
            int age = currentYear - birthYear;
            Console.WriteLine($"Добавлен пользователь {name} {surname} Возраст - {age}");

            string[] todos = new string[2];
            bool[] statuses = new bool[2];
            DateTime[] dates = new DateTime[2];
            int count = 0;

            while (true)
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                if (line == null || line.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase)) break;

                var (command, flags, argsLine) = ParseCommand(line);

                switch (command)
                {
                    case "add":
                        AddTask(ref todos, ref statuses, ref dates, ref count, argsLine, flags);
                        break;
                    case "done":
                        MarkTaskDone(statuses, dates, count, argsLine);
                        break;
                    case "delete":
                        DeleteTask(todos, statuses, dates, ref count, argsLine);
                        break;
                    case "update":
                        UpdateTask(todos, dates, count, argsLine);
                        break;
                    case "view":
                        ViewTasks(todos, statuses, dates, count, flags);
                        break;
                    case "read":
                        ReadTask(todos, statuses, dates, count, argsLine);
                        break;
                    case "help":
                        ShowHelp();
                        break;
                    case "profile":
                        ShowProfile(name, surname, birthYear);
                        break;
                    default:
                        Console.WriteLine("Неизвестная команда. Введите 'help' для списка команд.");
                        break;
                }
            }
        }


        static (string command, HashSet<string> flags, string argsLine) ParseCommand(string line)
        {
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) return ("", new HashSet<string>(), "");

            string command = parts[0].ToLowerInvariant();
            var flags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            int i = 1;

            for (; i < parts.Length; i++)
            {
                string token = parts[i];
                if (token.StartsWith("--"))
                    flags.Add(token.Substring(2));
                else if (token.StartsWith("-") && token.Length > 1)
                    foreach (char c in token.Substring(1))
                        flags.Add(c.ToString());
                else break;
            }

            if (flags.Contains("m")) flags.Add("multiline");
            if (flags.Contains("a")) flags.Add("all");
            if (flags.Contains("i")) flags.Add("index");
            if (flags.Contains("s")) flags.Add("status");
            if (flags.Contains("d")) flags.Add("update-date");

            string argsLine = i < parts.Length ? string.Join(' ', parts, i, parts.Length - i) : string.Empty;
            return (command, flags, argsLine);
        }

        static void ShowHelp()
        {
            Console.WriteLine("Команды:");
            Console.WriteLine("profile                         - выводит данные о пользователе");
            Console.WriteLine("add 'текст'                     - добавляет новую задачу (однострочный режим)");
            Console.WriteLine("add --multiline  или add -m     - добавляет новую задачу (многострочный режим). Ввод строк до '!end'");
            Console.WriteLine("done <номер>                    - пометить задачу как выполненную");
            Console.WriteLine("delete <номер>                  - удалить задачу");
            Console.WriteLine("update <номер> <текст>          - обновить задачу");
            Console.WriteLine("read <номер>                    - показать полную информацию о задаче");
            Console.WriteLine("view [флаги]                    - просмотреть список задач (по умолчанию только текст)");
            Console.WriteLine("    --index, -i       показывать индекс задачи");
            Console.WriteLine("    --status, -s      показывать статус (выполнена/не выполнена)");
            Console.WriteLine("    --update-date, -d показывать дату последнего изменения");
            Console.WriteLine("    --all, -a         показывать все столбцы одновременно");
            Console.WriteLine("help                            - показать это сообщение");
            Console.WriteLine("exit                            - выйти из программы");
        }

        static void ShowProfile(string name, string surname, int birthYear)
        {
            Console.WriteLine($"{name} {surname}, {birthYear}");
        }


        static void AddTask(ref string[] todos, ref bool[] statuses, ref DateTime[] dates, ref int count, string line, HashSet<string> flags)
        {
            string text;
            if (flags.Contains("multiline"))
            {
                Console.WriteLine("Многострочный ввод (введите !end для завершения):");
                var lines = new List<string>();
                while (true)
                {
                    string? input = Console.ReadLine();
                    if (input == null || input.Trim() == "!end") break;
                    lines.Add(input);
                }
                text = string.Join("\n", lines);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    Console.WriteLine("Ошибка: не введён текст задачи");
                    return;
                }
                text = line.Trim('"', '\'');
            }

            (todos, statuses, dates) = AddToArrays(todos, statuses, dates, ref count, text);
            Console.WriteLine("Задача добавлена!");
        }

        static (string[], bool[], DateTime[]) AddToArrays(string[] todos, bool[] statuses, DateTime[] dates, ref int count, string text)
        {
            if (count >= todos.Length)
                (todos, statuses, dates) = ExpandArrays(todos, statuses, dates);

            todos[count] = text;
            statuses[count] = false;
            dates[count] = DateTime.Now;
            count++;
            return (todos, statuses, dates);
        }

        static (string[], bool[], DateTime[]) ExpandArrays(string[] todos, bool[] statuses, DateTime[] dates)
        {
            int newSize = todos.Length * 2;
            if (newSize == 0) newSize = 2;

            string[] newTodos = new string[newSize];
            bool[] newStatuses = new bool[newSize];
            DateTime[] newDates = new DateTime[newSize];

            for (int i = 0; i < todos.Length; i++)
            {
                newTodos[i] = todos[i];
                newStatuses[i] = statuses[i];
                newDates[i] = dates[i];
            }

            return (newTodos, newStatuses, newDates);
        }

        static void ReadTask(string[] todos, bool[] statuses, DateTime[] dates, int count, string line)
        {
            if (!int.TryParse(line, out int idx))
            {
                Console.WriteLine("Ошибка: укажите номер задачи");
                return;
            }

            idx--;
            if (idx < 0 || idx >= count)
            {
                Console.WriteLine("Ошибка: некорректный номер задачи");
                return;
            }

            Console.WriteLine("================================");
            Console.WriteLine($"Номер: {idx + 1}");
            Console.WriteLine($"Статус: {(statuses[idx] ? "выполнена" : "не выполнена")}");
            Console.WriteLine($"Дата изменения: {dates[idx]:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine("Текст задачи:");
            Console.WriteLine("--------------------------------");
            Console.WriteLine(todos[idx]);
            Console.WriteLine("================================");
        }

        static void ViewTasks(string[] todos, bool[] statuses, DateTime[] dates, int count, HashSet<string> flags)
        {
            if (count == 0)
            {
                Console.WriteLine("Список задач пуст");
                return;
            }

            bool showIndex = flags.Contains("index") || flags.Contains("all");
            bool showStatus = flags.Contains("status") || flags.Contains("all");
            bool showDate = flags.Contains("update-date") || flags.Contains("all");

            const int textWidth = 30;
            int indexWidth = Math.Max(3, count.ToString().Length + 1);
            int statusWidth = 12;
            int dateWidth = 19;

            var headers = new List<string>();
            if (showIndex) headers.Add("Idx".PadRight(indexWidth));
            if (showStatus) headers.Add("Status".PadRight(statusWidth));
            if (showDate) headers.Add("Updated".PadRight(dateWidth));
            headers.Add("Task".PadRight(textWidth));

            Console.WriteLine(string.Join(" | ", headers));
            Console.WriteLine(new string('-', headers.Sum(h => h.Length + 3)));

            for (int i = 0; i < count; i++)
            {
                var row = new List<string>();
                if (showIndex) row.Add((i + 1).ToString().PadRight(indexWidth));
                if (showStatus)
                {
                    string st = statuses[i] ? "выполнена" : "не выполнена";
                    row.Add(st.PadRight(statusWidth));
                }
                if (showDate)
                    row.Add(dates[i].ToString("yyyy-MM-dd HH:mm:ss").PadRight(dateWidth));

                string text = todos[i] ?? "";
                string shortText = text.Length <= textWidth ? text : text.Substring(0, textWidth - 3) + "...";
                row.Add(shortText.PadRight(textWidth));

                Console.WriteLine(string.Join(" | ", row));
            }
        }

        static void MarkTaskDone(bool[] statuses, DateTime[] dates, int count, string line)
        {
            if (!int.TryParse(line, out int idx))
            {
                Console.WriteLine("Ошибка: укажите номер задачи");
                return;
            }

            idx--;
            if (idx >= 0 && idx < count)
            {
                statuses[idx] = true;
                dates[idx] = DateTime.Now;
                Console.WriteLine("Задача выполнена");
            }
            else Console.WriteLine("Ошибка: некорректный номер задачи");
        }

        static void DeleteTask(string[] todos, bool[] statuses, DateTime[] dates, ref int count, string line)
        {
            if (!int.TryParse(line, out int idx))
            {
                Console.WriteLine("Ошибка: укажите номер задачи");
                return;
            }

            idx--;
            if (idx >= 0 && idx < count)
            {
                for (int i = idx; i < count - 1; i++)
                {
                    todos[i] = todos[i + 1];
                    statuses[i] = statuses[i + 1];
                    dates[i] = dates[i + 1];
                }
                count--;
                Console.WriteLine("Задача удалена");
            }
            else Console.WriteLine("Ошибка: некорректный номер задачи");
        }

        static void UpdateTask(string[] todos, DateTime[] dates, int count, string line)
        {
            var parts = line.Split(' ', 2);
            if (parts.Length < 2 || !int.TryParse(parts[0], out int idx))
            {
                Console.WriteLine("Ошибка: укажите номер задачи и текст");
                return;
            }

            idx--;
            if (idx >= 0 && idx < count)
            {
                todos[idx] = parts[1].Trim('"', '\'');
                dates[idx] = DateTime.Now;
                Console.WriteLine("Задача обновлена");
            }
            else Console.WriteLine("Ошибка: некорректный номер задачи");
        }
    }
}
