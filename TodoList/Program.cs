using TodoList.commands;

namespace TodoList
{
    class Program
    {
        private static string _dataDirectory = "Data";
        public static string ProfileFilePath => Path.Combine(_dataDirectory, "profile.txt");
        static void Main()
        {
            Console.WriteLine("Работу выполнили: Вдовиченко и Кравец");

            Console.Write("Введите ваше имя: ");
            string name = Console.ReadLine();
            Console.Write("Введите вашу фамилию: ");
            string surname = Console.ReadLine();
            Console.Write("Введите год рождения: ");
            int year = int.Parse(Console.ReadLine());

            Profile profile = new Profile(name, surname, year);
            TodoList todoList = new TodoList();

            Console.WriteLine($"Добавлен пользователь: {profile.GetInfo()}");
            Console.WriteLine("Введите help для списка команд.");

            while (true)
            {
                Console.Write("\nВведите команду: ");
                string input = Console.ReadLine();

                ICommand command = CommandParser.Parse(input, todoList, profile);
                command?.Execute();
            }
        }
    }
}
