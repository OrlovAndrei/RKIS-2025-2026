using System;

namespace Todolist
{
	class Program
	{
		static TodoList todoList = new TodoList();
		static Profile userProfile = new Profile();

		static void Main()
		{
			bool isValid = true;
			int currentYear = DateTime.Now.Year;

			Console.Write("Работу сделали Приходько и Бочкарёв\n");
			Console.Write("Введите свое имя: ");
			userProfile.FirstName = Console.ReadLine();
			Console.Write("Введите свою фамилию: ");
			userProfile.LastName = Console.ReadLine();
			Console.Write("Введите свой год рождения: ");

			try
			{
				userProfile.BirthYear = int.Parse(Console.ReadLine());
			}
			catch (Exception)
			{
				isValid = false;
			}

			if ((isValid == true) && (userProfile.BirthYear <= currentYear))
			{
				Console.WriteLine($"Добавлен пользователь:{userProfile.GetInfo()}");
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
						ShowProfile();
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
		static void ShowProfile()
		{
			if (string.IsNullOrEmpty(userProfile.FirstName) || string.IsNullOrEmpty(userProfile.LastName))
			{
				Console.WriteLine("Данные пользователя не заполнены");
				return;
			}
			Console.WriteLine(userProfile.GetInfo());
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
				if (parts.Length < 2)
				{
					Console.WriteLine("Ошибка: не указана задача");
					return;
				}
				string task = string.Join(" ", parts, 1, parts.Length - 1);
				if (string.IsNullOrWhiteSpace(task))
				{
					Console.WriteLine("Ошибка: задача не может быть пустой");
					return;
				}
				TodoItem newItem = new TodoItem(task);
				todoList.Add(newItem);
				Console.WriteLine("Задача добавлена");
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
			TodoItem newItem = new TodoItem(task);
			todoList.Add(newItem);
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
			ViewTodosWithFlags(showIndex, showStatus, showDate);
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

				TodoItem item = todoList.GetItem(index);
				Console.WriteLine("=======================================");
				Console.WriteLine(item.GetFullInfo());
				Console.WriteLine("=======================================");
			}
			else Console.WriteLine("Неверный номер задачи");
		}
		static void DoneTodo(string numberStr)
		{
			if (int.TryParse(numberStr, out int number) && number > 0 && number <= todoList.Count)
			{
				int index = number - 1;
				TodoItem item = todoList.GetItem(index);
				item.MarkDone();
				Console.WriteLine($"Задача '{item.Text}' выполненна");
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
				TodoItem item = todoList.GetItem(index);
				string oldTask = item.Text;
				item.UpdateText(newText);
				Console.WriteLine($"Задача '{oldTask}' обновлена на '{newText}'");
			}
			else Console.WriteLine("Ошибка: Неверный номер задачи");
		}
	}
}