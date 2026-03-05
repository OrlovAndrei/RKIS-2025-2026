using System;
using System.Collections.Generic;
using System.IO;

namespace Todolist
{
	public static class AppInfo
	{
		public static TodoList Todos { get; set; }
		public static Profile CurrentProfile { get; set; }

		static AppInfo()
		{
			Console.WriteLine("AppInfo запускается...");

			// Создаем пустые объекты
			Todos = new TodoList();
			CurrentProfile = new Profile();

			Console.WriteLine("AppInfo работает, все норм");
		}

		public static void LoadFromFiles(string profilePath, string todosPath)
		{
			if (File.Exists(profilePath))
				CurrentProfile = FileManager.LoadProfile(profilePath);

			if (File.Exists(todosPath))
				Todos = FileManager.LoadTodos(todosPath);

			Console.WriteLine("Данные загружены в AppInfo");
		}
	}
}