

namespace TodoList;
internal class AppInfo
{
	public static TodoList CurrentTodoList => Todos[CurrentProfileID];
	public static List<TodoList> Todos { get; set; }
	public static Profile CurrentProfile => Profiles[CurrentProfileID];

	public static Stack<ICommand> UndoStack = new Stack<ICommand>();
	public static Stack<ICommand> RedoStack = new Stack<ICommand>();
	public static List<Profile> Profiles { get; set; }
	public static int CurrentProfileID { get; set; }

	public static void UndoPush(ICommand command) => UndoStack.Push(command);
	public static void UndoPop() => UndoStack.Pop();
	public static void RedoPush(ICommand command) => RedoStack.Push(command);
	public static void RedoPop() => RedoStack.Pop();
	public static void ClearCommandsHistory()
	{
		UndoStack.Clear();
		RedoStack.Clear();
	}

	public static void InsertNewProfile(Profile profile)
	{
		Profiles.Add(profile);
		CurrentProfileID = Profiles.Count - 1;
		Todos.Add(new TodoList());
		FileManager.InitializeTodoListCallacks(Todos[CurrentProfileID]);
		Console.WriteLine($"Добавлен новый пользователь {profile}.");
	}

	public static bool IsHaveLogin(string login, out int index)
	{
		index = -1;
		for (int i = 0; i < Profiles.Count; i++)
		{
			if (Profiles[i].Login == login)
			{
				index = i;
				return true;
			}
		}
		return false;
	}
}
