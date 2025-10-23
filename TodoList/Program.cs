using System;

namespace TodoList
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Имя: ");
            string fn = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(fn)) { Console.WriteLine("Имя пустое."); return; }

            Console.Write("Фамилия: ");
            string ln = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(ln)) { Console.WriteLine("Фамилия пустая."); return; }

            Console.Write("Год рождения: ");
            if (!int.TryParse(Console.ReadLine(), out int by)) { Console.WriteLine("Неверный год."); return; }

            Profile profile = new Profile(fn, ln, by);
            TodoList todoList = new TodoList();

            Console.WriteLine(profile.GetInfo());
            Console.WriteLine("help - команды");

            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine().Trim();
                if (string.IsNullOrEmpty(input)) { Console.WriteLine("Пусто."); continue; }

                if (input == "help") ProcessHelp();
                else if (input == "profile") ProcessProfile(profile);
                else if (input.StartsWith("add ")) ProcessAdd(input, todoList);
                else if (input.StartsWith("view")) ProcessView(input, todoList);
                else if (input.StartsWith("read ")) ProcessRead(input, todoList);
                else if (input.StartsWith("done ")) ProcessDone(input, todoList);
                else if (input.StartsWith("delete ")) ProcessDelete(input, todoList);
                else if (input.StartsWith("update ")) ProcessUpdate(input, todoList);
                else if (input == "exit") { ProcessExit(); break; }
                else Console.WriteLine("Неизвестно. help для списка.");
            }
        }

        static void ProcessHelp()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("help - показать список команд");
            Console.WriteLine("profile - показать данные пользователя");
            Console.WriteLine("add \"текст задачи\" - добавить задачу");
            Console.WriteLine("add --multiline (-m) - добавить многострочную задачу (ввод построчно, завершите '!end')");
            Console.WriteLine("view - показать все задачи");
            Console.WriteLine("view --index (-i) - показать с номерами");
            Console.WriteLine("view --status (-s) - показать с статусом (выполнена/не выполнена)");
            Console.WriteLine("view --update-date (-d) - показать с датой последнего изменения (dd.MM.yyyy HH:mm)");
            Console.WriteLine("view --all (-a) - показать все колонки (номер, статус, дата, текст)");
            Console.WriteLine("read <индекс> - показать полный текст задачи, статус и дату последнего изменения");
            Console.WriteLine("done <индекс> - отметить задачу как выполненную");
            Console.WriteLine("delete <индекс> - удалить задачу");
            Console.WriteLine("update <индекс> \"новый текст\" - обновить текст задачи");
            Console.WriteLine("update <индекс> --multiline (-m) - обновить многострочную задачу (ввод построчно, завершите '!end')");
            Console.WriteLine("exit - выйти из программы");
        }

        static void ProcessProfile(Profile profile) => Console.WriteLine(profile.GetInfo());

        static void ProcessAdd(string input, TodoList todoList)
        {
            string cmd = input.Substring(4).Trim();
            string text = (cmd == "-m" || cmd == "--multiline") ? ReadMultiline() : cmd.Trim('\"');
            if (string.IsNullOrEmpty(text)) { Console.WriteLine("Текст пустой."); return; }
            TodoItem item = new TodoItem(text);
            todoList.Add(item);
            Console.WriteLine("Добавлено.");
        }

        static string ReadMultiline()
        {
            Console.WriteLine("Ввод построчно, !end для конца:");
            string res = "";
            while (true) { Console.Write("> "); string l = Console.ReadLine(); if (l == "!end") break; res += l + "\n"; }
            return res.TrimEnd('\n');
        }

        static void ProcessView(string input, TodoList todoList)
        {
            bool idx = false, stat = false, date_ = false;
            foreach (string p in input.Split(' ').Skip(1))
            {
                if (p.Contains('i') || p == "--index") idx = true;
                if (p.Contains('s') || p == "--status") stat = true;
                if (p.Contains('d') || p == "--update-date") date_ = true;
                if (p.Contains('a') || p == "--all") idx = stat = date_ = true;
            }
            todoList.View(idx, stat, date_);
        }

        static void ProcessRead(string input, TodoList todoList)
        {
            string[] parts = input.Split(' ');
            if (parts.Length < 2 || !int.TryParse(parts[1], out int id)) { Console.WriteLine("Неверный индекс."); return; }
            TodoItem item = todoList.GetItem(id);
            if (item == null) Console.WriteLine("Неверный индекс.");
            else Console.WriteLine(item.GetFullInfo());
        }

        static void ProcessDone(string input, TodoList todoList)
        {
            string[] parts = input.Split(' ');
            if (parts.Length < 2 || !int.TryParse(parts[1], out int id)) { Console.WriteLine("Неверный индекс."); return; }
            TodoItem item = todoList.GetItem(id);
            if (item == null) Console.WriteLine("Неверный индекс.");
            else { item.MarkDone(); Console.WriteLine("Готово."); }
        }

        static void ProcessDelete(string input, TodoList todoList)
        {
            string[] parts = input.Split(' ');
            if (parts.Length < 2 || !int.TryParse(parts[1], out int id)) { Console.WriteLine("Неверный индекс."); return; }
            if (!todoList.Delete(id)) Console.WriteLine("Неверный индекс.");
            else Console.WriteLine("Удалено.");
        }

        static void ProcessUpdate(string input, TodoList todoList)
        {
            string[] parts = input.Split(new char[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2 || !int.TryParse(parts[1], out int id)) { Console.WriteLine("Неверно."); return; }
            TodoItem item = todoList.GetItem(id);
            if (item == null) { Console.WriteLine("Неверный индекс."); return; }
            string text = (parts.Length == 2 || parts[2] == "-m" || parts[2] == "--multiline") ? ReadMultiline() : parts[2].Trim('\"');
            if (string.IsNullOrEmpty(text)) { Console.WriteLine("Текст пустой."); return; }
            item.UpdateText(text);
            Console.WriteLine("Обновлено.");
        }

        static void ProcessExit() => Console.WriteLine("Выход.");
    }
}