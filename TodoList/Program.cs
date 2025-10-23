using System;
using System.Collections.Generic;
Console.WriteLine("Работу выполнили Андрей и Роман и Петр");
Console.WriteLine("Введите Имя");
string name = Console.ReadLine();
Console.WriteLine("Введите Фамилию");
string surname = Console.ReadLine();
Console.WriteLine("Введите дату рождения");
int yearOfBirth = Convert.ToInt32(Console.ReadLine());
const int currentYear = 2025;
const int initialSize = 3;
int currentNumber;
currentNumber = currentYear - yearOfBirth;
TodoList todolist = new TodoList(initialSize);
Console.WriteLine($"\nДобавлен пользователь: Имя: {name}, Фамилия: {surname}, Возраст: {currentNumber}"); 
string[] todos = new string[initialSize];
bool[] statuses = new bool[todos.Length];
DateTime[] dates = new DateTime[initialSize];
var todosCount = 0;

while (true)
{
    Console.WriteLine("Введите команду: ");
    string command = Console.ReadLine();
    if (command == "exit")
    {
        Console.WriteLine("До свидания");
        return;
    }
    else if (command.StartsWith("help")) Help(command);
    else if (command.StartsWith("profile")) Profile(command);
    else if (command.StartsWith("view")) ViewTask(command);
    else if (command.StartsWith("add")) AddTask(command);
    else if (command.StartsWith("read")) ReadTask(command);
    else if (command.StartsWith("done")) MarkTaskDone(command);
    else if (command.StartsWith("delete")) DeleteTask(command);
    else if (command.StartsWith("update")) UpdateTask(command);
    else
    {
        Console.WriteLine("Неизвестная команда. Введите help для списка команд.");
    }
}

void AddTask(string command)
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
            TodoList.Add(new TodoItem(task));
        }
        else
        {
            Console.WriteLine("Ошибка: задача не может быть пустой");
        }
    }
}
void AddMultilineTask()
{
    Console.WriteLine("Многострочный режим. Введите задачи (для завершения введите !end):");
    var lines = new List<string>();
    string line;
    while (true)
    {
        Console.Write("> ");
        line = Console.ReadLine();
        if (line == "!end") break;
        if (!string.IsNullOrWhiteSpace(line))
        {
            lines.Add(line);
        }
    }
    foreach (string finalTask in lines)
    {
        if (!string.IsNullOrEmpty(finalTask))
        {
            AddTaskToArray(finalTask);
        }
    }
    Console.WriteLine($"Добавлено {lines.Count} задач(и)");
}
void ReadTask(string command)
{
    var item = todoList.GetItem(index);
    if (item != null) 
    {
        Console.WriteLine(item.GetFullInfo());
    }
}
void MarkTaskDone(string command)
{
    var done = todoList.GetItem(index);
    if (done != null)
    {
        Console.WriteLine(done.MarkDone())
    }
}

void DeleteTask(string command)
{
    var delete = todoList.GetItem(index);
    if (delete != null)
    {
        Console.WriteLine(delete.Delete())
    }
}
void UpdateTask(string command)
{
    var update = todoList.GetItem(index);
    if (update != null)
    {
        Console.WriteLine(update.UpdateText())
    }
}
void Help(string command)
{
    Console.Write(
        "Доступные команды:\n" +
        "help - показать все команды\n" +
        "profile - показать профиль\n" +
        "add \"задача\" - добавить задачу\n" +
        "add --multiline / add -m - многострочный режим добавления\n" +
        "view - показать все задачи\n" +
        "view --index / view -i - показать задачи с индексами\n" +
        "view --status / view -s - показать задачи со статусом\n" +
        "view --date / view -d - показать задачи с датой изменения\n" +
        "view --all или view -a - показать все данные задач\n" +
        "read \"номер\" - просмотреть полный текст задачи\n" +
        "done \"номер\" - отметить задачу выполненной\n" +
        "delete \"номер\" - удалить задачу\n" +
        "update \"номер\" \"новый текст\" - обновить задачу\n" +
        "exit - выйти из программ\n"
    );
}
void Profile(string command)
{
    Console.WriteLine(Profile.GetInfo());
}
void ViewTask(string command)
{
    Console.WriteLine(TodoList.View())
}
string ExtractFlags(string command)
{
    string[] parts = command.Split(' ');
    foreach (string part in parts)
    {
        if (part.StartsWith("-") && !part.StartsWith("--") && part.Length > 1)
        {
            return part.Substring(1);
        }
    }
    return "";
}
void PrintTable(List<string[]> table)
{
    var print = todoList.GetItem(index);
    if (print != null)
    {
        Console.WriteLine(print.PrintTable())
}
int GetTotalWidth(int[] columnWidths)
{
    Console.WriteLine(TodoList.GetTotalWidth())
}