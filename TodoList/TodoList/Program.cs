Console.WriteLine("Работу выполнили: Амелина Яна и Кабанова Арина");
Console.WriteLine("ВВедите свое имя");
    String name = Console.ReadLine();
Console.WriteLine("Введите свою фамилию");
    String Surname = Console.ReadLine();
Console.WriteLine("ВВедите свою год рождения");
    String date1 = Console.ReadLine();
int date2 = int .Parse(date1);
int date3 = 2025;
int age = date3 - date2; 

Console.WriteLine("Добавлен пользователь " + name + " " + Surname + ", Возраст " + age);

