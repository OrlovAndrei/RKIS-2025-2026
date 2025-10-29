using System;
using System.Collections.Generic;
using TodoList1;
Console.WriteLine("Работу выполнили Андрей и Роман и Петр");
Console.WriteLine("Введите Имя");
string name = Console.ReadLine();
Console.WriteLine("Введите Фамилию");
string surname = Console.ReadLine();
Console.WriteLine("Введите дату рождения");
int yearOfBirth = Convert.ToInt32(Console.ReadLine());
const int currentYear = 2025;
const int initialSize = 3;
int currentNumber;
currentNumber = currentYear - yearOfBirth;
Profile userProfile = new Profile(name, surname, yearOfBirth);
TodoList todolist = new TodoList(initialSize);
Console.WriteLine($"\nДобавлен пользователь: Имя: {name}, Фамилия: {surname}, Возраст: {currentNumber}");

while (true)
{
	Console.WriteLine("Введите команду: ");
	string commandInput = Console.ReadLine();
	ICommand command = CommandParser.Parse(commandInput, todolist, userProfile);
	command.Execute();
}