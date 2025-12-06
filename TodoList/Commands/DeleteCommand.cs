namespace TodoApp.Commands
{
	public class DeleteCommand : BaseCommand
	{
		public new Guid? CurrentProfileId { get; set; }
		public int Index { get; set; }
		public TodoList? TodoList { get; private set; }
		public DeleteCommand(TodoList todoList, int index, Guid? currentProfileId)
		{
			this.TodoList = todoList;
			this.Index = index;
			this.CurrentProfileId = currentProfileId;
		}

		public DeleteCommand(Guid? profileId, int index)
		{
			CurrentProfileId = profileId;
			Index = index;
		}

		public override void Execute()
		{
			var todos = AppInfo.Todos;
			todos.Delete(Index);
		}

		public override void Unexecute()
		{
		}
	}
}

