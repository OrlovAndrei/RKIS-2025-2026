
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
                    HelpInfo();
                    break;
                case "profile":
                    UserInfo("Пользователь: ", name, surname, age);
                    break;
                case string addCommand when addCommand.StartsWith("add \""):
                    if (currentTaskID == todos.Length)
                    ArrayExpension(ref todos);
                    AddTask(ref todos, ref currentTaskID, addCommand);
                    break;
                case "view":
                    TodoInfo(todos);
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

    private static void TodoInfo(string[] todos)
    {
        Console.WriteLine("Ваш список задач:");
        for (int i = 0; i < todos.Length; i++)
        {
            if (!string.IsNullOrEmpty(todos[i]))
                Console.WriteLine(todos[i]);
        }
    }

    private static void HelpInfo()
    {
        Console.WriteLine("help - выводит список всех доступных команд\nprofile - выводит ваши данные\nadd - добавляет новую задачу (add \"Новая задача\")\nview - просмотр задач\nexit - выйти");
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
        string userAdded = "Добавлен пользователь: ";
        UserInfo (userAdded, name, surname, age);
        
    }
    private static void UserInfo (string userInfo, string name, string surname, int age)
    {
        Console.WriteLine(userInfo + name + " " + surname + ", возраст: " + age);
    }
    private static void ArrayExpension (ref string[] array)
    {
        string[] tempArray = new string[array.Length*2];
        for (int i = 0; i < array.Length; i++) 
        tempArray[i] = array[i];
        array = tempArray;
    }
    private static void AddTask (ref string[] todoArray, ref int currentTaskID, string task)
    {
        string[] taskText = task.Split('\"', 3);
        todoArray[currentTaskID] = taskText[1];
        currentTaskID++;

    }
}