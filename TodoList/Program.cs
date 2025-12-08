namespace TodoList
{
	class Program
	{
		public static void Main()
		{
			Console.WriteLine("Работу выполнили Николаенко Крошняк");
            
			Console.Write("Введите ваше имя: ");
			string name = Console.ReadLine();
			Console.Write("Введите вашу фамилию: ");
			string surname = Console.ReadLine();

			Console.Write("Введите ваш год рождения: ");
			int year = int.Parse(Console.ReadLine());
			int age = DateTime.Now.Year - year;
            
			Console.WriteLine($"Добавлен пользователь {name} {surname}, возраст - {age}");
			
			bool isRunning = true;

			while (isRunning)
			{
				Console.Write("\nВведите команду: ");
				string command = Console.ReadLine();
				
				if (command == "help")
				{
					Console.WriteLine("""
					Доступные команды:
					help — список команд
					profile — выводит данные профиля
					""");
				}
				else if (command == "profile")
				{
					Console.WriteLine($"{name} {surname} {age}");
				}
				else
				{
					Console.WriteLine("Неизвестная команда");
				}
			}
		}
	}
}