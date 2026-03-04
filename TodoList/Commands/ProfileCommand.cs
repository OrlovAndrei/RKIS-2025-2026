using System;

namespace TodoApp.Commands
{
	public class ProfileCommand : BaseCommand, ICommand
	{
		public Profile Profile { get; set; }
		public bool SaveToFile { get; set; } = false;
		private readonly IDataStorage _storage;

		public ProfileCommand(IDataStorage storage)
		{
			_storage = storage;
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
					_storage.SaveTodos(AppInfo.CurrentProfileId.Value, AppInfo.Todos);

				_storage.SaveProfiles(AppInfo.Profiles);

				Console.WriteLine("Профиль сохранён и выполнен выход.");
				AppInfo.CurrentProfileId = null;
				AppInfo.UserTodos.Clear();
				AppInfo.ResetUndoRedo();

				Program.ShowProfileSelection();
			}
			else
			{
				Console.WriteLine(Profile.GetInfo());
			}
		}
	}
}
