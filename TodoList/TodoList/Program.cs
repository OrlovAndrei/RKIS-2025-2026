
using System.Threading.Channels;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Работу выполнили: Амелина Яна и Кабанова Арина");
        string name, surname;
        int age, currentYear = 2025, yearOfBirth;
        AddUser(out name, out surname, currentYear, out yearOfBirth, out age);
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
    static void AddUser (out string name, out string surname, int currentYear, out int yearOfBirth, out int age)
    {
        Console.WriteLine("Напишите ваше имя и фамилию:");
        string fullName = Console.ReadLine();
        string[] splitFullName = fullName.Split(' ', 2);
        name = splitFullName[0];
        surname = splitFullName[1];
        Console.WriteLine("Напишите свой год рождения:");
        yearOfBirth = int.Parse (Console.ReadLine());
        age = currentYear - yearOfBirth;
        Console.WriteLine("Добавлен пользователь " + name + " " + surname + ", Возраст " + age);
        

    }
}