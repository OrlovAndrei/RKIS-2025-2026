namespace TodoList
{
    class Program
    {
        static Profile profile = new("", "", 2000);
        static TodoList todoList = new();
        static bool isRunning = true;

        public static void Main()
        {
            Console.WriteLine("Работу выполнили Турчин Крошняк");

            Console.Write("Введите ваше имя: ");
            var name = Console.ReadLine();
            Console.Write("Введите вашу фамилию: ");
            var surname = Console.ReadLine();

            Console.Write("Введите ваш год рождения: ");
            int year = int.Parse(Console.ReadLine());
            profile = new Profile(name, surname, year);
            Console.WriteLine($"Добавлен пользователь {profile.GetInfo()}");

            Action exitAction = () => isRunning = false;

            while (isRunning)
            {
                Console.Write("\nВведите команду: ");
                string input = Console.ReadLine();

                ICommand? command = CommandParser.Parse(input, todoList, profile, exitAction);
                if (command != null)
                {
                    command.Execute();
                }
            }
        }
    }
}