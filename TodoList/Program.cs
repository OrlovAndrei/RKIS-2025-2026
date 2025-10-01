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