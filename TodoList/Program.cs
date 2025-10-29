using System;

namespace TodoList
{
    public interface ICommand
    {
        void Execute();
    }
        static void Main(string[] args)
        {
            Console.Write("Имя: ");
            string firstName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(firstName)) { Console.WriteLine("Имя пустое."); return; }

            Console.Write("Фамилия: ");
            string lastName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(lastName)) { Console.WriteLine("Фамилия пустая."); return; }

            Console.Write("Год рождения: ");
            if (!int.TryParse(Console.ReadLine(), out int birthYear)) { Console.WriteLine("Неверный год."); return; }

            Profile profile = new Profile(firstName, lastName, birthYear);
            TodoList todoList = new TodoList();

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

        private static void ProcessHelp()
        {
            Console.WriteLine("""
            Доступные команды:
            help - показать список команд
            profile - показать данные пользователя
            add \"текст задачи\" - добавить задачу
            add --multiline (-m) - добавить многострочную задачу (ввод построчно, завершите '!end')
            view - показать все задачи
            view --index (-i) - показать с номерами
            view --status (-s) - показать с статусом (выполнена/не выполнена)
            view --update-date (-d) - показать с датой последнего изменения (dd.MM.yyyy HH:mm)
            view --all (-a) - показать все колонки (номер, статус, дата, текст)
            read <индекс> - показать полный текст задачи, статус и дату последнего изменения
            done <индекс> - отметить задачу как выполненную
            delete <индекс> - удалить задачу
            update <индекс> \"новый текст\" - обновить текст задачи
            update <индекс> --multiline (-m) - обновить многострочную задачу (ввод построчно, завершите '!end')
            exit - выйти из программы
            """);
        }

        private static void ProcessProfile(Profile profile) => Console.WriteLine(profile.GetInfo());

        private static void ProcessAdd(string input, TodoList todoList)
        {
            string cmd = input.Substring(4).Trim();
            string text;
            if (cmd == "-m" || cmd == "--multiline")
            {
                text = ReadMultiline();
            }
            else
            {
                text = cmd.Trim('\"');
            }
            if (string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine("Текст пустой.");
                return;
            }
            TodoItem item = new TodoItem(text);
            todoList.Add(item);
            Console.WriteLine("Добавлено.");
        }

        private static string ReadMultiline()
        {
            Console.WriteLine("Ввод построчно, !end для конца:");
            string res = "";
            while (true)
            {
                Console.Write("> ");
                string l = Console.ReadLine();
                if (l == "!end") break;
                res += l + "\n";
            }
            return res.TrimEnd('\n');
        }

        private static void ProcessView(string input, TodoList todoList)
        {
            bool showIndex = false, showStatus = false, showDate = false;
            string[] inputParts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string[] flags = new string[inputParts.Length - 1];
            for (int i = 1; i < inputParts.Length; i++)
            {
                flags[i - 1] = inputParts[i];
            }

            foreach (string flag in flags)
            {
                if (flag == "--index" || flag == "-i") showIndex = true;
                else if (flag == "--status" || flag == "-s") showStatus = true;
                else if (flag == "--update-date" || flag == "-d") showDate = true;
                else if (flag == "--all" || flag == "-a") showIndex = showStatus = showDate = true;
            }

            if (flags.Length == 0) showIndex = true;
            todoList.View(showIndex, showStatus, showDate);
        }

        private static void ProcessRead(string input, TodoList todoList)
        {
            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2 || !int.TryParse(parts[1], out int id)) { Console.WriteLine("Неверный индекс."); return; }
            TodoItem item = todoList.GetItem(id);
            if (item == null) Console.WriteLine("Задача с таким индексом не найдена.");
            else Console.WriteLine(item.GetFullInfo());
        }

        private static void ProcessDone(string input, TodoList todoList)
        {
            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2 || !int.TryParse(parts[1], out int id)) { Console.WriteLine("Неверный индекс."); return; }
            TodoItem item = todoList.GetItem(id);
            if (item == null) Console.WriteLine("Задача с таким индексом не найдена.");
            else { item.MarkDone(); Console.WriteLine("Готово."); }
        }

        private static void ProcessDelete(string input, TodoList todoList)
        {
            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2 || !int.TryParse(parts[1], out int id)) { Console.WriteLine("Неверный индекс."); return; }
            if (!todoList.Delete(id)) Console.WriteLine("Задача с таким индексом не найдена.");
            else Console.WriteLine("Удалено.");
        }

        private static void ProcessUpdate(string input, TodoList todoList)
        {
            string[] parts = input.Split(new char[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2 || !int.TryParse(parts[1], out int id))
            {
                Console.WriteLine("Неверно.");
                return;
            }
            TodoItem item = todoList.GetItem(id);
            if (item == null)
            {
                Console.WriteLine("Задача с таким индексом не найдена.");
                return;
            }
            string text;
            if (parts.Length == 2 || parts[2] == "-m" || parts[2] == "--multiline")
            {
                text = ReadMultiline();
            }
            else
            {
                text = parts[2].Trim('\"');
            }
            if (string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine("Текст пустой.");
                return;
            }
            item.UpdateText(text);
            Console.WriteLine("Обновлено.");
        }

        private static void ProcessExit() => Console.WriteLine("Выход.");
    }
}
