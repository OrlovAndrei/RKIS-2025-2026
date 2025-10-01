using System;

class Program
{
    static void Main(string[] args)
    { 
        string[] todos = new string[2];
        
        int todoCount = 0;

        Console.WriteLine("Введите команду (help - список команд):");

        while (true)
        {
            Console.Write("> ");
            string input = Console.ReadLine();