using System;
using TodoApp.Commands;
namespace TodoApp;
class Program
{
	static void Main(string[] args)
	{
		string dataDir = Path.Combine(Environment.CurrentDirectory, "data");
		string profilePath = Path.Combine(dataDir, "profile.txt");
		string todoPath = Path.Combine(dataDir, "todo.txt");
		string donePath = Path.Combine(dataDir, "done.txt");

		FileManager.EnsureDataDirectory(dataDir);
		if (!File.Exists(todoPath))
			File.WriteAllText(todoPath, "", System.Text.Encoding.UTF8);
		if (!File.Exists(donePath))
			File.WriteAllText(donePath, "", System.Text.Encoding.UTF8);

		Profile userProfile = FileManager.LoadProfile(profilePath);
		if (userProfile == null)
		{
			Console.WriteLine("Введите Имя:");
			string name = Console.ReadLine();
			Console.WriteLine("Введите Фамилию:");
			string surname = Console.ReadLine();
			Console.WriteLine("Введите год рождения:");
			int yearOfBirth = Convert.ToInt32(Console.ReadLine());

			userProfile = new Profile(name, surname, yearOfBirth);
			FileManager.SaveProfile(userProfile, profilePath);
			Console.WriteLine($"Создан профиль: {userProfile.GetInfo()}");
		}
		else
		{
			Console.WriteLine($"Загружен профиль: {userProfile.GetInfo()}");
		}

		TodoList todoList = FileManager.LoadTodos(todoPath, donePath);
		if (todoList == null)
		{
			todoList = new TodoList();
			Console.WriteLine("Создан пустой список задач.");
		}
		else
		{
			Console.WriteLine($"Загружено задач: {todoList.Count}");
		}

		while (true)
		{
			Console.WriteLine("Введите команлу: ");
			string commandInput = Console.ReadLine();
			BaseCommand command = CommandParser.Parse(commandInput, todoList, userProfile);
			command.Execute();
			FileManager.SaveTodos(todoList, todoPath);
		}
	}
}