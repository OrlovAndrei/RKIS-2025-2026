namespace TodoApp;

public class Profile
{
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public int BirthYear { get; set; }
	public int Age => DateTime.Now.Year - BirthYear;

	public Profile(string firstName, string lastName, int birthYear)
	{
		FirstName = firstName;
		LastName = lastName;
		BirthYear = birthYear;
	}

	public string GetInfo()
	{
		return $"{FirstName} {LastName}, возраст {Age}";
	}

	public static Profile CreateFromInput()
	{
		Console.Write("Введите ваше имя: ");
		string firstName = Console.ReadLine()?.Trim() ?? "Неизвестно";

		Console.Write("Введите вашу фамилию: ");
		string lastName = Console.ReadLine()?.Trim() ?? "Неизвестно";

		Console.Write("Введите год вашего рождения: ");
		if (int.TryParse(Console.ReadLine(), out int birthYear) && birthYear > 1900 && birthYear <= DateTime.Now.Year)
		{
			return new Profile(firstName, lastName, birthYear);
		}
		else
		{
			Console.WriteLine("Неверный год рождения. Установлен год по умолчанию: 2000");
			return new Profile(firstName, lastName, 2000);
		}
	}
}