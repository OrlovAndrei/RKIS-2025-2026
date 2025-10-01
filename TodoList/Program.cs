using System;

class Program
{
    static void Main(string[] args)
    {
        string name = "Артем";
        string surname = "Бешкеев";
        int birthYear = 2007;

        string[] todos = new string[2];
        int todoCount = 0; 

        Console.WriteLine("Введите команду (help - список команд):");

        while (true)
        {
            Console.Write("> ");
            string input = Console.ReadLine();

          if (input == "help")
            {
                Console.WriteLine("Доступные команды:");
                Console.WriteLine("help - показать список команд");
                Console.WriteLine("profile - показать данные пользователя");
                Console.WriteLine("add \"текст задачи\" - добавить задачу");
                Console.WriteLine("view - показать все задачи");
                Console.WriteLine("exit - выйти из программы");
                continue;
            }

            if (input == "profile")
            {
                Console.WriteLine($"{name} {surname}, {birthYear}");
                continue;
            }