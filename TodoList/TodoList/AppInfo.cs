public static class AppInfo
{
	public static Stack<ICommand> UndoStack { get; } = new Stack<ICommand>();
	public static Stack<ICommand> RedoStack { get; } = new Stack<ICommand>();
	public static List<Profile> Profiles { get; set; } = new List<Profile>();
	public static Guid CurrentProfileId { get; set; } = Guid.Empty;
	public static TodoList CurrentUserTodos { get; set; } = new TodoList();
	public static string DataDir { get; set; } = "Data";
	public static bool ShouldLogout { get; set; } = false;
}