using System;
using System.Collections.Generic;
using TodoList.Commands;
using TodoList.Interfaces;
using TodoList.Models;

namespace TodoList
{
	public static class AppInfo
	{
		public static IDataStorage Storage { get; set; } = null!;
		
		public static Dictionary<Guid, TodoList> AllTodos { get; set; } = new Dictionary<Guid, TodoList>();

		public static List<Profile> AllProfiles { get; set; } = new List<Profile>();

		public static Guid? CurrentProfileId { get; set; }

		public static Stack<ICommand> undoStack { get; set; } = new Stack<ICommand>();
		public static Stack<ICommand> redoStack { get; set; } = new Stack<ICommand>();

		public static TodoList? CurrentUserTodos
		{
			get
			{
				if (CurrentProfileId == null || !AllTodos.ContainsKey(CurrentProfileId.Value))
				{
					return null;
				}
				return AllTodos[CurrentProfileId.Value];
			}
		}
	}
}