

Console.WriteLine("Работа Столяровой и Аракелян");

Console.Write("Введите ваше имя: ");
string? firstName = Console.ReadLine();

if (firstName == null || firstName == "")
{
    throw new Exception("Вы не ввели ваше имя!");
}

Console.Write("Введите вашу фамилию: ");
string? lastName = Console.ReadLine();

if (lastName == null || lastName == "")
{
    throw new Exception("Вы не ввели фамилию!");
}

Console.Write("Введите ваш год рождения: ");
string? yearBirthString = Console.ReadLine();

if (yearBirthString == null || yearBirthString == "")
{
    throw new Exception("Вы не ввели год рождения!");
}

int yearBirth;

if (!int.TryParse(yearBirthString, out yearBirth))
{
    throw new Exception("Вы ввели не цифровое значение! - требуется год рождения.");
}

if (yearBirth < 1930 || yearBirth > DateTime.Now.Year)
{
    throw new Exception("Вы ввели некорректный год рождения!");
}

int age = DateTime.Now.Year - yearBirth;

Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст – {age}");


string[] todos = new string[2];

bool work = true;

while (work)
{

    Console.Write("\nвведите комманду:");
    string command = Console.ReadLine();

    switch (command.Trim())
    {
        case "help":
            Console.WriteLine("---Список доступных комманд---");
            Console.WriteLine("help - выводит текущее сообщение");
            Console.WriteLine("profile - выводит данные пользователя");
            Console.WriteLine("add - добавляет новую задачу (add текст задачи)");
            Console.WriteLine("view - выводит все задачи");
            Console.WriteLine("exit - заверешние программы");
            break;

        case "profile":
            Console.WriteLine($"{firstName} {lastName}, {yearBirth}");
            break;

        case "exit":
            Console.WriteLine("До свидания!");
            work = false;
            break;

        case "view":
            foreach (string task in todos)
            {
                if (task != null)
                {
                    Console.WriteLine(task);
                }
            }
            break;
    }

    //add задача 1 прикол

    if (command.StartsWith("add ")) //если команда начинается с "add "
    {
        var s = command.Split(" ", 2);
     
        var element = -1; //порядковый номер первого свободного элемента массива

        for (int i = 0; i < todos.Length; i++)
        {
            if (todos[i] == null)
            {
                element = i;
                break;
            }
        }

        if (element == -1) //если массив уже заполнен
        {
            string[] newtodos = new string[todos.Length * 2];

            for (int i = 0; i < todos.Length; i++)
            {
                newtodos[i] = todos[i];
                element = i + 1;
            }

            todos = newtodos;
        }

        todos[element] = s[1]; //добавляем задачу
    }


}

