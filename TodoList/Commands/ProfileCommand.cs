using System;
using System.Linq;

namespace TodoList.Commands
{
	public class ProfileCommand : ICommand
	{
		public string[] Flags { get; set; } = Array.Empty<string>();

		public void Execute()
		{
			if (Flags.Contains("out") || Flags.Contains("logout"))
			{
				Program.SaveCurrentUserTasks();
				Console.WriteLine("Вы вышли из системы.");
				AppInfo.CurrentProfileId = null;
				return;
			}

			if (AppInfo.CurrentProfileId == null)
			{
				Console.WriteLine("Вы не вошли в систему");
				return;
			}

			var currentProfile = AppInfo.AllProfiles.FirstOrDefault(p => p.Id == AppInfo.CurrentProfileId);
			if (currentProfile != null)
			{
				currentProfile.ShowProfile();
			}
			else
			{
				Console.WriteLine("Профиль не найден");
			}
		}

		public void Unexecute() { }
	}
}