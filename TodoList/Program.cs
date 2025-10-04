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
            
            string[] todos = new string[2];
            int index = 0;
            
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
                else if (command.StartsWith("add "))
                {
                    string task = command.Split("add ")[1];
                    if (index == todos.Length)
                    {
                        string[] newTodos = new string[todos.Length*2];
                        for (int i = 0; i < todos.Length; i++)
                        {
                            newTodos[i] = todos[i];
                        }
                        
                        todos = newTodos;
                    }

                    todos[index] = task;
                    index = index + 1;
                    
                    Console.WriteLine("Добавлена задача: " + task);
                }
                else if (command == "view")
                {
                    Console.WriteLine("Задачи:");
                    foreach (string todo in todos)
                    {
                        if (!string.IsNullOrEmpty(todo))
                        {
                            Console.WriteLine(todo);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Неизвестная команда.");
                }
            }
        }
    }
}