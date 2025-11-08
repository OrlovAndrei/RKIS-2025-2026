using System;

namespace Todolist
{
	public class ViewCommand : ICommand
	{
		public TodoList TodoList { get; set; }
		public bool ShowIndex { get; set; }
		public bool ShowStatus { get; set; }
		public bool ShowDate { get; set; }
		public bool ShowAll { get; set; }

		public void Execute()
		{
			if (ShowAll)
			{
				ShowIndex = true;
				ShowStatus = true;
				ShowDate = true;
			}
			TodoList.View(ShowIndex, ShowStatus, ShowDate);
		}
	}
}