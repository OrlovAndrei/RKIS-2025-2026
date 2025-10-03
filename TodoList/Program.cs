using System;

class Program
{
    static void Main(string[] args)
    {
        string name = "Алексей и Лев";
        string surname = "Прокопенко и Морозов";
        int birthYear = 2007;

        string[] todos = new string[2];
        int todoCount = 0; 

        Console.WriteLine("Добро пожаловать, введите help:");

        while (true)
        {
            Console.Write("> ");
            string input = Console.ReadLine();
            
          if (input == "help")
            {
                Console.WriteLine("Доступные команды:");
                Console.WriteLine("help - показать список команд");
                Console.WriteLine("profile - показать наши данные");
                Console.WriteLine("add "текст задачи" - добавить задачу");
                Console.WriteLine("view - показать все задания");
                Console.WriteLine("exit - выйти из программы");
                continue;
            }