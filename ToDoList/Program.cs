using System;
using System.Threading;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;

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

        static readonly string dataFolder = Path.Combine(Environment.CurrentDirectory, "data");
        static readonly string todoFilePath = Path.Combine(dataFolder, "todos.txt");

        static void Main()
        {
            SetupDataFolder();

            PrintLoadingAnimation();
            PrintAsciiArt();
            Thread.Sleep(500);
            Console.Clear();

            Console.WriteLine("Задание выполнено Ждановым и Емелиным");
            AddUser();
            LoadTodosFromFile();

            while (work)
            {
                Console.Write("\nВведите команду (help для справки): ");
                string? command = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(command)) continue;
                string trimmed = command.Trim();

                if (trimmed == "help") Help();
                else if (trimmed == "profile") ShowProfile();
                else if (trimmed == "view") ViewTodos();
                else if (trimmed == "exit") { SaveTodosToFile(); PrintGoodbyeAnimation(); work = false; }
                else if (trimmed == "delete") DeleteProfile();
                else if (trimmed == "blackout") Blackout();
                else if (trimmed == "info") ShowInfo();
                else if (trimmed == "oop") OopDemo.Show();
                else if (trimmed == "oop2") AdvancedOopDemo.Show();
                else if (trimmed == "files") FileDemo.Show(dataFolder, todoFilePath);
                else if (trimmed.StartsWith("add ")) { AddTodo(trimmed.Substring(4)); SaveTodosToFile(); }
                else if (trimmed.StartsWith("done ")) { DoneTodo(int.Parse(trimmed.Substring(5))); SaveTodosToFile(); }
                else if (trimmed.StartsWith("update ")) 
                {
                    var parts = trimmed.Split(' ', 3);
                    if (parts.Length == 3)
                    {
                        UpdateTodo(parts[1], parts[2]);
                        SaveTodosToFile();
                    }
                    else Console.WriteLine("Ошибка в команде update. Используйте: update [номер] [текст]");
                }
                else if (trimmed.StartsWith("remove ")) { RemoveTodo(int.Parse(trimmed.Substring(7))); SaveTodosToFile(); }
                else Console.WriteLine("Неизвестная команда.");
            }
        }

        static void SetupDataFolder()
        {
            if (!Directory.Exists(dataFolder))
                Directory.CreateDirectory(dataFolder);
        }

        static void SaveTodosToFile()
        {
            try
            {
                using StreamWriter writer = new StreamWriter(todoFilePath, false, Encoding.UTF8);
                for (int i = 0; i < index; i++)
                {
                    string line = $"{todos[i]}|{statuses[i]}|{dates[i]:O}";
                    writer.WriteLine(line);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при сохранении задач: " + ex.Message);
            }
        }

        static void LoadTodosFromFile()
        {
            if (!File.Exists(todoFilePath)) return;

            try
            {
                string[] lines = File.ReadAllLines(todoFilePath, Encoding.UTF8);
                index = 0;
                foreach (var line in lines)
                {
                    var parts = line.Split('|');
                    if (parts.Length != 3) continue;
                    if (index == todos.Length)
                    {
                        Array.Resize(ref todos, todos.Length * 2);
                        Array.Resize(ref statuses, statuses.Length * 2);
                        Array.Resize(ref dates, dates.Length * 2);
                    }
                    todos[index] = parts[0];
                    statuses[index] = bool.TryParse(parts[1], out bool st) && st;
                    dates[index] = DateTime.TryParse(parts[2], null, System.Globalization.DateTimeStyles.RoundtripKind, out DateTime dt) ? dt : DateTime.Now;
                    index++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при загрузке задач: " + ex.Message);
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
            Console.WriteLine("done [номер] - отметить задачу выполненной");
            Console.WriteLine("update [номер] \"текст\" - обновить задачу");
            Console.WriteLine("remove [номер] - удалить задачу");
            Console.WriteLine("view - показать все задачи");
            Console.WriteLine("delete - удалить профиль");
            Console.WriteLine("blackout - полное удаление пользователей");
            Console.WriteLine("info - показать демонстрацию языковых фич");
            Console.WriteLine("oop - показать демонстрацию ООП");
            Console.WriteLine("oop2 - показать расширенную демонстрацию ООП");
            Console.WriteLine("files - показать демонстрацию работы с файлами");
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
            else Console.WriteLine("Неверный номер задачи.");
        }

        static void UpdateTodo(string iStr, string task)
        {
            if (int.TryParse(iStr, out int i) && i >= 0 && i < index)
            {
                todos[i] = task;
                dates[i] = DateTime.Now;
                PrintWithDelay("Задача обновлена.");
            }
            else Console.WriteLine("Неверный номер задачи.");
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
            else Console.WriteLine("Неверный номер задачи.");
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

        static void ShowInfo()
        {
            Console.WriteLine("\n=== Демонстрация языковых возможностей C# ===\n");

            int valueType = 10;
            int anotherValue = valueType;
            anotherValue = 20;

            string refType = "Hello";
            string anotherRef = refType;
            anotherRef = "Changed";

            Console.WriteLine($"Значимые типы: {valueType}, {anotherValue}");
            Console.WriteLine($"Ссылочные типы: {refType}, {anotherRef}");

            int? nullableInt = null;
            Console.WriteLine($"Nullable: {nullableInt?.ToString() ?? "null"}");

            GC.Collect();
            Console.WriteLine("GC вызван");

            string s = "Строка\nС переносом\tи табуляцией";
            Console.WriteLine(s);

            int a = 5, b = 7;
            Console.WriteLine($"{a} + {b} = {a + b}");

            double money = 1234.56789;
            Console.WriteLine($"{money:F2}");

            StringBuilder sb = new StringBuilder();
            sb.Append("Это ");
            sb.Append("StringBuilder ");
            sb.Append("работает!");
            Console.WriteLine(sb.ToString());

            byte[] bytes = Encoding.UTF8.GetBytes("Привет");
            Console.WriteLine(BitConverter.ToString(bytes));

            string email = "test@gmail.com";
            bool match = Regex.IsMatch(email, @"^[\w\.-]+@\w+\.\w+$");
            Console.WriteLine($"Regex: {match}");

            int number = 10;
            Increment(ref number);
            Console.WriteLine(number);

            string msg;
            MakeMessage(out msg);
            Console.WriteLine(msg);

            Console.WriteLine("\n=== Конец ===\n");
        }

        static void Increment(ref int x) => x++;
        static void MakeMessage(out string m) => m = "out-параметр отработал";
    }

    static class FileDemo
    {
        public static void Show(string folderPath, string todoFile)
        {
            Console.WriteLine("\n=== Демонстрация работы с файлами и папками ===\n");

            Console.WriteLine($"Текущая папка для данных: {folderPath}");

            string sampleFile = Path.Combine(folderPath, "sample.txt");

            // Запись в файл построчно
            File.WriteAllLines(sampleFile, new string[]
            {
                "Первая строка",
                "Вторая строка",
                "Третья строка"
            }, Encoding.UTF8);

            Console.WriteLine($"Файл {Path.GetFileName(sampleFile)} создан и записан.");

            // Чтение файла построчно
            string[] lines = File.ReadAllLines(sampleFile, Encoding.UTF8);
            Console.WriteLine("Содержимое файла построчно:");
            foreach (var line in lines)
                Console.WriteLine(" - " + line);

            // Добавление строки в конец файла
            using (StreamWriter sw = File.AppendText(sampleFile))
            {
                sw.WriteLine("Добавленная строка");
            }
            Console.WriteLine("Добавлена строка в конец файла.");

            // Удаление файла sample.txt
            if (File.Exists(sampleFile))
            {
                File.Delete(sampleFile);
                Console.WriteLine($"Файл {Path.GetFileName(sampleFile)} удалён.");
            }

            // Работа с todo файлом: переименование
            string backupFile = Path.Combine(folderPath, "todos_backup.txt");
            if (File.Exists(todoFile))
            {
                File.Move(todoFile, backupFile, overwrite:true);
                Console.WriteLine($"Файл {Path.GetFileName(todoFile)} переименован в {Path.GetFileName(backupFile)}.");
                File.Move(backupFile, todoFile, overwrite:true);
                Console.WriteLine($"Файл {Path.GetFileName(backupFile)} возвращён обратно.");
            }
            else
            {
                Console.WriteLine($"Файл {Path.GetFileName(todoFile)} не найден для демонстрации переименования.");
            }

            // Список файлов в папке
            var files = Directory.GetFiles(folderPath);
            Console.WriteLine("Файлы в папке data:");
            foreach (var f in files)
            {
                Console.WriteLine(" - " + Path.GetFileName(f));
            }

            // Удаление папки (если пустая)
            string emptyFolder = Path.Combine(folderPath, "emptyFolder");
            if (!Directory.Exists(emptyFolder))
                Directory.CreateDirectory(emptyFolder);
            Console.WriteLine("Создана пустая папка emptyFolder");

            if (Directory.Exists(emptyFolder))
            {
                Directory.Delete(emptyFolder);
                Console.WriteLine("Папка emptyFolder удалена");
            }

            Console.WriteLine("\n=== Конец демонстрации файлов ===\n");
        }
    }

    class OopDemo
    {
        public static void Show()
        {
            Console.WriteLine("\n=== Демонстрация ООП ===\n");

            Person p = new Person("Андрей", "Жданов", 17);
            Console.WriteLine(p.FullName);
            Console.WriteLine($"Возраст: {p.Age}");

            Employee e = new Employee("Егор", "Емелин", 18, "Разработчик", 50000);
            Console.WriteLine(e.GetInfo());

            Company company = new Company("Acme");
            company.AddEmployee(e);
            Console.WriteLine($"Компания: {company.Name} сотрудников: {company.EmployeeCount}");

            var product = new Product("Книга", 300);
            Console.WriteLine(product);

            Console.WriteLine($"Константа TAX: {Constants.TaxRate}");
            Console.WriteLine($"Уникальный id продукта: {product.Id}");

            Console.WriteLine("\n=== Конец ООП демонстрации ===\n");
        }
    }

    class Person
    {
        private string firstName;
        private string lastName;
        private int birthYear;
        public static int Population { get; private set; }
        public readonly Guid InstanceId;
        public Person(string firstName, string lastName, int birthYear)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.birthYear = birthYear;
            InstanceId = Guid.NewGuid();
            Population++;
        }
        public string FullName => $"{firstName} {lastName}";
        public int Age => DateTime.Now.Year - birthYear;
        ~Person()
        {
            Population--;
        }
    }

    class Employee : Person
    {
        private string position;
        private decimal salary;
        public Employee(string firstName, string lastName, int birthYear, string position, decimal salary)
            : base(firstName, lastName, birthYear)
        {
            this.position = position;
            this.salary = salary;
            Department = "General";
        }
        public string Department { get; set; }
        public string GetInfo()
        {
            return $"{FullName} / {Department} / {position} / Зарплата: {salary:C0}";
        }
    }

    class Company
    {
        private readonly List<Employee> employees = new List<Employee>();
        public string Name { get; }
        public int EmployeeCount => employees.Count;

        public Company(string name)
        {
            Name = name;
        }

        public void AddEmployee(Employee e)
        {
            employees.Add(e);
        }
    }

    class Product
    {
        private static int nextId = 1;
        public int Id { get; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Product(string name, decimal price)
        {
            Id = nextId++;
            Name = name;
            Price = price;
        }

        public override string ToString() => $"Продукт {Name}, цена: {Price}";
    }

    static class Constants
    {
        public const double TaxRate = 0.2;
    }

    static class AdvancedOopDemo
    {
        public static void Show()
        {
            Console.WriteLine("\n=== Расширенная демонстрация ООП ===\n");

            // Абстрактный класс и наследование
            Shape circle = new Circle(5);
            Shape rectangle = new Rectangle(4, 6);

            Console.WriteLine($"Площадь круга: {circle.Area():F2}");
            Console.WriteLine($"Площадь прямоугольника: {rectangle.Area():F2}");

            // Интерфейсы
            IPlayable player = new MusicPlayer();
            player.Play();
            player.Pause();
            player.Stop();

            // Паттерн Стратегия
            Context context = new Context(new ConcreteStrategyA());
            context.ExecuteStrategy();
            context.Strategy = new ConcreteStrategyB();
            context.ExecuteStrategy();

            // Паттерн Команда
            Receiver receiver = new Receiver();
            ICommand command = new ConcreteCommand(receiver);
            Invoker invoker = new Invoker();
            invoker.SetCommand(command);
            invoker.ExecuteCommand();

            Console.WriteLine("\n=== Конец расширенной демонстрации ООП ===\n");
        }
    }

    abstract class Shape
    {
        public abstract double Area();
    }

    class Circle : Shape
    {
        public double Radius { get; }
        public Circle(double radius) => Radius = radius;
        public override double Area() => Math.PI * Radius * Radius;
    }

    class Rectangle : Shape
    {
        public double Width { get; }
        public double Height { get; }
        public Rectangle(double width, double height)
        {
            Width = width;
            Height = height;
        }
        public override double Area() => Width * Height;
    }

    interface IPlayable
    {
        void Play();
        void Pause();
        void Stop();
    }

    class MusicPlayer : IPlayable
    {
        public void Play() => Console.WriteLine("Музыка воспроизводится");
        public void Pause() => Console.WriteLine("Музыка на паузе");
        public void Stop() => Console.WriteLine("Музыка остановлена");
    }

    interface IStrategy
    {
        void AlgorithmInterface();
    }

    class ConcreteStrategyA : IStrategy
    {
        public void AlgorithmInterface() => Console.WriteLine("Стратегия А выполняется");
    }

    class ConcreteStrategyB : IStrategy
    {
        public void AlgorithmInterface() => Console.WriteLine("Стратегия B выполняется");
    }

    class Context
    {
        public IStrategy Strategy { get; set; }
        public Context(IStrategy strategy) => Strategy = strategy;
        public void ExecuteStrategy() => Strategy.AlgorithmInterface();
    }

    interface ICommand
    {
        void Execute();
    }

    class ConcreteCommand : ICommand
    {
        private readonly Receiver receiver;
        public ConcreteCommand(Receiver receiver) => this.receiver = receiver;
        public void Execute() => receiver.Action();
    }

    class Receiver
    {
        public void Action() => Console.WriteLine("Получатель выполняет действие");
    }

    class Invoker
    {
        private ICommand command;
        public void SetCommand(ICommand command) => this.command = command;
        public void ExecuteCommand() => command?.Execute();
    }
}