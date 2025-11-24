namespace TodoList;

internal class Program
{
	public static void Main()
	{
		Console.WriteLine("Работу выполнили Антонов и Мадойкин 3833");
		Console.Write("Введите имя: ");
		var name = Console.ReadLine();
		Console.Write("Введите фамилию: ");
		var surname = Console.ReadLine();

		Console.Write("Введите год рождения: ");
		var year = int.Parse(Console.ReadLine());
		var age = DateTime.Now.Year - year;

		Console.WriteLine($"Добавлен пользователь {name} {surname}, возраст - {age}");
	}
}