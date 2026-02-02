using System;
using TodoApp.Commands;
using System.Collections.Generic;
namespace TodoApp.Commands
{
	public static class AppInfo
	{
		public static List<Profile> Profiles { get; set; } = new List<Profile>();
		public static Guid? CurrentProfileId { get; set; }
		public static Profile CurrentProfile =>
			CurrentProfileId.HasValue
				? Profiles.FirstOrDefault(p => p.Id == CurrentProfileId.Value)
				: null;
		public static bool IsLoggedIn => CurrentProfile != null;
		public static string ProfilesFilePath { get; set; }
		public static Dictionary<Guid, TodoList> UserTodos { get; set; } = new Dictionary<Guid, TodoList>();
		public static Stack<BaseCommand> UndoStack { get; set; } = new Stack<BaseCommand>();
		public static Stack<BaseCommand> RedoStack { get; set; } = new Stack<BaseCommand>();
		public static TodoList Todos =>
				  CurrentProfileId.HasValue && UserTodos.ContainsKey(CurrentProfileId.Value)
					  ? UserTodos[CurrentProfileId.Value]
					  : new TodoList();
		public static void Logout()
        {
            CurrentProfileId = null;
        }
		public static void ClearUserTodos()
        {
            if (CurrentProfileId.HasValue)
            {
                UserTodos.Remove(CurrentProfileId.Value);
            }
        }
		public static void ResetUndoRedo()
		{
			UndoStack.Clear();
			RedoStack.Clear();
		}
	}
}
