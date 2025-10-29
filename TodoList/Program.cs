using System;
using System.Collections.Generic;
using TodoList1;
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
Profile userProfile = new Profile(name, surname, yearOfBirth);
TodoList todolist = new TodoList(initialSize);
Console.WriteLine($"\nДобавлен пользователь: Имя: {name}, Фамилия: {surname}, Возраст: {currentNumber}"); 

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
            todolist.Add(new TodoItem(task));
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
void AddTaskToArray(string task)
{
	todolist.Add(new TodoItem(task));
}
int GetIndexFromCommand(string command)
{
    string[] parts = command.Split(' ');
    if (parts.Length > 1 && int.TryParse(parts[1], out int index))
    {
        return index - 1;
    }
    return -1;
}
void ReadTask(string command)
{
    int index = GetIndexFromCommand(command);
    var item = todolist.GetItem(index);
    if (item != null) 
    {
        Console.WriteLine(item.GetFullInfo());
    }
}
void MarkTaskDone(string command)
{
    int index = GetIndexFromCommand(command);
    var done = todolist.GetItem(index);
    if (done != null)
    {
        Console.WriteLine(done.MarkDone());
    }
}
void DeleteTask(string command)
{
    int index = GetIndexFromCommand(command);
    var delete = todolist.GetItem(index);
    if (delete != null)
    {
        todolist.Delete(index);
    }
}
void UpdateTask(string command)
{
	int index = GetIndexFromCommand(command);
	var update = todolist.GetItem(index);
	if (update != null)
	{
		int firstSpaceIndex = command.IndexOf(' ');
		if (firstSpaceIndex != -1)
		{
			int taskStartIndex = command.IndexOf(' ', firstSpaceIndex + 1);
			if (taskStartIndex != -1)
			{
				string newText = command.Substring(taskStartIndex + 1).Trim(' ', '"');
				update.UpdateText(newText);
				Console.WriteLine($"Задача обновлена: {update.Text}");
			}
			else
			{
				Console.WriteLine("Ошибка: не указан новый текст задачи");
			}
		}
		else
		{
			Console.WriteLine("Ошибка: неверный формат команды");
		}
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
    Console.WriteLine(userProfile.GetInfo());
}
void ViewTask(string command)
{
    bool showIndex = command.Contains("--index") || command.Contains("-i");
    bool showStatus = command.Contains("--status") || command.Contains("-s");
    bool showDate = command.Contains("--date") || command.Contains("-d");
    bool showAll = command.Contains("--all") || command.Contains("-a");
    string flags = ExtractFlags(command);
    showAll = command.Contains("--all") || command.Contains("-a") || flags.Contains("a");
    showIndex = command.Contains("--index") || command.Contains("-i") || flags.Contains("i") || showAll;
    showStatus = command.Contains("--status") || command.Contains("-s") || flags.Contains("s") || showAll;
    showDate = command.Contains("--date") || command.Contains("-d") || flags.Contains("d") || showAll;
	if (!showIndex && !showStatus && !showDate && !showAll)
	{
		if (command.Trim() == "view")
		{
			Console.WriteLine("Список задач:");
			for (int i = 0; i < todolist._count; i++)
			{
				var item = todolist.GetItem(i);
				if (item != null)
				{					
					Console.WriteLine(item.GetShortInfo());
				}
			}
		}
	}
	else
	{
		if (showAll)
		{
			todolist.View(true, true, true);
		}
		else
		{
			todolist.View(showIndex, showStatus, showDate);
		}
    }
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