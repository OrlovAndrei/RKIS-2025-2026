

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

if (yearBirthString == null || yearBirthString=="")
{
    throw new Exception("Вы не ввели год рождения!");
}

int yearBirth;

if(!int.TryParse(yearBirthString, out yearBirth))
{
    throw new Exception("Вы ввели не цифровое значение! - требуется год рождения.");
}

if(yearBirth < 1930 || yearBirth > DateTime.Now.Year)
{
    throw new Exception("Вы ввели некорректный год рождения!");
}

int age = DateTime.Now.Year - yearBirth;

Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст – {age}");


string[] todos = { "задача 1", "задача 2" };

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


    }




