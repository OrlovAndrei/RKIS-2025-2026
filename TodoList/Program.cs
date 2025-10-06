
namespace TodoList
{
	class MainClass
	{
		public static void Main()
		{
			Console.WriteLine("Работу выполнили: Вдовиченко и Кравец");
			Console.Write("Введите ваше имя: ");
			string name = Console.ReadLine();
			Console.Write("Введите вашу фамилию: ");
			string surname = Console.ReadLine();

			Console.Write("Введите ваш год рождения: ");
			int year = int.Parse(Console.ReadLine());

			string text = $"Добавлен пользователь {name} {surname}, возраст - {DateTime.Now.Year - year}";
			Console.WriteLine(text);

			while (true)
			{
				Console.WriteLine("Введите команду:");
				string command = Console.ReadLine();

				if (command == "help")
				{
					Console.WriteLine("Список команд:");
					Console.WriteLine("help — выводит список доступных команд");
					Console.WriteLine("profile — выводит данные пользователя");
					Console.WriteLine("exit — выход из цилка");
				}
				else if (command == "profile")
				{
					Console.WriteLine(name + " " + surname + " - " + (DateTime.Now.Year - year));
				}
				else if (command == "exit")
				{
					Console.WriteLine("Выход из цилка.");
					break;
				}
			}
		}
	}
}