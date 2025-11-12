using TodoList.Commands;

namespace TodoList
{
	class Program
	{
		public static void Main()
		{
			FileManager.EnsureDataDirectory(FileManager.dataDirPath);
			if (!File.Exists(FileManager.profilePath)) File.WriteAllText(FileManager.profilePath, "Default User 2000");
			Console.WriteLine("Работу выполнили Поплевин и Музыка 3831");

			while (true)
			{
				Console.Write("\nВведите команду: ");
				var input = Console.ReadLine();

				var command = CommandParser.Parse(input);
				command.Execute();
			}
		}
	}
}