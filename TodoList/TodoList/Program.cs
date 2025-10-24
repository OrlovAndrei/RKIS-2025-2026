using System;

class Program
{
    static void Main()
    {
        string[] todos = new string[2]; // начальный размер 2 задачи
        int count = 0; // сколько задач реально добавлено

        while (true)
        {
            Console.Write("Введите команду: ");
            string command = Console.ReadLine();


            if (command == "help")
            {
                Console.WriteLine("Доступные команды:");
                Console.WriteLine("help - список команд");
                Console.WriteLine("profile - информация о пользователе");
                Console.WriteLine("add \"текст задачи\" - добавить новую задачу");
                Console.WriteLine("view - показать все задачи");
                Console.WriteLine("exit - выйти из программы");
            }
            else if (command == "profile")
            {
                Console.WriteLine("Введите имя: ");
                string name = Console.ReadLine();

                Console.WriteLine("Введите фамилию: ");
                string surname = Console.ReadLine();

                Console.WriteLine("Введите год рождения: ");
                int year = int.Parse(Console.ReadLine());

                int age = DateTime.Now.Year - year;
                Console.WriteLine($"<Имя>: {name}, <Фамилия>: {surname}, <Возраст>: {age}");
            }
            else if (command.StartsWith("add"))
            {
                // пример команды: add "помыть посуду"
                string[] parts = command.Split(' ', 2);
                if (parts.Length < 2)
                {
                    Console.WriteLine("Ошибка: добавьте текст задачи в кавычках.");
                    continue;
                }

                string task = parts[1].Trim('"');

                // проверяем, хватает ли места
                if (count >= todos.Length)
                {
                    string[] newTodos = new string[todos.Length * 2];
                    for (int i = 0; i < todos.Length; i++)
                        newTodos[i] = todos[i];
                    todos = newTodos;
                }

                todos[count] = task;
                count++;
                Console.WriteLine("Задача добавлена!");
            }
            else if (command == "view")
            {
                Console.WriteLine("Ваши задачи:");
                for (int i = 0; i < count; i++)
                {
                    Console.WriteLine($"[{i + 1}] {todos[i]}");
                }
            }
            else if (command == "exit")
            {
                Console.WriteLine("Выход из программы...");
                break;
            }
            else
            {
                Console.WriteLine("Неизвестная команда. Введите help для справки.");
            }
        }
    }
}