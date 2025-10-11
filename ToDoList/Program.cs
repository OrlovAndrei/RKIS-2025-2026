using System;
using System.Threading;

namespace TodoListRefactored
{
    class Program
    {
        static string firstName, lastName;
        static int age, yearBirth;
        static string[] todos = new string[2];
        static bool[] statuses = new bool[2];
        static DateTime[] dates = new DateTime[2];
        static int index = 0;
        static bool work = true;

        static void Main()
        {
            PrintLoadingAnimation();
            PrintAsciiArt();
            Thread.Sleep(500);
            Console.Clear();

            Console.WriteLine("Задание выполнено Ждановым и Емелиным");
            AddUser();

            while (work)
            {
                Console.Write("\nВведите команду (help для справки): ");
                string? command = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(command)) continue;

                string trimmed = command.Trim();

                if (trimmed == "help") Help();
                else if (trimmed == "profile") ShowProfile();
                else if (trimmed == "view") ViewTodos();
                else if (trimmed == "exit") { PrintGoodbyeAnimation(); work = false; }
                else if (trimmed == "delete") DeleteProfile();
                else if (trimmed == "blackout") Blackout();
                else if (trimmed.StartsWith("add ")) AddTodo(trimmed.Split(" ", 2)[1]);
                else if (trimmed.StartsWith("done ")) DoneTodo(int.Parse(trimmed.Split(" ", 2)[1]));
                else if (trimmed.StartsWith("update ")) UpdateTodo(trimmed.Split(" ", 3)[1], trimmed.Split(" ", 3)[2]);
                else if (trimmed.StartsWith("remove ")) RemoveTodo(int.Parse(trimmed.Split(" ", 2)[1]));
                else Console.WriteLine("Неизвестная команда.");
            }
        }

        static void PrintWithDelay(string text, int delay = 100)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.WriteLine();
        }

        static void PrintAsciiArt()
        {
            Console.WriteLine(@"
  _____         _       _     _     _ 
 |  __ \       | |     | |   (_)   | |
 | |  | | ___  | |_ ___| |__  _  __| |
 | |  | |/ _ \ | __/ __| '_ \| |/ _` |
 | |__| |  __/ | || (__| | | | | (_| |
 |_____/ \___|  \__\___|_| |_|_|\__,_|
");
        }

        static void PrintLoadingAnimation()
        {
            string loading = "Загрузка системы";
            for (int i = 0; i < 3; i++)
            {
                Console.Write(loading);
                for (int dots = 0; dots < 3; dots++)
                {
                    Console.Write(".");
                    Thread.Sleep(300);
                }
                Thread.Sleep(300);
                Console.Clear();
            }
        }

        static void PrintGoodbyeAnimation()
        {
            string goodbye = "До свидания!";
            foreach (char c in goodbye)
            {
                Console.Write(c);
                Thread.Sleep(150);
            }
            Console.WriteLine();
        }

        static void PrintScaryAnimation()
        {
            string[] frames = {
@"
  .-.      .-.      .-.      .-.      .-.
 (o o)    (o o)    (o o)    (o o)    (o o)
 | O \    | O \    | O \    | O \    | O \
  \   \   \   \   \   \   \   \   \   \   \
   `~~~`   `~~~`   `~~~`   `~~~`   `~~~`   ",
@"
  .-.      .-.      .-.      .-.      .-.
 (O O)    (O O)    (O O)    (O O)    (O O)
 | o /    | o /    | o /    | o /    | o /
  \   \   \   \   \   \   \   \   \   \   \
   `~~~`   `~~~`   `~~~`   `~~~`   `~~~`   "
            };
            for (int i = 0; i < 6; i++)
            {
                Console.Clear();
                Console.WriteLine(frames[i % 2]);
                Thread.Sleep(400);
            }
            Console.Clear();
        }

        static void AddUser()
        {
            Console.Write("Введите ваше имя: ");
            firstName = Console.ReadLine() ?? throw new Exception("Вы не ввели имя!");
            Console.Write("Введите вашу фамилию: ");
            lastName = Console.ReadLine() ?? throw new Exception("Вы не ввели фамилию!");
            Console.Write("Введите ваш год рождения: ");
            if (!int.TryParse(Console.ReadLine(), out yearBirth) || yearBirth < 1800 || yearBirth > DateTime.Now.Year)
                throw new Exception("Некорректный год рождения!");
            age = DateTime.Now.Year - yearBirth;
            Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст – {age}");
        }

        static void Help()
        {
            Console.WriteLine("--- Команды ---");
            Console.WriteLine("help - список команд");
            Console.WriteLine("profile - данные пользователя");
            Console.WriteLine("add \"текст\" - добавить задачу");
            Console.WriteLine("done - отметить задачу выполненной");
            Console.WriteLine("update \"текст\" - обновить задачу");
            Console.WriteLine("remove - удалить задачу");
            Console.WriteLine("view - показать все задачи");
            Console.WriteLine("delete - удалить профиль");
            Console.WriteLine("blackout - полное удаление пользователей");
            Console.WriteLine("exit - выход");
        }

        static void ShowProfile() => PrintWithDelay($"Профиль: {firstName} {lastName}, {age}");

        static void AddTodo(string task)
        {
            if (index == todos.Length)
            {
                Array.Resize(ref todos, todos.Length * 2);
                Array.Resize(ref statuses, statuses.Length * 2);
                Array.Resize(ref dates, dates.Length * 2);
            }
            todos[index] = task;
            statuses[index] = false;
            dates[index] = DateTime.Now;
            PrintWithDelay($"Добавлена задача [{index}] {task}");
            index++;
        }

        static void DoneTodo(int i)
        {
            if (i >= 0 && i < index)
            {
                statuses[i] = true;
                PrintWithDelay($"Задача \"{todos[i]}\" выполнена!");
            }
        }

        static void UpdateTodo(string iStr, string task)
        {
            int i = int.Parse(iStr);
            if (i >= 0 && i < index)
            {
                todos[i] = task;
                dates[i] = DateTime.Now;
                PrintWithDelay("Задача обновлена.");
            }
        }

        static void RemoveTodo(int i)
        {
            if (i >= 0 && i < index)
            {
                PrintWithDelay($"Удалена задача: {todos[i]}");
                for (int j = i; j < index - 1; j++)
                {
                    todos[j] = todos[j + 1];
                    statuses[j] = statuses[j + 1];
                    dates[j] = dates[j + 1];
                }
                index--;
            }
        }

        static void ViewTodos()
        {
            Console.WriteLine("--- Список задач ---");
            if (index == 0) Console.WriteLine("Задач нет.");
            else
                for (int i = 0; i < index; i++)
                    Console.WriteLine($"{i}) {dates[i]} - {todos[i]} (выполнена: {statuses[i]})");
        }

        static void DeleteProfile()
        {
            Console.Write("Удалить профиль? (да/нет): ");
            if ((Console.ReadLine() ?? "").ToLower() == "да")
            {
                PrintWithDelay("Удаление профиля...");
                firstName = lastName = "";
                yearBirth = age = index = 0;
                todos = new string[2];
                statuses = new bool[2];
                dates = new DateTime[2];
                work = false;
                PrintWithDelay("Профиль удален.");
            }
        }

        static void Blackout()
        {
            Console.WriteLine("ВНИМАНИЕ! Удаляются все пользователи!");
            Console.Write("Введите код доступа: ");
            if (Console.ReadLine() == "0802")
            {
                PrintScaryAnimation();
                PrintWithDelay("Все пользователи удалены...");
                firstName = lastName = "";
                yearBirth = age = index = 0;
                todos = new string[2];
                statuses = new bool[2];
                dates = new DateTime[2];
                work = false;
            }
            else PrintWithDelay("Неверный код. Операция отменена.");
        }
    }
}