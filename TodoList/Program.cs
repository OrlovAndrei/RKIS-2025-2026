Console.WriteLine("Работу выполнили Андрей и Роман и Петр");
Console.WriteLine("Введите Имя");
string Name = Console.ReadLine();
Console.WriteLine("Введите Фамилию");
string Surname = Console.ReadLine();
Console.WriteLine("Введите дату рождения");
int YearOfBirth = Convert.ToInt32(Console.ReadLine());
const int CurrentYear = 2025;
const int InitialSize = 3;
int CurrentNumber;
CurrentNumber = CurrentYear - YearOfBirth;
Console.WriteLine($"\nДобавлен пользователь: Имя: {Name}, Фамилия: {Surname}, Возраст: {CurrentNumber}"); 
string[] todos = new string[InitialSize];
bool[] statuses = new bool[todos.Length];
DateTime[] dates = new DateTime[InitialSize];

while (true)
{g
    Console.WriteLine("Введите команду: ");
    string command = Console.ReadLine();
    switch (command)
    {
        default:
            if (command.StartsWith("help"))
            {
                Help(command);
            }
            else if (command.StartsWith("profile"))
            {
                Profile(command);
            }
            else if (command.StartsWith("view"))
            {
                ViewTask(command);
            }
            else if (command.StartsWith("add"))
            {
                AddTask(command);
            }
            else if (command.StartsWith("done"))
            {
                MarkTaskDone(command);
            }
            else if (command.StartsWith("delete"))
            {
                DeleteTask(command);
            }
            else if (command.StartsWith("update"))
            {
                UpdateTask(command);
            }
            else
            {
                Console.WriteLine("Неизвестная команда. Введите help для списка команд.");
            }
            break;
        case "exit":
            Console.WriteLine("До свидания");
            return;
    }
}

void Help(string command)
{
    if (command.StartsWith("help"))
    {
        Console.WriteLine("Доступные команды:");
        Console.WriteLine("help - показать все команды");
        Console.WriteLine("profile - показать профиль");
        Console.WriteLine("add \"задача\" - добавить задачу");
        Console.WriteLine("view - показать все задачи");
        Console.WriteLine("done \"номер\" - отметить задачу выполненной");
        Console.WriteLine("delete \"номер\" - удалить задачу");
        Console.WriteLine("update \"номер\" \"новый текст\" - обновить задачу");
        Console.WriteLine("exit - выйти из программgit add Program.csы");
    }
}
void Profile(string command)
{
    if (command.StartsWith("profile"))
    {
        Console.WriteLine($"{Name} {Surname}, {YearOfBirth}");
    }
}
void ViewTask(string command)
{
    if (command.StartsWith("view"))
    {
        Console.WriteLine("Список задач:");
        bool hasTasks = false;
        for (int i = 0; i < todos.Length; i++)
        {
            if (!string.IsNullOrEmpty(todos[i]))
            {
                string status = statuses[i] ? "[Выполнено]" : "[Не выполнено]";
                Console.WriteLine($"{i + 1}. {todos[i]}-{status}-{dates[i]:dd.MM.yyyy}");
                hasTasks = true;
            }
        }
        if (!hasTasks)
        {       
            Console.WriteLine("Задач нет!");
        }
    }
}
void AddTask(string command)
{
    if (command.StartsWith("add"))
    {
        string task = command.Substring(4).Trim(' ', '"');
        for (int i = 0; i < todos.Length; i++)
        {
            if (string.IsNullOrEmpty(todos[i]))
            {
                todos[i] = task;
                statuses[i] = false;
                dates[i] = DateTime.Now;
                Console.WriteLine($"Задача добавлена: {task}");
                break;
            }
            if (i == todos.Length - 1)
            {
                string[] newTodos = new string[todos.Length * 2];
                bool[] newStatuses = new bool[statuses.Length * 2];
                DateTime[] newDates = new DateTime[dates.Length * 2];
                for (int j = 0; j < todos.Length; j++)
                {
                    newTodos[j] = todos[j];
                    newStatuses[j] = statuses[j];
                    newDates[j] = dates[j];
                }
                todos = newTodos;
                statuses = newStatuses;
                dates = newDates;
                Console.WriteLine("Массив расширен!");
                todos[i + 1] = task;
                statuses[i + 1] = false;
                dates[i + 1] = DateTime.Now;
                Console.WriteLine($"Задача добавлена: {task}");
                break;
            }
        }
    }
}
void MarkTaskDone(string command)
{
    if (command.StartsWith("done "))
    {
        string numberStr = command.Substring(5).Trim();
        if (int.TryParse(numberStr, out int number) && number > 0 && number <= todos.Length)
        {
            int index = number - 1;
            if (!string.IsNullOrEmpty(todos[index]))
            {
                statuses[index] = true;
                dates[index] = DateTime.Now;
                Console.WriteLine($"Задача {number} отмечена как выполненная");
            }
            else
            {
                Console.WriteLine($"Задача {number} не существует");
            }
        }
        else
        {
            Console.WriteLine("Неверный номер задачи");
        }
    }
}
void DeleteTask(string command)
{
    if (command.StartsWith("delete "))
    {
        string numberStr = command.Substring(7).Trim();
        if (int.TryParse(numberStr, out int number) && number > 0 && number <= todos.Length)
        {
            int index = number - 1;
            if (!string.IsNullOrEmpty(todos[index]))
            {
                for (int i = index; i < todos.Length - 1; i++)
                {
                    todos[i] = todos[i + 1];
                    statuses[i] = statuses[i + 1];
                    dates[i] = dates[i + 1];
                }
                todos[todos.Length - 1] = null;
                statuses[todos.Length - 1] = false;
                dates[todos.Length - 1] = DateTime.MinValue;
                Console.WriteLine($"Задача {number} удалена");
            }
            else
            {
                Console.WriteLine($"Задача {number} не существует");
            }
        }
        else
        {
            Console.WriteLine("Неверный номер задачи");
        }
    }
}
void UpdateTask(string command)
{
    if (command.StartsWith("update "))
    {
        string rest = command.Substring(7).Trim();
        int spaceIndex = rest.IndexOf(' ');
        if (spaceIndex > 0)
        {
            string numberStr = rest.Substring(0, spaceIndex);
            string newText = rest.Substring(spaceIndex + 1).Trim(' ', '"');
                    
            if (int.TryParse(numberStr, out int number) && number > 0 && number <= todos.Length)
            {
                int index = number - 1;
                if (!string.IsNullOrEmpty(todos[index]))
                {
                    todos[index] = newText;
                    dates[index] = DateTime.Now;
                    Console.WriteLine($"Задача {number} обновлена: {newText}");
                }
                else
                {
                    Console.WriteLine($"Задача {number} не существует");
                }
            }
            else
            {
                Console.WriteLine("Неверный номер задачи");
            }
        }
        else
        {
            Console.WriteLine("Неверный формат команды. Используйте: update <номер> \"новый текст\"");
        }
    }
    else
    {
        Console.WriteLine("Неизвестная команда. Введите 'help' для списка команд.");
    }
}
