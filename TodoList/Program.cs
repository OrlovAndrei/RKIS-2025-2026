namespace TodoList
{
    class Program
    {
        public static void Main()
        {
            Console.WriteLine("Работу выполнил Кулаков");
            Console.Write("Введите имя: "); 
            string name = Console.ReadLine();
            Console.Write("Введите фамилию: ");
            string surname = Console.ReadLine();

            Console.Write("Введите год рождения: ");
            int year = int.Parse(Console.ReadLine());
            int age = DateTime.Now.Year - year;

            Console.WriteLine($"Добавлен пользователь {name} {surname}, возраст - {age}");
            
            while(true)
            {
                Console.Write("\nВведите команду: ");
                string command = Console.ReadLine();

                if (command == "help")
                {
                    Console.WriteLine("""
                    Доступные команды:
                    help — список команд
                    profile — выводит данные профиля
                    exit — завершить программу
                    """);
                }
                else if (command == "profile")
                {
                    Console.WriteLine($"{name} {surname}, {year}");
                }
                else if (command == "exit")
                {
                    Console.WriteLine("Программа завершена.");
                    break;
                }
                else
                {
                    Console.WriteLine("Неизвестная команда. Введите help для списка команд.");
                }
            }
        }
    }
}