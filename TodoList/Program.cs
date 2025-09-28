
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
		}
	}
}
