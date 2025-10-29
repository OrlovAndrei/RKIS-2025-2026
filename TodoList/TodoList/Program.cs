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
					var helpCommand = new HelpCommand();
					helpCommand.Execute();
					break;
				case "profile":
					var profileCommand = new ProfileCommand();
					profileCommand.UserProfile = userProfile;
					profileCommand.Execute();
					break;
				case string addCommand when addCommand.StartsWith("add"):
					if (addCommand.Contains("-m") || addCommand.Contains("--multiline"))
					{
						var multiAddCommand = new AddCommand();
						multiAddCommand.Todos = todos;
						multiAddCommand.Multiline = true;
						multiAddCommand.Execute();
					}
					else
					{
						var addCommandObj = new AddCommand();
						addCommandObj.Todos = todos;
						addCommandObj.Multiline = false;
						string[] taskText = addCommand.Split('\"');
						if (taskText.Length >= 2)
						{
							addCommandObj.TaskText = taskText[1];
							addCommandObj.Execute();
						}
						else
						{
							Console.WriteLine("Неверный формат команды. Используйте: add \"текст задачи\"");
						}
					}
					break;
				case string viewCommand when viewCommand.StartsWith("view"):
					var viewCommandObj = new ViewCommand();
					viewCommandObj.Todos = todos;
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
					viewCommandObj.AllOutput = allOutput;
					viewCommandObj.ShowIndex = showIndex;
					viewCommandObj.ShowStatus = showStatus;
					viewCommandObj.ShowDate = showDate;
					viewCommandObj.Execute();
					break;
				case string markTaskDone when markTaskDone.StartsWith("done "):
					var doneCommand = new MarkDoneCommand();
					doneCommand.Todos = todos;
					string[] taskDone = markTaskDone.Split(' ');
					if (taskDone.Length >= 2 && int.TryParse(taskDone[1], out int doneTaskID))
					{
						doneCommand.TaskIndex = doneTaskID;
						doneCommand.Execute();
					}
					else
					{
						Console.WriteLine("Неверный индекс задачи");
					}
					break;
				case string taskToDelete when taskToDelete.StartsWith("delete "):
					var deleteCommand = new DeleteCommand();
					deleteCommand.Todos = todos;
					string[] splitDeleteTaskText = taskToDelete.Split(' ');
					if (splitDeleteTaskText.Length >= 2 && int.TryParse(splitDeleteTaskText[1], out int deleteTaskID))
					{
						deleteCommand.TaskIndex = deleteTaskID;
						deleteCommand.Execute();
					}
					else
					{
						Console.WriteLine("Неверный индекс задачи");
					}
					break;
				case string updateTaskText when updateTaskText.StartsWith("update "):
					var updateCommand = new UpdateCommand();
					updateCommand.Todos = todos;
					string[] splitUpdateTaskText = updateTaskText.Split('\"');
					if (splitUpdateTaskText.Length >= 2)
					{
						string[] splitUpdateTaskID = updateTaskText.Split(' ');
						if (splitUpdateTaskID.Length >= 2 && int.TryParse(splitUpdateTaskID[1], out int taskID))
						{
							updateCommand.TaskIndex = taskID;
							updateCommand.NewText = splitUpdateTaskText[1];
							updateCommand.Execute();
						}
						else
						{
							Console.WriteLine("Неверный индекс задачи");
						}
					}
					else
					{
						Console.WriteLine("Неверный формат команды. Используйте: update индекс \"новый текст\"");
					}
					break;
				case string readTaskText when readTaskText.StartsWith("read"):
					var readCommand = new ReadCommand();
					readCommand.Todos = todos;
					string[] splitCommand = readTaskText.Split(' ');
					if (splitCommand.Length >= 2 && int.TryParse(splitCommand[1], out int readTaskId))
					{
						readCommand.TaskIndex = readTaskId;
						readCommand.Execute();
					}
					else
					{
						Console.WriteLine("Неверный индекс задачи");
					}
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
	private static Profile CreateUserProfile()
	{
		string name, surname;
		int yearOfBirth;
		Console.WriteLine("Напишите ваше имя и фамилию:");
		string fullName;
		while (string.IsNullOrEmpty(fullName = Console.ReadLine()))
		{
			Console.WriteLine("Вы ничего не ввели");
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
}