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
            
			Console.WriteLine($"Добавлен пользователь {name} {surname}, возраст - {DateTime.Now.Year - year}");
		}
	}
}