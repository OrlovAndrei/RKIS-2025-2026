using System;
using System.Collections.Generic;
using ToDoList;

namespace TodoList
{
	public static class AppInfo
	{
		public static TodoList Todos { get; set; }
		public static Profile CurrentProfile { get; set; }

		public static Stack<ICommand> undoStack = new Stack<ICommand>();
		public static Stack<ICommand> redoStack = new Stack<ICommand>();

		static AppInfo()
		{
			Todos = new TodoList();
			CurrentProfile = new Profile("", "", DateTime.Now.Year);
			undoStack = new Stack<ICommand>();
			redoStack = new Stack<ICommand>();
		}
	}
}