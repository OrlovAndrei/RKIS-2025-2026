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
		private static Profile userProfile;
		private static TodoList todoList = new TodoList();

		public static void Main()
		{
			Console.WriteLine("Работу выполнил: Измайлов");

			InitializeUserProfile();

			while (true)
			{
				Console.Write("Введите команду: ");
				string input = Console.ReadLine();

				if (string.IsNullOrEmpty(input))
				{
					continue;
				}

				ICommand command = CommandParser.Parse(input, todoList, userProfile);

				if (command != null)
				{
					if (command is ExitCommand)
					{
						command.Execute();
						break;
					}
					command.Execute();
				}
			}
		}

		private static void InitializeUserProfile()
		{
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
		}
	}
}