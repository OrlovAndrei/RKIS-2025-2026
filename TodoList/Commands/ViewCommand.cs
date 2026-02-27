namespace TodoApp.Commands
{
	public class ViewCommand : BaseCommand, ICommand
	{
		public new Guid? CurrentProfileId { get; set; }
		public bool ShowIndex { get; set; } = true;
		public bool ShowDone { get; set; } = true;
		public bool ShowDate { get; set; } = false;
		public bool ShowStatus { get; set; } = true;
		public bool ShowAll { get; set; } = false;
		public TodoStatus? FilterStatus { get; set; }
		public TodoList? TodoList { get; private set; }
		public ViewCommand(TodoList todoList, Guid? currentProfileId)
		{
			TodoList = todoList;
			CurrentProfileId = currentProfileId;
		}

		public ViewCommand(Guid? profileId)
		{
			CurrentProfileId = profileId;
		}

		public override void Execute()
		{
			var todos = AppInfo.Todos;

			if (FilterStatus.HasValue)
			{
				todos.ViewByStatus(FilterStatus.Value, ShowIndex, ShowDate, ShowDone, ShowAll);
			}
			else
			{
				todos.View(ShowIndex, ShowDone, ShowDate, ShowAll);
			}
		}
	}
}


