using System;

namespace Todolist
{
	public class TodoItem
	{
		public string Text { get; private set; }
		public bool IsDone { get; private set; }
		public DateTime LastUpdate { get; private set; }

		public TodoItem(string text)
		{
			Text = text;
			IsDone = false;
			LastUpdate = DateTime.Now;
		}
		public void MarkDone()
		{
			IsDone = true;
			LastUpdate = DateTime.Now;
		}
		public void UpdateText(string newText)
		{
			Text = newText;
			LastUpdate = DateTime.Now;
		}
		public string GetShortInfo()
		{
			string shortText = Text.Length > 30 ? Text.Substring(0, 27) + "..." : Text;
			string status = IsDone ? "Выполнена" : "Не выполнена";
			string date = LastUpdate.ToString("dd.MM.yyyy HH:mm");

			return $"{shortText,-30} | {status,-12} | {date}";
		}

		public string GetFullInfo()
		{
			string status = IsDone ? "Выполнена" : "Не выполнена";
			string date = LastUpdate.ToString("dd.MM.yyyy HH:mm");

			return $"Задача: {Text}\nСтатус: {status}\nПоследнее изменение: {date}";
		}
	}
	public class TodoList
	{
		private TodoItem[] items;
		private int count;

		public TodoList(int initialCapacity = 2)
		{
			items = new TodoItem[initialCapacity];
			count = 0;
		}
		public void Add(TodoItem item)
		{
			if (count >= items.Length)
			{
				IncreaseArray(item);
			}
			items[count] = item;
			count++;
		}

		public void Delete(int index)
		{
			if (index < 0 || index >= count)
				throw new ArgumentOutOfRangeException(nameof(index), "Неверный индекс");

			for (int i = index; i < count - 1; i++)
			{
				items[i] = items[i + 1];
			}
			items[count - 1] = null;
			count--;
		}

		public void View(bool showIndex, bool showStatus, bool showDate)
		{
			if (count == 0)
			{
				Console.WriteLine("Список пуст");
				return;
			}
			string header = "";
			if (showIndex) header += "Индекс".PadRight(8);
			if (showStatus) header += "Статус".PadRight(12);
			if (showDate) header += "Дата изменения".PadRight(20);
			header += "Задача";

			Console.WriteLine(header);
			Console.WriteLine(new string('-', header.Length));

			for (int i = 0; i < count; i++)
			{
				string line = "";

				if (showIndex) line += $"{i + 1}".PadRight(8);

				if (showStatus)
				{
					string status = items[i].IsDone ? "Сделано" : "Не сделано";
					line += status.PadRight(12);
				}

				if (showDate)
				{
					string date = items[i].LastUpdate.ToString("dd.MM.yyyy HH:mm");
					line += date.PadRight(20);
				}

				string taskText = items[i].Text?.Replace("\n", " ") ?? "";
				if (taskText.Length > 30)
					taskText = taskText.Substring(0, 27) + "...";
				line += taskText;

				Console.WriteLine(line);
			}
		}

		public TodoItem GetItem(int index)
		{
			if (index < 0 || index >= count)
				throw new ArgumentOutOfRangeException(nameof(index), "Неверный индекс");

			return items[index];
		}
		public int Count => count;

		private void IncreaseArray(TodoItem item)
		{
			int newSize = items.Length * 2;
			TodoItem[] newArray = new TodoItem[newSize];

			Array.Copy(items, newArray, items.Length);
			items = newArray;

			Console.WriteLine($"Массив расширен до {newSize} элементов");
		}
	}

	class Program
    {
		static TodoList todoList = new TodoList();
        static string name = "";
        static string secondName = "";
        static int birthYear = 0;

        static void Main()
        {
            bool isValid = true;
            int currentYear = DateTime.Now.Year;

            Console.Write("Работу сделали Приходько и Бочкарёв\n");
            Console.Write("Введите свое имя: ");
            string? name = Console.ReadLine();
            Console.Write("Введите свою фамилию: ");
            string? secondName = Console.ReadLine();
            Console.Write("Введите свой год рождения: ");
            int birthYear = 0;

            try
            {
                birthYear = int.Parse(Console.ReadLine());
            }
            catch (Exception)
            {
                isValid = false;
            }

            if ((isValid == true) && (birthYear <= currentYear))
            {
                int age = currentYear - birthYear;
                Console.WriteLine($"Добавлен пользователь:{name},{secondName},возраст - {age}");
            }

            else Console.WriteLine("Неверно введен год рождения");

            Console.WriteLine("Добро пожаловать в программу");
            Console.WriteLine("Введите 'help' для списка команд");
            while (true)
            {
                Console.WriteLine("=-=-=-=-=-=-=-=");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                string[] parts = input.Split(' ');
                string command = parts[0].ToLower();

                switch (command)
                {
                    case "help":
                        ShowHelp();
                        break;
                    case "profile":
                        ShowProfile(name, secondName, birthYear);
                        break;
                    case "add":
                        ProcessAddCommand(parts);
                        break;
                    case "view":
                        ProcessViewCommand(parts);
                        break;
                    case "read":
                        ProcessReadCommand(parts);
                        break;
                    case "done":
                        if (parts.Length < 2) Console.WriteLine("Ошибка: не указан номер задачи");
                        else DoneTodo(parts[1]);
                        break;
                    case "delete":
                        if (parts.Length < 2) Console.WriteLine("Ошибка: не указан номер задачи");
                        else DeleteTodo(parts[1]);
                        break;
                    case "update":
                        if (parts.Length < 3) Console.WriteLine("Ошибка: не указан номер задачи");
                        else
                        {
                            string newText = string.Join(" ", parts, 2, parts.Length - 2);
                            UpdateTodo(parts[1], newText);
                        }
                        break;
                    case "exit":
                        Console.WriteLine("Выход из программы");
                        return;
                    default:
                        Console.WriteLine($"Неизвестная команда: {command}");
                        break;
                }
            }
        }
        static void ShowHelp()
        {
			var t = """
				Доступные команды
				help - вывести список команд
				profile - показать данные пользователя
				add - добавить задачу (однострочный режим)
				   --multiline или -m - добавить задачу (многострочный режим)
				view - показать только текст задачи
				   --index или -i - показать с индексами
				   --status или -s - показать со статусами
				   --update-date или -d - показать дату последнего изменения
				   --all или -a - показать все данные
				read <номер> - просмотреть полный текст задачи
				done <номер> - отметить задачу как выполненную
				delete <номер> - удалить задачу
				update <номер> \"новый текст\" - обновить текст задачи
				exit - выход из программы
				""";
			Console.WriteLine(t);
        }
        static void ShowProfile(string? name, string? secondName, int birthYear)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(secondName))
            {
                Console.WriteLine("Данные пользователя не заполнены");
                return;
            }

            Console.WriteLine($"Имя: {name}");
            Console.WriteLine($"Фамилия: {secondName}");
            Console.WriteLine($"Год рождения: {birthYear}");
        }
        static void ProcessAddCommand(string[] parts)
        {
            bool multilineMode = false;
            for (int i = 1; i < parts.Length; i++)
            {
                if (!string.IsNullOrEmpty(parts[i]) &&
                    (parts[i] == "--multiline" || parts[i] == "-m"))
                {
                    multilineMode = true;
                    break;
                }
            }
			if (multilineMode) AddTodoMultiline();
            else
            {
                if (parts.Length < 2) Console.WriteLine("Ошибка: не указана задача");
                else
                {
                    string task = string.Join(" ", parts, 1, parts.Length - 1);
                    AddTodo(task);
                }
            }
        }
        static void AddTodoMultiline()
        {
            Console.WriteLine("Введите задачу построчно. Для завершения введите '!end':");
			string[] lines = new string[100];
			int lineCount = 0;

            while (true)
            {
                Console.Write("> ");
                string line = Console.ReadLine();
                if (string.IsNullOrEmpty(line)) continue;

                if (line == "!end") break;

				if (lineCount >= lines.Length)
				{
					Console.WriteLine("Достигнут лимит строк (100). Завершите ввод");
					break;
				}
				lines[lineCount] = line;
				lineCount++;
            }
            if (lineCount == 0)
            {
                Console.WriteLine("Задача не была добавлена - пустой ввод");
                return;
            }
			string task = "";
			for (int i = 0; i < lineCount; i++)
			{
				task += lines[i];
				if (i < lineCount - 1)
				{
					task += "\n";
				}
			}
			todoList.Add(new TodoItem(task));
            Console.WriteLine("Многострочная задача добавлена");
        }
        static void ProcessViewCommand(string[] parts)
        {
            bool showIndex = false;
            bool showStatus = false;
            bool showDate = false;
            bool showAll = false;
            for (int i = 1; i < parts.Length; i++)
            {
                string flag = parts[i];
                if (flag == "--all" || flag == "-a") showAll = true;
                else if (flag == "--index" || flag == "-i") showIndex = true;
                else if (flag == "--status" || flag == "-s") showStatus = true;
                else if (flag == "--update-date" || flag == "-d") showDate = true;
                else if (flag.StartsWith("-") && flag.Length > 1 && !flag.StartsWith("--"))
                {
                    foreach (char c in flag.Substring(1))
                    {
                        switch (c)
                        {
                            case 'i': showIndex = true; break;
                            case 's': showStatus = true; break;
                            case 'd': showDate = true; break;
                            case 'a': showAll = true; break;
                        }
                    }
                }
            }
            if (showAll)
            {
                showIndex = true;
                showStatus = true;
                showDate = true;
            }
            ViewTodosWithFlags (showIndex, showStatus, showDate);
        }
        static void ViewTodosWithFlags(bool showIndex, bool showStatus, bool showDate)
        {
			todoList.View(showIndex, showStatus, showDate);
		}
        static void ProcessReadCommand(string[] parts)
        {
            if (parts.Length < 2)
            {
                Console.WriteLine("Ошибка: не указан номер задачи");
                return;
            }
            string? numberStr = parts[1];
            if (string.IsNullOrEmpty(numberStr))
            {
                Console.WriteLine("Ошибка: номер задачи не может быть пустым");
                return;
            }
            ReadTodo(numberStr);
        }
        static void ReadTodo(string numberStr)
        {
            if (int.TryParse(numberStr, out int number) && number > 0 && number <= todoList.Count)
            {
                int index = number - 1;

                Console.WriteLine("=======================================");
				Console.WriteLine(todoList.GetItem(index).GetFullInfo());
                Console.WriteLine("=======================================");
            }
            else Console.WriteLine("Неверный номер задачи");
        }

        static void AddTodo(string task)
        {
            if (string.IsNullOrWhiteSpace(task))
            {
                Console.WriteLine("Ошибка: задача не может быть пустой");
                return;
            }
			todoList.Add(new TodoItem(task));
			Console.WriteLine("Задача добавлена");
        }
        static void DoneTodo(string numberStr)
        {
            if (int.TryParse(numberStr, out int number) && number > 0 && number <= todoList.Count)
            {
                int index = number - 1;
				todoList.GetItem(index).MarkDone();
                Console.WriteLine($"Задача '{todoList.GetItem(index).Text}' выполненна");
            }
            else Console.WriteLine("Неверный номер задачи");
        }
        static void DeleteTodo(string numberStr)
        {
            if (int.TryParse(numberStr, out int number) && number > 0 && number <= todoList.Count)
            {
                int index = number - 1;
                string deletedTask = todoList.GetItem(index).Text;
				todoList.Delete(index);
                Console.WriteLine($"Задача '{deletedTask}' удалена");
            }
            else Console.WriteLine("Неверный номер задачи");
        }
        static void UpdateTodo(string numberStr, string newText)
        {
            if (string.IsNullOrWhiteSpace(newText))
            {
                Console.WriteLine("Ошибка: новый текст не может быть пустым");
                return;
            }
            if (int.TryParse(numberStr, out int number) && number > 0 && number <= todoList.Count)
            {
                int index = number - 1;
                string oldTask = todoList.GetItem(index).Text;
				todoList.GetItem(index).UpdateText(newText);
                Console.WriteLine($"Задача '{oldTask}' обновлена на '{newText}'");
            }
            else Console.WriteLine("Неверный номер задачи");
        }
    }
}