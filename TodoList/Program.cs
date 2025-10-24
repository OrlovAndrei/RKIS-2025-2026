using System;
using System.Collections.Generic;

namespace TodoList
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Работу выполнили: Вдовиченко и Кравец");

            Console.Write("Введите ваше имя: ");
            string name = Console.ReadLine();
            Console.Write("Введите вашу фамилию: ");
            string surname = Console.ReadLine();
            Console.Write("Введите год рождения: ");
            int year = int.Parse(Console.ReadLine());

            Profile profile = new Profile(name, surname, year);
            TodoList list = new TodoList();

            Console.WriteLine($"Добавлен пользователь: {profile.GetInfo()}");

            while (true)
            {
                Console.Write("\nВведите команду: ");
                string command = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(command))
                    continue;

                if (command == "exit")
                {
                    Console.WriteLine("Выход из программы.");
                    break;
                }
                else if (command == "help")
                {
                    ShowHelp();
                }
                else if (command.StartsWith("add "))
                {
                    HandleAddCommand(command, list);
                }
                else if (command.StartsWith("done "))
                {
                    if (TryGetIndex(command, out int idx))
                    {
                        list.GetItem(idx).MarkDone();
                        Console.WriteLine($"Задача #{idx + 1} отмечена выполненной.");
                    }
                }
                else if (command.StartsWith("update "))
                {
                    string[] parts = command.Split(' ', 3);
                    if (parts.Length < 3) { Console.WriteLine("Ошибка: укажите новый текст."); continue; }
                    int idx = int.Parse(parts[1]) - 1;
                    list.GetItem(idx).UpdateText(parts[2]);
                    Console.WriteLine($"Задача #{idx + 1} обновлена.");
                }
                else if (command.StartsWith("read "))
                {
                    if (TryGetIndex(command, out int idx))
                        Console.WriteLine(list.GetItem(idx).GetFullInfo());
                }
                else if (command.StartsWith("delete "))
                {
                    if (TryGetIndex(command, out int idx))
                    {
                        list.Delete(idx);
                        Console.WriteLine($"Задача #{idx + 1} удалена.");
                    }
                }
                else if (command.StartsWith("view"))
                {
                    HandleViewCommand(command, list);
                }
                else if (command == "profile")
                {
                    Console.WriteLine(profile.GetInfo());
                }
                else
                {
                    Console.WriteLine("Неизвестная команда. Введите help для списка команд.");
                }
            }
        }

        static void HandleAddCommand(string command, TodoList list)
        {
            bool isMultiline = command.Contains("--multiline") || command.Contains("-m");

            if (isMultiline)
            {
                Console.WriteLine("Введите строки задачи (каждая с префиксом '>'). Для завершения введите '!end':");
                List<string> lines = new List<string>();

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
                list.Add(new TodoItem(text));
                Console.WriteLine($"Добавлена многострочная задача ({lines.Count} строк).");
            }
            else
            {
                string text = command.Substring(4).Trim();
                if (string.IsNullOrWhiteSpace(text))
                {
                    Console.WriteLine("Ошибка: текст задачи не может быть пустым.");
                    return;
                }

                list.Add(new TodoItem(text));
                Console.WriteLine($"Добавлена задача: \"{text}\"");
            }
        }

        static bool TryGetIndex(string command, out int index)
        {
            index = -1;
            string[] parts = command.Split(' ');
            if (parts.Length < 2 || !int.TryParse(parts[1], out int parsed) || parsed <= 0)
            {
                Console.WriteLine("Ошибка: неверный номер задачи.");
                return false;
            }
            index = parsed - 1;
            return true;
        }

        static void HandleViewCommand(string command, TodoList list)
        {
            string[] parts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            bool showIndex = false, showDone = false, showDate = false;

            foreach (string part in parts)
            {
                if (!part.StartsWith("-")) continue;
                foreach (char c in part.Substring(1))
                {
                    switch (c)
                    {
                        case 'a': showIndex = showDone = showDate = true; break;
                        case 'i': showIndex = true; break;
                        case 's': showDone = true; break;
                        case 'd': showDate = true; break;
                    }
                }
            }

            if (list.IsEmpty())
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
            if (showDone) Console.Write("| {0,-12} ", "Статус");
            if (showDate) Console.Write("| {0,-20} ", "Дата");
            Console.WriteLine("|");

            if (showIndex) Console.Write($"+{new string('-', indexWidth + 2)}");
            Console.Write($"+{new string('-', textWidth + 2)}");
            if (showDone) Console.Write($"+{new string('-', statusWidth + 2)}");
            if (showDate) Console.Write($"+{new string('-', dateWidth + 2)}");
            Console.WriteLine("+");

            for (int i = 0; i < list.Count; i++)
            {
                TodoItem item = list.GetItem(i);
                if (showIndex)
                    Console.Write($"| {i + 1,-5} ");

                string taskText = item.Text.Length > 30 ? item.Text.Substring(0, 30) + "..." : item.Text;
                taskText = taskText.Replace("\n", " ");
                Console.Write($"| {taskText,-35} ");

                if (showDone)
                    Console.Write($"| {(item.IsDone ? "Сделано" : "Не сделано"),-12} ");
                if (showDate)
                    Console.Write($"| {item.LastUpdate.ToString("dd.MM.yyyy HH:mm:ss"),-20} ");
                Console.WriteLine("|");
            }
        }

        static void ShowHelp()
        {
            string helpText = """
        Список команд:
        add <текст> — добавить задачу
            (-m) — добавить многострочную задачу
        done <номер> — отметить задачу выполненной
        update <номер> <новый текст> — изменить текст задачи
        delete <номер> — удалить задачу
        view — показать задачи
            (-i) — показать с индексом
            (-s) — показать со статусом
            (-d) — показать с датой
            (-a) — показать все
        read <номер> — показать полное описание задачи
        profile — показать информацию о пользователе
        help — показать список команд
        exit — выйти из программы
    """;
            Console.WriteLine(helpText);




        }
    }
}