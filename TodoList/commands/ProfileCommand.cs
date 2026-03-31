using System;

namespace TodoList.commands;

public class ProfileCommand : ICommand
{
    public string[] parts { get; set; }

    public void Execute()
    {
        if (parts.Length == 1)
        {
            Console.WriteLine(CommandParser.userProfile?.GetInfo() ?? "Профиль не установлен");
            return;
        }

        if (parts.Length < 2)
        {
            Console.WriteLine("Ошибка: недостаточно аргументов");
            return;
        }

        switch (parts[1].ToLower())
        {
            case "set":
            case "установить":
                if (parts.Length >= 5 && int.TryParse(parts[4], out var birthYear))
                {
                    CommandParser.userProfile = new Profile(parts[2], parts[3], birthYear);
                    FileManager.SaveProfile(CommandParser.userProfile);
                    Console.WriteLine($"Профиль установлен: {CommandParser.userProfile.GetInfo()}");
                }
                else
                {
                    Console.WriteLine("Ошибка: неверный формат команды. Использование: profile set [имя] [фамилия] [год_рождения]");
                }
                break;
                
            case "показать":
            case "show":
                Console.WriteLine(CommandParser.userProfile?.GetInfo() ?? "Профиль не установлен");
                break;
                
            default:
                Console.WriteLine("Неизвестная подкоманда профиля. Доступные: set, show");
                break;
        }
    }
}