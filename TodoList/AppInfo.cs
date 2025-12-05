using TodoApp.Commands;
using System.Collections.Generic;
using TodoApp;

namespace TodoApp 
{
	public static class AppInfo
	{
		public static List<Profile> Profiles { get; set; } = new List<Profile>();
		public static Guid? CurrentProfileId { get; set; }

		public static Dictionary<Guid, TodoList> UserTodos { get; set; } = new Dictionary<Guid, TodoList>();
		public static Stack<BaseCommand> UndoStack { get; set; } = new Stack<BaseCommand>();
		public static Stack<BaseCommand> RedoStack { get; set; } = new Stack<BaseCommand>();
		public static void ResetUndoRedo()
		{
			UndoStack = new Stack<BaseCommand>();
			RedoStack = new Stack<BaseCommand>();
		}
	}
}
