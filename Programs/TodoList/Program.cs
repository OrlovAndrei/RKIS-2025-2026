using System;
bool ZXC = true;
int CurrentYear = DateTime.Now.Year;

Console.Write("Введите свое имя: ");
string? name = Console.ReadLine();
Console.Write("Введите свою фамилию: ");
string? secname = Console.ReadLine();
Console.Write("Введите свой год рождения: ");
var bday = Console.ReadLine();

try
{
    int m = Int32.Parse(bday);
}
catch (FormatException)
{
    ZXC = false;
}

int convertedbday = Int32.Parse(bday);

if ((ZXC == true) && (convertedbday <= CurrentYear))
{
    int age = CurrentYear - convertedbday;
    Console.WriteLine($"Добавлен пользователь:{name},{secname},возраст - {age}");
}
else
{
    Console.WriteLine("Неправильно введенный год"); // это ошибка, которая выводится при неправильном введении года рождения, например если введут "арбуз" или введут год больше 2025
}




