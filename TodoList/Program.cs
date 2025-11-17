Console.WriteLine("Работу выполнил: Морозов Иван 3833.9");
Console.WriteLine("Введите свое имя");
string name = Console.ReadLine();
Console.WriteLine("Ведите свою фамилию");
string surname = Console.ReadLine();
Console.WriteLine("Ведите свой год рождения");
int date = int.Parse(Console.ReadLine());
int age = 2025 - date;
Console.WriteLine("Добавлен пользователь " + name + " " + surname + ", Возраст " + age);

