namespace TodoApp.Commands
{
	public class ProfileCommand : BaseCommand, ICommand
	{
		public Profile Profile { get; set; }
		public bool SaveToFile { get; set; } = false;
		public ProfileCommand()
		{
			Profile = AppInfo.CurrentProfile;
		}

		public override void Execute()
		{
    		if (AppInfo.CurrentProfile == null)
    		{
        		Console.WriteLine("Ошибка: профиль не загружен.");
        		return;
    		}
    		if (SaveToFile)
    		{
        		if (AppInfo.CurrentProfileId.HasValue)
                {
                    string todosPath = Path.Combine("data", $"todos_{AppInfo.CurrentProfileId}.csv");
                    var todoList = AppInfo.Todos;
                    FileManager.SaveTodosForUser(todoList, todosPath);
                }
                string profileFilePath = Path.Combine("data", $"profile_{AppInfo.CurrentProfileId}.csv");
                FileManager.SaveProfile(Profile, profileFilePath);
                string profilesPath = Path.Combine("data", "profile.csv");
                FileManager.SaveAllProfiles(AppInfo.Profiles, profilesPath);
                Console.WriteLine($"Профиль сохранён в {profileFilePath} и выполнен выход.");
                AppInfo.CurrentProfileId = null;
				AppInfo.UserTodos.Clear();
                AppInfo.ResetUndoRedo();
				FileManager.SaveAllProfiles(AppInfo.Profiles, AppInfo.ProfilesFilePath);
				Program.ShowProfileSelection();
            }
            else
            {
                Console.WriteLine(Profile.GetInfo());
            }
        }
	}
}

