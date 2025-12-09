namespace TodoList
{
	class Program
	{
		public static void Main()
		{
			Console.WriteLine("Работу выполнили: Десятун и Пономаренко 3833");
            
			Console.Write("Введите ваше имя: ");
			string name = Console.ReadLine();
			Console.Write("Введите вашу фамилию: ");
			string surname = Console.ReadLine();

			Console.Write("Введите ваш год рождения: ");
			int year = int.Parse(Console.ReadLine());
			var age = DateTime.Now.Year - year;
			Console.WriteLine($"Добавлен пользователь {name} {surname}, возраст - {age}");
			
			while (true)
			{
				Console.WriteLine("\nВведите команду: ");
				string userInput = Console.ReadLine();
				
				if (userInput == "help")
				{
					Console.WriteLine("Команды:");
					Console.WriteLine("help — выводит список всех доступных команд с кратким описанием");
					Console.WriteLine("profile — выводит данные пользователя");
				}
				else if (userInput == "profile")
				{
					Console.WriteLine($"{name} {surname} {age}");
				}
				else
				{
					Console.WriteLine("Неизвестная команда. Воспользуйтесь командой help");
				}
			}
		}
	}
}