

namespace TodoList
{
    internal class ExitCommand : ICommand
    {
		public void Execute()
		{
			FileManager.SaveCurrentTodoList();
			FileManager.SaveProfiles(FileManager.ProfileInfoPath);
			Environment.Exit(0);
		}

	}
}
