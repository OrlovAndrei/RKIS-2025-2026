namespace TodoList.commands;

public class ProfileCommand : ICommand
{
	public string[] parts { get; set; }

	public void Execute()
	{
		if (parts.Length == 1)
		{
			Console.WriteLine(CommandParser.userProfile.GetInfo());
			return;
		}

				if (parts.Length >= 5 && int.TryParse(parts[4], out var birthYear))
				{
					CommandParser.userProfile = new Profile(parts[2], parts[3], birthYear);
					FileManager.SaveProfile(CommandParser.userProfile);
					Console.WriteLine($"Профиль установлен: {CommandParser.userProfile.GetInfo()}");
				}
				else
				break;
			case "показать":
			case "show":
				Console.WriteLine(CommandParser.userProfile.GetInfo());
				break;
			default:
				Console.WriteLine("Неизвестная подкоманда профиля");
				break;
	}
}