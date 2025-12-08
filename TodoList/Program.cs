namespace TodoList
{
	class Program
	{
		public static void Main()
		{
			Console.WriteLine("Работу выполнил Турчин 3833.9");
			Console.Write("Введите имя: "); 
			string name = Console.ReadLine();
			Console.Write("Введите фамилию: ");
			string surname = Console.ReadLine();

			Console.Write("Введите год рождения: ");
			int year = int.Parse(Console.ReadLine());
			int age = DateTime.Now.Year - year;

			Console.WriteLine($"Добавлен пользователь {name} {surname}, возраст - {age}");
		}
	}
}