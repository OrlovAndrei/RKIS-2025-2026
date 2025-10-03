using System;

public class UserInfo{

    // Тупо чтобы не городить лишних полей
    struct UserData
    {
        public string firstName, lastName, birthYearString;
        public int currentYear, age, birthYear;
    }

    // По порядку задач
    // 1. Продолжайте работу в проекте TodoList, созданном ранее
    // 2. Создайте массив строк todos , в котором будут храниться задачи.
    private static string[] todos;
    private static int todosCount, todosLen;

    private static UserData userData = new UserData();
    private static bool isProgramRunning = true;

    private static int todosStartLen = 2;


    // 4. Реализуйте следующие команды:
    // Далее реализации всех комманд
    private static void DoHelp()
    {
        Console.WriteLine("****\tUserInfo Помощник\t****");
        Console.WriteLine("profile — выводит данные пользователя в формате: <Имя> <Фамилия>, <Год рождения> .");
        Console.WriteLine("add — добавляет новую задачу. Формат ввода: add \"текст задачи\"");
        Console.WriteLine("view — выводит все задачи из массива (только непустые элементы).");
        Console.WriteLine("exit — завершает цикл и останавливает выполнение программы.");
    }

    private static void DoExit() => isProgramRunning = false;

    private static void DoAdd(string command)
    {
        string newTodo = "";
        int newTodosCount = todosCount + 1;
        var task = command.Split("add ");

        foreach (var item in task)
        {
            newTodo += item;
        }

        Console.WriteLine($"Added new task: \"{newTodo}\".");

        // 5. Расширение массива 'todos'
        if (newTodosCount > todosLen)
        {
            int newTodosLen = todosLen * 2;
            string[] newTodos = new string[newTodosLen];

            for (int i = 0;i < todosCount;i++)
            {
                newTodos[i] = todos[i];
            }

            newTodos[todosCount] = newTodo;
            todos = newTodos;
            todosLen = newTodosLen;
        }
        else
        {
            todos[todosCount] = newTodo;
        }

        todosCount = newTodosCount;
    }

    private static void DoProfile()
    {
        Console.WriteLine($"User Data: \"{userData.firstName}\" \"{userData.lastName}\", {userData.birthYear}");
    }

    private static void DoView()
    {
        Console.WriteLine("===========================================================");
        Console.WriteLine("****\tAll tasks list\t****");
        for (int i = 0;i < todosCount;i++)
        {
            Console.WriteLine($"\t\"{todos[i]}\"");
        }
        Console.WriteLine("===========================================================");
    }


    // Эта байда обрабатывает комманду введенную юзером
    private static void ProcessCommand(string command)
    {
        if (command == "help")
        { 
            DoHelp();
            return;
        }

        if (command == "exit")
        {
            DoExit();
            return;
        }

        if (command.StartsWith("add"))
        {
            DoAdd(command);
            return;
        }

        if (command == "profile")
        {
            DoProfile();
            return;
        }

        if (command == "view")
        {
            DoView();
            return;
        }

        Console.WriteLine("Неизвестная комманда!");
    }

    public static void Main(string[] args){
        Console.WriteLine("Работу выполнили Чернова Юлия и Соловьев Иван 3833");

        Console.WriteLine("Введите имя:");
        userData.firstName = Console.ReadLine();

        Console.WriteLine("Введите фамилию:");
        userData.lastName = Console.ReadLine();
        Console.WriteLine("Введите год рождения:");
        userData.birthYearString = Console.ReadLine();

        bool isBirthYearValid = int.TryParse(userData.birthYearString, out userData.birthYear);

        if (!isBirthYearValid)
        {
            Console.WriteLine("Ошибка: Некорректный год рождения. Пожалуйста, введите целое число.");
            return;
        }

        userData.currentYear = DateTime.Now.Year;
        userData.age = userData.currentYear - userData.birthYear;

        Console.WriteLine($"Добавлен пользователь {userData.firstName} {userData.lastName}, возраст - {userData.age}");

        // Инициализация 'todos'
        todosCount = 0;
        todosLen = todosStartLen;
        todos = new string[todosStartLen];

        // 3. Сделайте бесконечный цикл, в котором будет проверяться введённая пользователем команда.
        while (isProgramRunning)
        {
            // Получаем комманду от юзера
            var command = Console.ReadLine();

            // И ее обрабатываем
            ProcessCommand(command);
        }
    }
}