using System;

namespace Todolist
{
	public class ProfileCommand : ICommand
	{
		public Profile UserProfile { get; set; }

		public void Execute()
		{
			if (string.IsNullOrEmpty(UserProfile.FirstName) || string.IsNullOrEmpty(UserProfile.LastName))
			{
				Console.WriteLine("Данные пользователя не заполнены");
				return;
			}
			Console.WriteLine(UserProfile.GetInfo());
		}
	}
}