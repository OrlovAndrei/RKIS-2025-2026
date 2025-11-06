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
		}
	}
}