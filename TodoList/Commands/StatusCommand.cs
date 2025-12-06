namespace TodoApp.Commands
{
	public class StatusCommand : BaseCommand
	{
		public new Guid? CurrentProfileId { get; set; }
		public int Index { get; set; }
		public TodoStatus NewStatus { get; set; }
		public TodoList? TodoList { get; set; }
		public StatusCommand(TodoList todoList, int index, TodoStatus status, Guid? currentProfileId)
		{
			this.TodoList = todoList;
			this.Index = index;
			this.NewStatus = status;
			this.CurrentProfileId = currentProfileId;
		}
		public StatusCommand(Guid? profileId, int index, TodoStatus status)
		{
			CurrentProfileId = profileId;
			Index = index;
			NewStatus = status;
		}

		public override void Execute()
		{
			var todos = AppInfo.Todos;
			todos.SetStatus(Index, NewStatus);
		}

		public override void Unexecute()
		{
		}
	}
}