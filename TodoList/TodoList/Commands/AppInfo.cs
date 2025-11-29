public static class AppInfo
{
	public static Stack<ICommand> UndoStack { get; } = new Stack<ICommand>();
	public static Stack<ICommand> RedoStack { get; } = new Stack<ICommand>();
}
