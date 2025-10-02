using System; // Подключение пространства имен System для использования базовых классов

class TodoList // Объявление класса TodoList
{
    static void Main() // Главный метод программы, точка входа
    {
        Console.WriteLine("выполнил работу Турищев Иван"); // Вывод информации об авторе
        int yaerNow = DateTime.Now.Year; // Получение текущего года и сохранение в переменную
        System.Console.WriteLine(yaerNow); // Вывод текущего года
        System.Console.Write("Введите ваше имя: "); // Запрос имени пользователя
        string userName = Console.ReadLine() ?? "Неизвестно"; // Чтение ввода имени
        if (userName.Length == 0) userName = "Неизвестно"; //Проверка на имя 
        System.Console.Write($"{userName}, введите год вашего рождения: "); // Запрос года рождения
        string yaerBirth = Console.ReadLine() ?? "Неизвестно"; // Чтение ввода года рождения
        if (yaerBirth == "") yaerBirth = "Неизвестно"; // Обработка пустого ввода
        int age = -1;  //Инициализация переменной для возраста
        if (int.TryParse(yaerBirth, out age) && age < yaerNow)//Сравнение года с введённым значением 
        {
            System.Console.WriteLine($"Добавлен пользователь {userName}, возрастом {yaerNow-age}");
        }
        else System.Console.WriteLine("Пользователь не ввел возраст"); // Сообщение об ошибке
    }
}
