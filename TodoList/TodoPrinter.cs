using System;
using System.Linq;

namespace TodoApp.Commands
{
	public static class TodoPrinter
	{
		public static void PrintAllTasksInOneLine(TodoList todos)
		{
			PrintTasks(todos);
		}

		public static void PrintPendingTasksInOneLine(TodoList todos)
		{
			PrintTasks(todos, false);
		}

		public static void PrintCompletedTasksInOneLine(TodoList todos)
		{
			PrintTasks(todos, true);
		}

		private static void PrintTasks(TodoList todos, bool? isDoneFilter = null)
		{
			var taskLines = todos
				.Where(t => isDoneFilter == null || t.IsDone == isDoneFilter)
				.Select((t, index) => t.GetFormattedInfo(index))
				.ToList();
			Console.WriteLine(string.Join("\n", taskLines));
		}
	}
}