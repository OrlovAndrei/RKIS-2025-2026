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
                else if (trimmed.StartsWith("add ")) { AddTodo(trimmed.Split(" ", 2)[1]); SaveTodosToFile(); }
                else if (trimmed.StartsWith("done ")) { DoneTodo(int.Parse(trimmed.Split(" ", 2)[1])); SaveTodosToFile(); }
                else if (trimmed.StartsWith("update ")) { UpdateTodo(trimmed.Split(" ", 3)[1], trimmed.Split(" ", 3)[2]); SaveTodosToFile(); }
                else if (trimmed.StartsWith("remove ")) { RemoveTodo(int.Parse(trimmed.Split(" ", 2)[1])); SaveTodosToFile(); }
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
            Console.WriteLine("done - отметить задачу выполненной");
            Console.WriteLine("update \"текст\" - обновить задачу");
            Console.WriteLine("remove - удалить задачу");
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
        public string Name { get; private set; }
        public Company(string name)
        {
            Name = name;
        }
        public int EmployeeCount => employees.Count;
        public void AddEmployee(Employee e)
        {
            if (e != null) employees.Add(e);
        }
        public bool RemoveEmployee(Employee e)
        {
            return employees.Remove(e);
        }
    }

    static class Utilities
    {
        public static string CombineNames(string a, string b) => $"{a} {b}";
        public static bool ValidateName(string s) => !string.IsNullOrWhiteSpace(s);
    }

    class Product
    {
        private static int counter = 0;
        public int Id { get; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public Product(string name, decimal price)
        {
            Id = System.Threading.Interlocked.Increment(ref counter);
            SetName(name);
            SetPrice(price);
        }
        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("name");
            Name = name;
        }
        public void SetPrice(decimal price)
        {
            if (price < 0) throw new ArgumentOutOfRangeException(nameof(price));
            Price = price;
        }
        public override string ToString() => $"{Id}: {Name} - {Price:C0}";
    }

    static class Constants
    {
        public const decimal TaxRate = 0.2m;
    }

    static class AdvancedOopDemo
    {
        public static void Show()
        {
            Console.WriteLine("\n=== Расширенная демонстрация ООП ===\n");

            Animal a1 = new Dog("Бим");
            Animal a2 = new Cat("Мурка");
            a1.MakeSound();
            a2.MakeSound();

            Dog maybeDog = a1 as Dog;
            if (maybeDog != null) maybeDog.Fetch();

            Mammal m = new Dog("Рекс");
            m.ShowType();

            SealExample se = new SealExample("Сейл");
            Console.WriteLine(se.Info());

            BaseProcessor csv = new CsvProcessor();
            csv.Process();

            CompressionContext ctx = new CompressionContext(new ZipStrategy());
            ctx.Compress("файл.txt");
            ctx.SetStrategy(new RarStrategy());
            ctx.Compress("файл2.txt");

            Light light = new Light();
            ICommand cmdOn = new LightOnCommand(light);
            ICommand cmdOff = new LightOffCommand(light);
            Invoker invoker = new Invoker();
            invoker.SetCommand(cmdOn);
            invoker.Run();
            invoker.SetCommand(cmdOff);
            invoker.Run();

            Console.WriteLine("\n=== Конец расширенной демонстрации ООП ===\n");
        }
    }

    abstract class Animal
    {
        protected string name;
        public Animal(string name) { this.name = name; }
        public abstract void MakeSound();
        public virtual void ShowType() => Console.WriteLine($"Животное: {name}");
    }

    class Mammal : Animal
    {
        public Mammal(string name) : base(name) { }
        public override void MakeSound() => Console.WriteLine($"{name} издает звук (млекопитающее)");
    }

    sealed class Dog : Mammal
    {
        private int energy = 100;
        public Dog(string name) : base(name) { }
        public override void MakeSound() => Console.WriteLine($"{name}: Гав!");
        public void Fetch()
        {
            energy -= 10;
            Console.WriteLine($"{name} приносит палку. Энергия: {energy}");
        }
    }

    class Cat : Mammal
    {
        protected int mood = 5;
        public Cat(string name) : base(name) { }
        public override void MakeSound() => Console.WriteLine($"{name}: Мяу!");
    }

    abstract class BaseProcessor
    {
        public void Process()
        {
            StepOne();
            StepTwo();
            StepThree();
        }
        protected abstract void StepOne();
        protected abstract void StepTwo();
        protected virtual void StepThree() => Console.WriteLine("Шаг 3 (по умолчанию)");
    }

    class CsvProcessor : BaseProcessor
    {
        protected override void StepOne() => Console.WriteLine("CSV: чтение");
        protected override void StepTwo() => Console.WriteLine("CSV: парсинг");
        protected override void StepThree() => Console.WriteLine("CSV: запись результата");
    }

    interface ICompressionStrategy
    {
        void CompressFile(string fileName);
    }

    class ZipStrategy : ICompressionStrategy
    {
        public void CompressFile(string fileName) => Console.WriteLine($"Compress {fileName} using ZIP");
    }

    class RarStrategy : ICompressionStrategy
    {
        public void CompressFile(string fileName) => Console.WriteLine($"Compress {fileName} using RAR");
    }

    class CompressionContext
    {
        private ICompressionStrategy strategy;
        public CompressionContext(ICompressionStrategy strategy) { this.strategy = strategy; }
        public void SetStrategy(ICompressionStrategy s) => strategy = s;
        public void Compress(string file) => strategy.CompressFile(file);
    }

    interface ICommand
    {
        void Execute();
    }

    class Light
    {
        private bool isOn = false;
        public void On() { isOn = true; Console.WriteLine("Свет включен"); }
        public void Off() { isOn = false; Console.WriteLine("Свет выключен"); }
    }

    class LightOnCommand : ICommand
    {
        private Light light;
        public LightOnCommand(Light l) { light = l; }
        public void Execute() => light.On();
    }

    class LightOffCommand : ICommand
    {
        private Light light;
        public LightOffCommand(Light l) { light = l; }
        public void Execute() => light.Off();
    }

    class Invoker
    {
        private ICommand command;
        public void SetCommand(ICommand cmd) => command = cmd;
        public void Run() => command?.Execute();
    }

    sealed class SealExample
    {
        private string name;
        public SealExample(string name) => this.name = name;
        public string Info() => $"Sealed class example: {name}";
    }
}