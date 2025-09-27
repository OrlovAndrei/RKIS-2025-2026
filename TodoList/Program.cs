using System;

Console.Write("Введите имя: ");
string name = Console.ReadLine();

Console.Write("Введите фамилию: ");
string surname = Console.ReadLine();

Console.Write("Введите год рождения: ");
string birthYearString = Console.ReadLine();

int birthYear = int.Parse(birthYearString);
int currentYear = DateTime.Now.Year;
int age = currentYear - birthYear;

Console.WriteLine($"Добавлен пользователь {name} {surname}, возраст - {age}");
