// Работу выполнили Дайнеко и Гергель 3832

Console.WriteLine("Работу выполнили Дайнеко и Гергель 3832");
Console.WriteLine();

Console.Write("Введите имя: ");
string name = Console.ReadLine();

Console.Write("Введите фамилию: ");
string surname = Console.ReadLine();

Console.Write("Введите год рождения: ");
string yearText = Console.ReadLine();

int year = int.Parse(yearText);
int age = 2024 - year;

Console.WriteLine();
Console.WriteLine($"Добавлен пользователь {name} {surname}, возраст = {age}");

Console.WriteLine("Нажмите Enter чтобы выйти...");
Console.ReadLine();