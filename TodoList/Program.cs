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
		}
	}
}