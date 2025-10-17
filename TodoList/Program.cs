using System;
using System.Collections.Generic;
using System.Text;

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

void AddTask(string command)
{
    if (command.StartsWith("add"))
    {
        if (command.Contains("--multiline") || command.Contains("-m"))
        {
            AddMultilineTask();
        }
        else
        {
            string task = command.Substring(4).Trim(' ', '"');
            if (!string.IsNullOrEmpty(task))
            {
                AddTaskToArray(task);
            }
            else
            {
                Console.WriteLine("Ошибка: задача не может быть пустой");
            }
        }
    }
}
void AddTaskToArray(string task)
{
    for (int i = 0; i < todos.Length; i++)
    {
        if (string.IsNullOrEmpty(todos[i]))
        {
            todos[i] = task;
            statuses[i] = false;
            dates[i] = DateTime.Now;
            Console.WriteLine($"Задача добавлена: {task}");
            return;
        }
    }
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
    todos[todos.Length / 2] = task;
    statuses[statuses.Length / 2] = false;
    dates[dates.Length / 2] = DateTime.Now;
    Console.WriteLine("Массив расширен!");
    Console.WriteLine($"Задача добавлена: {task}");
}
void AddMultilineTask()
{
    Console.WriteLine("Многострочный режим. Введите задачи (для завершения введите !end):");
    StringBuilder multilineTask = new StringBuilder();
    string line;
    int lineCount = 0;
    while (true)
    {
        Console.Write("> ");
        line = Console.ReadLine();
        if (line == "!end")
        {
            break;
        }
        if (lineCount > 0)
        {
            multilineTask.AppendLine();
        }
        multilineTask.Append(line);
        lineCount++;
    }
    string finalTask = multilineTask.ToString().Trim();
    if (!string.IsNullOrEmpty(finalTask))
    {
        AddTaskToArray(finalTask);
        Console.WriteLine("Многострочная задача добавлена");
    }
    else
    {
        Console.WriteLine("Ошибка: задача не может быть пустой");
    }
}
void ReadTask(string command)
{
    if (command.StartsWith("read "))
    {
        string numberStr = command.Substring(5).Trim();
        if (int.TryParse(numberStr, out int number) && number > 0 && number <= todos.Length)
        {
            int index = number - 1;
            if (!string.IsNullOrEmpty(todos[index]))
            {
                Console.WriteLine($"\n=== Полная информация о задаче {number} ===");
                Console.WriteLine($"Текст: {todos[index]}");
                Console.WriteLine($"Статус: {(statuses[index] ? "Выполнено" : "Не выполнено")}");
                Console.WriteLine($"Дата изменения: {dates[index]:dd.MM.yyyy HH:mm:ss}");
                Console.WriteLine(new string('=', 50));
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
        Console.WriteLine("Неверный формат команды. Используйте: read <номер>");
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

while (true)
{
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
            else if (command.StartsWith("read"))
            {
                ReadTask(command);
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
        Console.WriteLine("add --multiline / add -m - многострочный режим добавления");
        Console.WriteLine("view - показать все задачи");
        Console.WriteLine("view --index / view -i - показать задачи с индексами");
        Console.WriteLine("view --status / view -s - показать задачи со статусом");
        Console.WriteLine("view --date / view -d - показать задачи с датой изменения");
        Console.WriteLine("view --all или view -a - показать все данные задач");
        Console.WriteLine("read \"номер\" - просмотреть полный текст задачи");
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
        bool showIndex = false;
        bool showStatus = false;
        bool showDate = false;
        bool showAll = false;
        
        if (command.Contains("--all") || command.Contains("-a"))
        {
            showAll = true;
        }
        else
        {
            showIndex = command.Contains("--index") || command.Contains("-i");
            showStatus = command.Contains("--status") || command.Contains("-s");
            showDate = command.Contains("--date") || command.Contains("-d");
        }
        if (!showIndex && !showStatus && !showDate && !showAll)
        {
            Console.WriteLine("Список задач:");
            bool hasAnyTasks = false;
            for (int i = 0; i < todos.Length; i++)
            {
                if (!string.IsNullOrEmpty(todos[i]))
                {
                    string status = statuses[i] ? "[Выполнено]" : "[Не выполнено]";
                    Console.WriteLine($"{i + 1}. {todos[i]}-{status}-{dates[i]:dd.MM.yyyy}");
                    hasAnyTasks = true;
                }
            }
            if (!hasAnyTasks)
            {       
                Console.WriteLine("Задач нет!");
            }
        else
        {
            var table = new List<string[]>();
            var headers = new List<string>();
            
            if (showAll || showIndex) headers.Add("№");
            headers.Add("Задача");
            if (showAll || showStatus) headers.Add("Статус");
            if (showAll || showDate) headers.Add("Дата изменения");
            
            table.Add(headers.ToArray());
            
            bool hasTasksInTable = false;
            for (int i = 0; i < todos.Length; i++)
            {
                if (!string.IsNullOrEmpty(todos[i]))
                {
                    var row = new List<string>();
                    
                    if (showAll || showIndex) row.Add((i + 1).ToString());
                    row.Add(todos[i]);
                    if (showAll || showStatus) row.Add(statuses[i] ? "Выполнено" : "Не выполнено");
                    if (showAll || showDate) row.Add(dates[i].ToString("dd.MM.yyyy HH:mm"));
                    
                    table.Add(row.ToArray());
                    hasTasksInTable = true;
                }
            }
            
            if (!hasTasksInTable)
            {       
                Console.WriteLine("Задач нет!");
            }
            else
            {
                PrintTable(table);
            }
        }
    }
}
void PrintTable(List<string[]> table)
{
    if (table.Count == 0) return;
    int[] columnWidths = new int[table[0].Length];
    for (int i = 0; i < table.Count; i++)
    {
        for (int j = 0; j < table[i].Length; j++)
        {
            if (table[i][j].Length > columnWidths[j])
            {
                columnWidths[j] = table[i][j].Length;
            }
        }
    }
    Console.WriteLine("\n" + new string('-', GetTotalWidth(columnWidths) + 3));
    for (int i = 0; i < table.Count; i++)
    {
        Console.Write("|");
        for (int j = 0; j < table[i].Length; j++)
        {
            Console.Write($" {table[i][j].PadRight(columnWidths[j])} |");
        }
        Console.WriteLine();
        if (i == 0)
        {
            Console.WriteLine(new string('-', GetTotalWidth(columnWidths) + 3));
        }
    }
    Console.WriteLine(new string('-', GetTotalWidth(columnWidths) + 3));
}
int GetTotalWidth(int[] columnWidths)
{
    int total = 0;
    foreach (int width in columnWidths)
    {
        total += width + 2;
    }
    return total - 1;
}
}
