namespace TodoList
{
	class Program
	{
		public static void Main()
		{
			Console.WriteLine("Работу  выполнили Лютов и Легатов 3832");
			Console.Write("Введите ваше имя: "); 
			string firstName = Console.ReadLine();
			Console.Write("Введите вашу фамилию: ");
			string lastName = Console.ReadLine();

			Console.Write("Введите ваш год рождения: ");
			int year = int.Parse(Console.ReadLine());
			int age = DateTime.Now.Year - year;
			
			string text = "Добавлен пользователь " + firstName + " " + lastName + ", возраст - " + age;
			Console.WriteLine(text);
			

			while (true)
			{
				Console.Write("Введите команду: ");
				string command = Console.ReadLine();

				if (command == "help")
				{
					Console.WriteLine("Команды:");
					Console.WriteLine("help — выводит список всех доступных команд с кратким описанием");
					Console.WriteLine("profile — выводит данные пользователя");
				}
				else if (command == "profile")
				{
					Console.WriteLine(firstName + " " + lastName + ", - " + age);
				}
			}
		}
	}
}