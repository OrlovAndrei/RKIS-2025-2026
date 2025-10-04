Console.WriteLine("Работу выполнили Андрей и Роман и Петр");
Console.WriteLine("Введите Имя");
string Name = Console.ReadLine();
Console.WriteLine("Введите Фамилию");
string Surname = Console.ReadLine();
Console.WriteLine("Введите дату рождения");
int YearOfBirth = Convert.ToInt32(Console.ReadLine());
int CurrentYear = 2025;
int CurrentNumber;
CurrentNumber = CurrentYear - YearOfBirth;
Console.WriteLine($"\nДобавлен пользователь: Имя: {Name}, Фамилия: {Surname}, Возраст: {CurrentNumber}"); 
string[] todos = new string[3];
while (true)
{
    Console.WriteLine("Введите команду: ");
    string command = Console.ReadLine();
    switch (command)
    {
        case "help":
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("help - показать все команды");
            Console.WriteLine("profile - показать профиль");
            Console.WriteLine("add \"задача\" - добавить задачу");
            Console.WriteLine("view - показать все задачи");
            Console.WriteLine("exit - выйти из программы");
            break;
        case "profile":
            Console.WriteLine($"{Name} {Surname}, {YearOfBirth}");
            break;
        case "view":
            Console.WriteLine("Список задач:");
            bool hasTasks = false;
            for (int i = 0; i < todos.Length; i++)
            {
                if (!string.IsNullOrEmpty(todos[i]))
                {
                    Console.WriteLine($"{i + 1}. {todos[i]}");
                    hasTasks = true;
                }
            }
            if (!hasTasks)
            {       
                Console.WriteLine("Задач нет!");
            }
            break;
        case "exit":
            Console.WriteLine("До свидания");
            return;
            if (command.StartsWith("add"))
            {
                string task = command.Substring(4).Trim(' ', '"');
                for (int i = 0; i < todos.Length; i++)
                {
                    if (string.IsNullOrEmpty(todos[i]))
                    {
                        todos[i] = task;
                        Console.WriteLine($"Задача добавлена: {task}");
                        break;
                    }
                    if (i == todos.Length - 1)
                    {
                        string[] newTodos = new string[todos.Length * 2];
                        for (int j = 0; j < todos.Length; j++)
                    {
                        newTodos[j] = todos[j];
                    }
                    todos = newTodos;
                    Console.WriteLine("Массив расширен!");
                    todos[i + 1] = task;
                    Console.WriteLine($"Задача добавлена: {task}");
                    break;
                    }
                }
               }
            if (command == "exit")
        {
        Console.WriteLine("До свидания!");
        break;
        }
    }
}