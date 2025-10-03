Console.WriteLine("Работу выполнили Андрей и Роман");
Console.WriteLine("Введите Имя");
string name = Console.ReadLine();
Console.WriteLine("Введите Фамилию");
string surname = Console.ReadLine();
Console.WriteLine("Введите дату рождения");
int date = Convert.ToInt32(Console.ReadLine());
int year1 = 2025;
int finalnumber;
finalnumber = year1 - date;
Console.WriteLine("Добавлен пользователь"); Console.WriteLine(name); Console.WriteLine(surname); Console.WriteLine(finalnumber);

string[] todos = new string[3];
while (true)
{
    Console.WriteLine("Введите команду: ");
    string command = Console.ReadLine();
    if (command == "help")
    {
        Console.WriteLine("Доступные команды:");
        Console.WriteLine("help - показать все команды");
        Console.WriteLine("profile - показать профиль");
        Console.WriteLine("add \"задача\" - добавить задачу");
        Console.WriteLine("view - показать все задачи");
        Console.WriteLine("exit - выйти из программы");
    }
    if (command == "profile")
    {
        string name1 = name;
        string surname1 = surname;
        int year = date;

        Console.WriteLine($"{name1} {surname1}, {year}");
    }

    if (command.StartsWith("add"))
    {
        string task = command.Substring(4).Trim().Trim('"');
        for (int i = 0; i < todos.Length; i++)
        {
            if (todos[i] == null)
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
    if (command == "view")
    {
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
    }
    if (command == "exit")
    {
        Console.WriteLine("До свидания!");
        break;
    }
}