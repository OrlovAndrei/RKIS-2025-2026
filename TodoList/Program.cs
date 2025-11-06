namespace TodoList
{
	internal class Program
	{
		public static void Main()
		{
			Console.WriteLine("Работу выполнили Поплевин и Музыка 3831");
			Console.Write("Введите ваше имя: "); 
			string firstName = Console.ReadLine();
			Console.Write("Введите вашу фамилию: ");
			string lastName = Console.ReadLine();

			Console.Write("Введите ваш год рождения: ");
			int year = int.Parse(Console.ReadLine());

			Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст - {DateTime.Now.Year - year}");
            
			while (true)
			{
				Console.Write("Введите команду: ");
				string command = Console.ReadLine();

				if (command == "help")
				{
					Console.WriteLine("""
					Доступные команды:
					help — список команд
					profile — выводит данные профиля
					add "текст задачи" — добавляет задачу
					view — просмотр всех задач
					exit — завершить программу
					""");
				}
				else if (command == "profile")
				{
					Console.WriteLine($"{firstName} {lastName}, {year}");
				}
			}
		}
	}
}