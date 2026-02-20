using System;
using TodoList.Exceptions;
namespace TodoList.Commands
{
	public class ViewCommand : ICommand
	{
		public bool ShowIndex { get; set; }
		public bool ShowDone { get; set; }
		public bool ShowDate { get; set; }
		public bool ShowAll { get; set; }
		public void Execute()
		{
			if (AppInfo.CurrentProfile == null)
			{
				throw new AuthenticationException("Для просмотра задач необходимо авторизоваться.");
			}
			if (ShowAll) ShowIndex = ShowDone = ShowDate = true;
			AppInfo.CurrentUserTodos.View(ShowIndex, ShowDone, ShowDate);
		}
	}
}