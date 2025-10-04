
using System.Threading.Channels;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Работу выполнили: Амелина Яна и Кабанова Арина");
        Console.WriteLine("ВВедите свое имя");
        string name = Console.ReadLine();
        Console.WriteLine("Введите свою фамилию");
        string surname = Console.ReadLine();
        Console.WriteLine("ВВедите свою год рождения");
        var yearOfBirth = int.Parse(Console.ReadLine());
        var currentYear = 2025;
        var age = currentYear - yearOfBirth;
        Console.WriteLine("Добавлен пользователь " + name + " " + surname + ", Возраст " + age);
        int arrayLength = 2;
        string[] todos = new string[arrayLength];
        bool isOpen = true;
        int currentTaskID = 0;
        Console.ReadKey();
        while (isOpen)
        {
            Console.Clear();
            string userCommand = "";
            Console.WriteLine("Введите команду:\nдля помощи напиши команду help");
            userCommand = Console.ReadLine();
            switch (userCommand)
            {
                case "help":
                    Console.WriteLine("help - выводит список всех доступных команд\nprofile - выводит ваши данные\nadd - добавляет новую задачу (add \"Новая задача\")\nview - просмотр задач\nexit - выйти");
                    break;
                case "profile":
                    Console.WriteLine("Пользователь: " + name + " " + surname + ", Возраст " + age);
                    break;
                case string addCommand when addCommand.StartsWith("add \""):
                    if (currentTaskID == todos.Length)
                    {
                        arrayLength *= 2;
                        string[] tempTodos = new string[arrayLength];
                        for (int i = 0; i < todos.Length; i++)
                            tempTodos[i] = todos[i];
                        todos = tempTodos;
                    }
                    string[] taskText = addCommand.Split('\"', 3);
                    todos[currentTaskID] = taskText[1];
                    currentTaskID++;
                    break;
                case "view":
                    Console.WriteLine("Ваш список задач:");
                    for (int i = 0; i < todos.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(todos[i]))
                            Console.WriteLine(todos[i]);
                    }
                    break;
                case "exit":
                    isOpen = false;
                    break;
                default:
                    Console.WriteLine("Неправильно введена команда");
                    break;

            }
            Console.ReadKey();

        }
    }
}