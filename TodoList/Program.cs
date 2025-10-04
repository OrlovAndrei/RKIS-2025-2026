namespace TodoList
{
    class Program
    {
        public static void Main()
        {
            Console.WriteLine();
            Console.Write("Введите ваше имя: "); 
            string firstName = Console.ReadLine();
            Console.Write("Введите вашу фамилию: ");
            string lastName = Console.ReadLine();

            Console.Write("Введите ваш год рождения: ");
            int year = int.Parse(Console.ReadLine());

            Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст - {DateTime.Now.Year - year}");
            
            string[] todos = new string[2];

            for(;;)
            {
                Console.Write("\nВведите команду: ");
                string command = Console.ReadLine();

                if (command == "help")
                {
                    Console.WriteLine("""
                    Доступные команды:
                    help — список команд
                    profile — выводит данные профиля
                    add "текст задачи" — добавляет задачу
                    view — просмотр всех задач
                    exit — завершить программу
                    """);
                }
                else if (command == "profile")
                {
                    Console.WriteLine($"{firstName} {lastName}, {year}");
                }
                else if (command.StartsWith("add "))
                {
                    string task = command.Split("add ")[1];
                    int index = Array.FindIndex(todos, t => !string.IsNullOrWhiteSpace(t));
                    if (index == todos.Length)
                    {
                        string[] newTodos = new string[todos.Length * 2];
                        for (int i = 0; i < todos.Length; i++)
                        {
                            newTodos[i] = todos[i];
                        }
                        todos = newTodos;
                    }

                    todos[index] = task;

                    Console.WriteLine($"Задача добавлена: {task}");
                }
                else if (command == "view")
                {
                    Console.WriteLine($"Список задач:\n{string.Join("\n", todos.Where(s => !string.IsNullOrWhiteSpace(s)))}");
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