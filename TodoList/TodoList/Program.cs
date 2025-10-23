

internal class Program
{
	private static void Main(string[] args)
    {
        Console.WriteLine("Работу выполнили: Амелина Яна и Кабанова Арина");
		Profile userProfile = CreateUserProfile();
		TodoList todos = new TodoList();
		bool isOpen = true;
		Console.ReadKey();
        while (isOpen)
        {
            Console.Clear();
            string userCommand = "";
            Console.WriteLine("Введите команду:\nдля помощи напиши команду help");
            userCommand = Console.ReadLine();
            switch (userCommand)
            {
                case "help":
                    GetHelpInfo();
                    break;
                case "profile":
                    Console.WriteLine("Пользователь: " + userProfile.GetInfo(2025));
                    break;
				case string addCommand when addCommand.StartsWith("add"):
					if (addCommand.Contains("-m") || addCommand.Contains("--multiline"))
						MultiLineAddTask(todos);
					else
						AddTask(todos, addCommand);
					break;
				case string viewCommand when viewCommand.StartsWith("view"):
					bool allOutput = viewCommand.Contains("--all");
					bool showIndex = viewCommand.Contains("--index");
					bool showStatus = viewCommand.Contains("--status");
					bool showDate = viewCommand.Contains("--update-date");
					if (!viewCommand.Contains("--"))
					{
						int indexOfShortFlag = viewCommand.IndexOf("-");
						if (indexOfShortFlag >= 0)
						{
							for (int i = indexOfShortFlag + 1; i < viewCommand.Length; i++)
							{
								if (viewCommand[i] == 'i')
									showIndex = true;
								if (viewCommand[i] == 'd')
									showDate = true;
								if (viewCommand[i] == 's')
									showStatus = true;
								if (viewCommand[i] == 'a')
									allOutput = true;
							}
						}
					}
					if (viewCommand.Contains("-a") || viewCommand.Contains("--all"))
					{
						showIndex = true;
						showStatus = true;
						showDate = true;
					}
					todos.View(showIndex, showStatus, showDate);
					break;
				case string markTaskDone when markTaskDone.StartsWith("done "):
					MarkTaskDone(todos, markTaskDone);
					break;
				case string taskToDelete when taskToDelete.StartsWith("delete "):
					DeleteTask(todos, taskToDelete);
					break;
				case string updateTaskText when updateTaskText.StartsWith("update "):
					UpdateTask(todos, updateTaskText);
					break;
				case string readTaskText when readTaskText.StartsWith("read"):
					ReadFullTask(todos, readTaskText);
					break;
				case "exit":
                    isOpen = false;
                    break;
                default:
                    Console.WriteLine("Неправильно введена команда");
                    break;
            }
            Console.ReadKey();
        }
    }
	private static void GetHelpInfo()
	{
		Console.WriteLine("help - выводит список всех доступных команд\n" +
						 "profile - выводит ваши данные\n" +
						 "add - добавляет новую задачу (add \"Новая задача\")(флаги: add --multiline/-m, !end)\n" +
						 "view - просмотр задач (флаги: --index/-i, --status/-s, --update-date/-d, --all/-a)\n" +
						 "read idx - просмотр полного текста задач\n" +
						 "done - отмечает задачу выполненной\n" +
						 "delete - удаляет задачу по индексу\n" +
						 "update\"new_text\"- обновляет текст задачи\n" +
						 "exit - выйти");
	}
	private static Profile CreateUserProfile()
	{
		string name, surname;
		int yearOfBirth;
	WrongNameMark:
		Console.WriteLine("Напишите ваше имя и фамилию:");
		string fullName = Console.ReadLine();
		if (string.IsNullOrEmpty(fullName))
		{
			Console.WriteLine("Вы ничего не ввели");
			goto WrongNameMark;
		}
		string[] splitFullName = fullName.Split(' ', 2);
		name = splitFullName[0];
		surname = splitFullName.Length > 1 ? splitFullName[1] : "";
		Console.WriteLine("Напишите свой год рождения:");
		yearOfBirth = int.Parse(Console.ReadLine());
		Profile profile = new Profile(name, surname, yearOfBirth);
		Console.WriteLine("Добавлен пользователь: " + profile.GetInfo(2025));
		return profile;
	}
	private static void AddTask(TodoList todos, string task)
	{
		string[] taskText = task.Split('\"');
		TodoItem newTodo = new TodoItem(taskText[1]);
		todos.Add(newTodo);
		Console.WriteLine("Задача добавлена");
	}
	private static void MarkTaskDone(TodoList todos, string doneCommandText)
	{
		string[] taskDone = doneCommandText.Split(' ');
		if (taskDone.Length < 2 || !int.TryParse(taskDone[1], out int userTaskID) || userTaskID < 0 || userTaskID >= todos.Count)
		{
			Console.WriteLine("Неверный индекс задачи");
			return;
		}
		todos.GetItem(userTaskID).MarkDone();
		Console.WriteLine($"Задача {userTaskID} отмечена как выполненная!");
	}
	private static void DeleteTask(TodoList todos, string deleteTaskText)
	{
		string[] splitDeleteTaskText = deleteTaskText.Split(' ');
		if (splitDeleteTaskText.Length < 2 || !int.TryParse(splitDeleteTaskText[1], out int deleteTaskID) || deleteTaskID < 0 || deleteTaskID >= todos.Count)
		{
			Console.WriteLine("Неверный индекс задачи");
			return;
		}
		todos.Delete(deleteTaskID);
		Console.WriteLine($"Задача {deleteTaskID} удалена");
	}
	private static void UpdateTask(TodoList todos, string updateTasktext)
	{
		{
			string[] splitUpdateTaskText = updateTasktext.Split('\"');
			if (splitUpdateTaskText.Length < 2)
			{
				Console.WriteLine("Неверный формат команды. Используйте: update индекс \"новый текст\"");
				return;
			}
			string[] splitUpdateTaskID = updateTasktext.Split(' ');
			if (splitUpdateTaskID.Length < 2 || !int.TryParse(splitUpdateTaskID[1], out int taskID) || taskID < 0 || taskID >= todos.Count)
			{
				Console.WriteLine("Неверный индекс задачи");
				return;
			}
			todos.GetItem(taskID).UpdateText(splitUpdateTaskText[1]);
			Console.WriteLine($"Задача {taskID} обновлена!");
		}
	}
	private static void ReadFullTask(TodoList todos, string readCommand)
	{
		string[] splitCommand = readCommand.Split(' ');
		if (splitCommand.Length < 2 || !int.TryParse(splitCommand[1], out int taskId) || taskId < 0 || taskId >= todos.Count)
		{
			Console.WriteLine("Неверный индекс задачи");
			return;
		}
		Console.WriteLine(todos.GetItem(taskId).GetFullInfo());
	}
	private static void MultiLineAddTask(TodoList todos)
	{
		string userInput = "";
		bool isInput = true;
		string userTask = "";
		Console.WriteLine("Введите текст задачи (для завершения введите !end):");
		while (isInput)
		{
			Console.Write("> ");
			userInput = Console.ReadLine();
			if (userInput == "!end")
				isInput = false;
			else
				userTask += (userTask == "" ? "" : "\n") + userInput;
		}
		if (!string.IsNullOrEmpty(userTask))
		{
			TodoItem newTodo = new TodoItem(userTask);
			todos.Add(newTodo);
			Console.WriteLine("Задача добавлена!");
		}
	}
}