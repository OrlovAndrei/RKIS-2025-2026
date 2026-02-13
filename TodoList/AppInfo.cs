using System;
using System.Collections.Generic;
using TodoApp.Commands;

namespace TodoApp
{
	public static class AppInfo
	{
		public static List<Profile> Profiles { get; set; } = new List<Profile>();
		public static Guid? CurrentProfileId { get; set; }
		public static Profile? CurrentProfile =>
			CurrentProfileId.HasValue ? Profiles.Find(p => p.Id == CurrentProfileId.Value) : null;

		public static string ProfilesFilePath { get; set; } = string.Empty;

		public static Dictionary<Guid, TodoList> UserTodos { get; set; } = new Dictionary<Guid, TodoList>();

		public static TodoList Todos =>
			CurrentProfileId.HasValue && UserTodos.ContainsKey(CurrentProfileId.Value)
				? UserTodos[CurrentProfileId.Value]
				: new TodoList(new List<TodoItem>());
		public static Stack<IUndo> UndoStack { get; set; } = new Stack<IUndo>();
		public static Stack<IUndo> RedoStack { get; set; } = new Stack<IUndo>();

		public static void ResetUndoRedo()
		{
			UndoStack.Clear();
			RedoStack.Clear();
		}
	}
}