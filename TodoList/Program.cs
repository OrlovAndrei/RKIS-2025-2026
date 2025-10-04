namespace TodoList
{
    class Program
    {
        public static void Main()
        {
            Console.WriteLine("Работу выполнили Зусикова и Кабачек 3833.9");
            Console.WriteLine("Введите ваше имя:"); 
            string name = Console.ReadLine();
            Console.WriteLine("Введите вашу фамилию:");
            string surname = Console.ReadLine();

            Console.WriteLine("Введите ваш год рождения:");
            int year = int.Parse(Console.ReadLine());
            int age = DateTime.Now.Year - year;
            
            Console.WriteLine("Добавлен пользователь " + name + " " + surname + ", возраст - " + age);
            

            while (true)
            {
                Console.WriteLine("Введите команду:");
                string command = Console.ReadLine();

                if (command == "help")
                {
                    Console.WriteLine("Команды:");
                    Console.WriteLine("help — выводит список всех доступных команд с кратким описанием");
                    Console.WriteLine("profile — выводит данные пользователя");
                    Console.WriteLine("add \"текст задачи\" — добавляет новую задачу");
                    Console.WriteLine("view — выводит все задачи");
                    Console.WriteLine("exit — выход из программы");
                }
                else if (command == "profile")
                {
                    Console.WriteLine(name + " " + surname + " - " + age);
                }

                else if (command == "exit")
                {
                    Console.WriteLine("Выход из программы.");
                    break;
                }
                
            }
        }
    }
}