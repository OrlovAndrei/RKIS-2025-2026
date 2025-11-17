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
        int currentTaskNumber = 0;

        while (true)
        {
            Console.WriteLine("Введите команду: ");
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
	                if (currentTaskNumber == todos.Length)
	                {
		                arrayLength *= 2;
		                string[] tempTodos = new string[arrayLength];
		                for (int i = 0; i < todos.Length; i++)
			                tempTodos[i] = todos[i];
		                todos = tempTodos;
	                }
	                string[] taskText = userCommand.Split('\"', 3);
	                todos[currentTaskNumber] = taskText[1];
	                currentTaskNumber++;
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