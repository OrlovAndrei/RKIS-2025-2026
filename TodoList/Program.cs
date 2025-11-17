internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Работу выполнил: Морозов Иван 3833.9");
        Console.WriteLine("Введите свое имя");
        string name = Console.ReadLine();
        Console.WriteLine("Ведите свою фамилию");
        string surname = Console.ReadLine();
        Console.WriteLine("Ведите свой год рождения");
        int date = int.Parse(Console.ReadLine());
        int age = 2025 - date;
        Console.WriteLine("Добавлен пользователь " + name + " " + surname + ", Возраст " + age);
        
        int arrayLength = 2;
        string[] todos = new string[arrayLength];
        
        while (true)
        {
            Console.WriteLine("Введите команду: для помощи напиши команду help");
            string userCommand = Console.ReadLine();
            switch (userCommand.Split()[0])
            {
                case "help":
	                Console.WriteLine("help - выводит список всех доступных команд\n" +
	                                  "profile - выводит ваши данные\n" +
	                                  "add - добавляет новую задачу\n" +
	                                  "view - просмотр задач");
                    break;
                case "profile":
                    Console.WriteLine("Пользователь: " + name + " " + surname + ", Возраст " + age);
                    break;
                case "add":
                    for (int i = 0; i < arrayLength; i++)
                    {
                        if (string.IsNullOrEmpty(todos[i]))
                        {
                            Console.WriteLine("Напишите задачу которую необходимо добавить");
                            todos[i] = Console.ReadLine();
                            break;
                        }
                    }
                    break;
                case "view":
                    for (int i = 0;i < arrayLength; i++)
                    {
                        if (!string.IsNullOrEmpty(todos[i]))
                            Console.WriteLine(todos[i]);
                    }
                    break;
                default:
                    Console.WriteLine("Неправильно введена команда");
                    break;
            }
        }
    }
}