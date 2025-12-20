using System;

class Program
{
    static void Main()
    {
        TodoList todoList = new TodoList();
        Profile profile = new Profile("Имя", "Фамилия", 2000);

        Console.WriteLine("TodoList (ООП версия). Введите help.");

        while (true)
        {
            Console.Write("> ");
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) continue;

            string[] parts = input.Split(' ', 3);
            string command = parts[0];

            switch (command)
            {
                case "add":
                    if (parts.Length < 2) { Console.WriteLine("add <текст>"); break; }
                    todoList.Add(new TodoItem(parts[1]));
                    break;

                case "done":
                    if (TryGetIndex(parts, out int d)) todoList.GetItem(d)?.MarkDone();
                    break;

                case "update":
                    if (parts.Length < 3) { Console.WriteLine("update <id> <text>"); break; }
                    if (TryGetIndex(parts, out int u)) todoList.GetItem(u)?.UpdateText(parts[2]);
                    break;

                case "view":
                    todoList.View(true, true, true);
                    break;

                case "read":
                    if (TryGetIndex(parts, out int r))
                    {
                        Console.WriteLine("========== ЗАДАЧА ==========");
                        Console.WriteLine(todoList.GetItem(r)?.GetFullInfo());
                        Console.WriteLine("============================");
                    }
                    break;

                case "profile":
                    Console.WriteLine("Профиль пользователя:");
                    Console.WriteLine(profile.GetInfo());
                    break;

                case "help":
                Console.WriteLine("Доступные команды:");
                Console.WriteLine("add <текст>            — добавить задачу");
                Console.WriteLine("add --multiline        — добавить многострочную задачу");
                Console.WriteLine("view                   — показать список задач");
                Console.WriteLine("view --done            — показать выполненные задачи");
                Console.WriteLine("view --undone          — показать невыполненные задачи");
                Console.WriteLine("read <id>              — показать задачу полностью");
                Console.WriteLine("done <id>              — отметить задачу выполненной");
                Console.WriteLine("update <id> <текст>    — изменить текст задачи");
                Console.WriteLine("delete <id>            — удалить задачу");
                Console.WriteLine("profile                — показать профиль пользователя");
                Console.WriteLine("help                   — показать справку");
                Console.WriteLine("exit                   — выход из программы");
                break;
            }
        }
    }

    static bool TryGetIndex(string[] parts, out int index)
        => parts.Length >= 2 && int.TryParse(parts[1], out index);
}


