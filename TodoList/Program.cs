namespace TodoList
{
	class Program
	{
		public static void Main()
		{
			Console.WriteLine("Выполнил Герасимов Егор");
			Console.Write("Введите имя: "); 
			string firstName = Console.ReadLine();
			Console.Write("Введите фамилию: ");
			string lastName = Console.ReadLine();

			Console.Write("Введите год рождения: ");
			int year = int.Parse(Console.ReadLine());

			string output = "Добавлен пользователь " + firstName + " " + lastName + ", возраст - " + (DateTime.Now.Year - year);
			Console.WriteLine(output);
		}
	}
}