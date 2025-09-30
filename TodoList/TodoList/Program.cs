
using System.Threading.Channels;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Работу выполнили: Амелина Яна и Кабанова Арина");
        Console.WriteLine("ВВедите свое имя");
        string name = Console.ReadLine();
        Console.WriteLine("Введите свою фамилию");
        string Surname = Console.ReadLine();
        Console.WriteLine("ВВедите свою год рождения");
        string date1 = Console.ReadLine();
        int date2 = int.Parse(date1);
        int date3 = 2025;
        int age = date3 - date2;
        Console.WriteLine("Добавлен пользователь " + name + " " + Surname + ", Возраст " + age);
        int arrayLength = 2;
        string[] todos = new string[arrayLength];
        bool isOpen = true;
        while (isOpen)
        {
            Console.Clear();
            string userCommand = "";
            Console.WriteLine("Введите команду:\nдля помощи напиши команду help");
            userCommand = Console.ReadLine();
            switch (userCommand)
            {
                case "help":
                    Console.WriteLine("help - выводит список всех доступных команд\nprofile - выводит ваши данные\nadd - добавляет новую задачу\nview - просмотр задач\nexit - выйти");
                    break;
                case "profile":
                    Console.WriteLine("Пользователь: " + name + " " + Surname + ", Возраст " + age);
                    break;
                case "add":
                    int countNULL = 0;
                    for (int i = 0; i < todos.Length; i++)
                    {
                        if (string.IsNullOrEmpty(todos[i]))
                            countNULL++;
                    }
                    if (countNULL == 0)
                    {
                        arrayLength *= 2;
                        string[] tempTodos = new string[arrayLength];
                        for (int i = 0;i< todos.Length;i++)
                            tempTodos[i] = todos[i];
                        todos = tempTodos;
                    }
                    for (int i = 0; i < todos.Length; i++)
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
                    Console.WriteLine("Ваш список задач");
                    for (int i = 0;i < todos.Length; i++)
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
            Console.ReadLine();



        }
    }
}