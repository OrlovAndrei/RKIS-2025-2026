namespace TodoApp.Commands
{
	public class ProfileCommand : BaseCommand
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
				string filePath = Path.Combine("data", $"profile_{AppInfo.CurrentProfileId}.csv");
				FileManager.SaveProfile(Profile, filePath);
				AppInfo.CurrentProfileId = null;
				Console.WriteLine($"Профиль сохранён в {filePath} и выполнен выход.");
			}
			else
			{
				Console.WriteLine(Profile.GetInfo());
			}
		}
		public override void Unexecute()
		{
			Console.WriteLine("Отмена просмотра профиля (нет изменений для отмены)");
		}
	}
}

