namespace TodoList
{
	public class Profile
	{
		public string FirstName { get; private set; }
		public string LastName { get; private set; }
		public int BirthYear { get; private set; }

		public Profile(string firstName, string lastName, int birthYear)
		{
			FirstName = firstName;
			LastName = lastName;
			BirthYear = birthYear;
		}

		public string GetInfo()
		{
			int age = DateTime.Now.Year - BirthYear;
			return $"{FirstName} {LastName}, возраст {age}";
		}
	}

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
			if (!string.IsNullOrEmpty(newText))
			{
				Text = newText;
				LastUpdate = DateTime.Now;
			}
		}

		public string GetFullInfo()
		{
			string statusText = IsDone ? "Выполнена" : "Не выполнена";
			string dateText = LastUpdate.ToString("yyyy-MM-dd HH:mm:ss");

			return $"Полный текст задачи:\n\t{Text}\nСтатус: {statusText}\nДата последнего изменения: {dateText}";
		}
	}

	public class TodoList
	{
		private TodoItem[] tasks;
		private int count;
		private const int InitialCapacity = 2;
		private const int ShortTextLength = 30;

		public TodoList()
		{
			tasks = new TodoItem[InitialCapacity];
			count = 0;
		}

		public void Add(TodoItem item)
		{
			if (item == null)
			{
				return;
			}

			if (count >= tasks.Length)
			{
				tasks = IncreaseArray(tasks);
			}

			tasks[count] = item;
			count++;
		}

		public TodoItem GetItem(int index)
		{
			if (index < 1 || index > count)
			{
				return null;
			}
			return tasks[index - 1];
		}

		public void View(bool showIndex, bool showStatus, bool showDate)
		{
			if (count == 0)
			{
				Console.WriteLine("Список задач пуст.");
				return;
			}

			int indexWidth = count.ToString().Length;
			if (indexWidth < 5) indexWidth = 5;
			int taskWidth = ShortTextLength;
			int statusWidth = 10;
			int dateWidth = 19;

			string header = "";
			if (showIndex) header += $"{"Инд",-indexWidth} ";
			header += $"{"Задача",-taskWidth} ";
			if (showStatus) header += $"{"Статус",-statusWidth} ";
			if (showDate) header += $"{"Дата",-dateWidth}";

			Console.WriteLine("Список задач:");

			if (showIndex || showStatus || showDate)
			{
				Console.WriteLine(header.TrimEnd());
				Console.WriteLine(new string('-', header.Length));
			}

			for (int i = 0; i < count; i++)
			{
				TodoItem item = tasks[i];
				if (item == null) continue;

				string output = "";

				if (showIndex)
				{
					output += $"{(i + 1),-indexWidth} ";
				}

				string taskText = item.Text ?? string.Empty;
				if (taskText.Length > taskWidth)
				{
					taskText = taskText.Substring(0, taskWidth - 3) + "...";
				}
				output += $"{taskText,-taskWidth} ";

				if (showStatus)
				{
					string statusText = item.IsDone ? "сделано" : "не сделано";
					output += $"{statusText,-statusWidth} ";
				}

				if (showDate)
				{
					string dateText = item.LastUpdate.ToString("yyyy-MM-dd HH:mm:ss");
					output += $"{dateText,-dateWidth}";
				}

				if (showIndex || showStatus || showDate)
				{
					Console.WriteLine(output.TrimEnd());
				}
				else
				{
					Console.WriteLine(taskText);
				}
			}
		}

		private TodoItem[] IncreaseArray(TodoItem[] currentTasks)
		{
			int newCapacity = currentTasks.Length * 2;
			TodoItem[] newTasks = new TodoItem[newCapacity];

			for (int i = 0; i < currentTasks.Length; i++)
			{
				newTasks[i] = currentTasks[i];
			}

			return newTasks;
		}
	}

	class Program
	{
		private const string CommandHelp = "help";
		private const string CommandProfile = "profile";
		private const string CommandAdd = "add";
		private const string CommandDone = "done";
		private const string CommandUpdate = "update";
		private const string CommandView = "view";
		private const string CommandRead = "read";
		private const string CommandExit = "exit";

		private const string FlagMultiline = "multiline";
		private const string FlagShortMultiline = "m";

		private const string FlagIndex = "index";
		private const string FlagShortIndex = "i";
		private const string FlagStatus = "status";
		private const string FlagShortStatus = "s";
		private const string FlagDate = "update-date";
		private const string FlagShortDate = "d";
		private const string FlagAll = "all";
		private const string FlagShortAll = "a";

		private static Profile userProfile;
		private static TodoList todoList = new TodoList();

		public static void Main()
		{
			Console.WriteLine("Работу выполнил: Измайлов");

			string input;
			string firstName, lastName;
			int birthYear;

			Console.Write("Введите ваше имя: ");
			input = Console.ReadLine();
			firstName = string.IsNullOrEmpty(input) ? "Гость" : input;

			Console.Write("Введите вашу фамилию: ");
			input = Console.ReadLine();
			lastName = string.IsNullOrEmpty(input) ? "Гость" : input;

			bool validYear = false;
			while (!validYear)
			{
				Console.Write("Введите ваш год рождения: ");
				input = Console.ReadLine();
				if (int.TryParse(input, out birthYear) && birthYear > 1900 && birthYear <= DateTime.Now.Year)
				{
					validYear = true;
				}
				else
				{
					Console.WriteLine("Неверный формат года. Пожалуйста, введите корректный год (например, 1990).");
				}
			}

			userProfile = new Profile(firstName, lastName, birthYear);
			Console.WriteLine($"Добавлен пользователь {userProfile.GetInfo()}");

			while (true)
			{
				Console.Write("Введите команду: ");
				string command = Console.ReadLine();

				if (string.IsNullOrEmpty(command))
				{
					continue;
				}

				if (command == CommandHelp)
				{
					ShowHelp();
				}
				else if (command == CommandProfile)
				{
					ShowProfile();
				}
				else if (command.StartsWith(CommandAdd))
				{
					AddTask(command);
				}
				else if (command.StartsWith(CommandDone))
				{
					DoneTask(command);
				}
				else if (command.StartsWith(CommandUpdate))
				{
					UpdateTask(command);
				}
				else if (command.StartsWith(CommandView))
				{
					ViewTasks(command);
				}
				else if (command.StartsWith(CommandRead))
				{
					ReadTask(command);
				}
				else if (command == CommandExit)
				{
					Console.WriteLine("Программа завершена.");
					break;
				}
				else
				{
					Console.WriteLine("Неизвестная команда. Введите help для списка команд.");
				}
			}
		}

		private static void ShowHelp()
		{
			Console.WriteLine("""
            Доступные команды:
            help — список команд
            profile — выводит данные профиля
            add "текст задачи" multiline , m — добавляет задачу. Флаг multiline (m) позволяет вводить задачу в несколько строк до команды !end.
            done <idx> — отмечает задачу как выполненную.
            update <idx> "новый текст" — изменяет текст задачи.
            view [флаги] — просмотр всех задач. Показывает только текст задачи по умолчанию.
                index, i — показать индекс задачи
                status, s — показать статус задачи (сделано/не сделано)
                update-date, d — показать дату последнего изменения
                all, a — показать все данные
            read <idx> — просмотр полного текста задачи, статуса и даты по индексу
            exit — завершить программу
            """);
		}

		private static void ShowProfile()
		{
			Console.WriteLine(userProfile.GetInfo());
		}

		private static void AddTask(string command)
		{
			string taskText = "";
			bool isMultiline = false;
			string args = command.Length > CommandAdd.Length ? command.Substring(CommandAdd.Length).Trim() : string.Empty;

			if (string.IsNullOrEmpty(args))
			{
				Console.WriteLine("Неверный формат команды. Используйте: add \"текст задачи\" или add multiline");
				return;
			}

			if (args.EndsWith(FlagMultiline) || args.EndsWith(FlagShortMultiline))
			{
				isMultiline = true;
				if (args.EndsWith(FlagMultiline))
				{
					taskText = args.Substring(0, args.Length - FlagMultiline.Length).Trim();
				}
				else
				{
					taskText = args.Substring(0, args.Length - FlagShortMultiline.Length).Trim();
				}
			}
			else
			{
				taskText = args;
			}

			if (isMultiline)
			{
				Console.WriteLine("Введите задачу (для завершения введите !end):");
				string line;
				System.Text.StringBuilder taskBuilder = new System.Text.StringBuilder();

				if (!string.IsNullOrEmpty(taskText))
				{
					taskBuilder.AppendLine(taskText);
				}

				while (true)
				{
					Console.Write("> ");
					line = Console.ReadLine();
					if (line == null)
					{
						continue;
					}
					if (line.Trim().Equals("!end", StringComparison.OrdinalIgnoreCase))
					{
						break;
					}
					taskBuilder.AppendLine(line);
				}
				taskText = taskBuilder.ToString().TrimEnd();

				if (string.IsNullOrEmpty(taskText))
				{
					Console.WriteLine("Задача не добавлена (пустой текст).");
					return;
				}
			}
			else if (string.IsNullOrEmpty(taskText))
			{
				Console.WriteLine("Неверный формат команды. Используйте: add \"текст задачи\"");
				return;
			}

			TodoItem newItem = new TodoItem(taskText);
			todoList.Add(newItem);
			Console.WriteLine($"Задача добавлена: {taskText}");
		}

		private static void DoneTask(string command)
		{
			string args = command.Length > CommandDone.Length ? command.Substring(CommandDone.Length).Trim() : string.Empty;
			if (string.IsNullOrEmpty(args) || !int.TryParse(args, out int taskIndex))
			{
				Console.WriteLine("Неверный формат команды. Используйте: done <idx>");
				return;
			}

			TodoItem item = todoList.GetItem(taskIndex);

			if (item == null)
			{
				Console.WriteLine($"Ошибка: Задача с индексом {taskIndex} не найдена.");
				return;
			}

			item.MarkDone();
			Console.WriteLine($"Задача {taskIndex} отмечена как выполненная.");
		}

		private static void UpdateTask(string command)
		{
			string args = command.Length > CommandUpdate.Length ? command.Substring(CommandUpdate.Length).Trim() : string.Empty;
			string[] parts = args.Split(' ', 2);

			if (parts.Length != 2 || string.IsNullOrEmpty(parts[0]) || string.IsNullOrEmpty(parts[1]) || !int.TryParse(parts[0], out int taskIndex))
			{
				Console.WriteLine("Неверный формат команды. Используйте: update <idx> \"новый текст\"");
				return;
			}

			string newText = parts[1];

			TodoItem item = todoList.GetItem(taskIndex);

			if (item == null)
			{
				Console.WriteLine($"Ошибка: Задача с индексом {taskIndex} не найдена.");
				return;
			}

			item.UpdateText(newText);
			Console.WriteLine($"Задача {taskIndex} обновлена. Новый текст: {newText}");
		}

		private static void ViewTasks(string command)
		{
			string flags = command.Length > CommandView.Length ? command.Substring(CommandView.Length).Trim() : string.Empty;

			bool showIndex = flags.Contains(FlagIndex) || flags.Contains(FlagShortIndex) || flags.Contains(FlagAll) || flags.Contains(FlagShortAll);
			bool showStatus = flags.Contains(FlagStatus) || flags.Contains(FlagShortStatus) || flags.Contains(FlagAll) || flags.Contains(FlagShortAll);
			bool showDate = flags.Contains(FlagDate) || flags.Contains(FlagShortDate) || flags.Contains(FlagAll) || flags.Contains(FlagShortAll);

			todoList.View(showIndex, showStatus, showDate);
		}

		private static void ReadTask(string command)
		{
			string[] parts = command.Split(" ", 2);
			if (parts.Length != 2 || string.IsNullOrEmpty(parts[1]))
			{
				Console.WriteLine("Неверный формат команды. Используйте: read <idx>");
				return;
			}

			if (!int.TryParse(parts[1], out int taskIndex))
			{
				Console.WriteLine("Неверный индекс задачи.");
				return;
			}

			TodoItem item = todoList.GetItem(taskIndex);

			if (item == null)
			{
				Console.WriteLine($"Ошибка: Задача с индексом {taskIndex} не найдена.");
				return;
			}

			Console.WriteLine(item.GetFullInfo());
		}
	}
}