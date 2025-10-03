using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Работу выполнили: Должиков и Бут, группа 3834");
        Console.WriteLine("Добро пожаловать в консольный ToDoList!\n");

        // Ввод профиля пользователя
        Console.Write("Введите имя: ");
        string name = Console.ReadLine();

        Console.Write("Введите фамилию: ");
        string surname = Console.ReadLine();

        Console.Write("Введите год рождения: ");
        int year = int.Parse(Console.ReadLine());

        int age = DateTime.Now.Year - year;
        Console.WriteLine($"\nПрофиль создан: {name} {surname}, возраст – {age}\n");

        // Создаём массив задач
        string[] todos = new string[2];
        int taskCount = 0;

        Console.WriteLine("Введите команду (help для справки).");

        // Бесконечный цикл команд
        while (true)
        {
            Console.Write("\n>>> ");
            string input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                continue;

            string[] parts = input.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            string command = parts[0].ToLower();

            switch (command)
            {
                case "help":
                    Console.WriteLine("Доступные команды:");
                    Console.WriteLine(" help      — список всех команд");
                    Console.WriteLine(" profile   — данные пользователя");
                    Console.WriteLine(" add       — добавить задачу (пример: add \"Купить молоко\")");
                    Console.WriteLine(" view      — показать список задач");
                    Console.WriteLine(" exit      — выйти из программы");
                    break;

                case "profile":
                    Console.WriteLine($"{name} {surname}, {year} г.р.");
                    break;

                case "add":
                    if (parts.Length < 2)
                    {
                        Console.WriteLine("Ошибка: укажите текст задачи. Пример: add \"Сделать задание\"");
                        break;
                    }

                    string task = parts[1].Trim('"');

                    // Проверка вместимости массива
                    if (taskCount >= todos.Length)
                    {
                        string[] newTodos = new string[todos.Length * 2];
                        for (int i = 0; i < todos.Length; i++)
                            newTodos[i] = todos[i];
                        todos = newTodos;

                        Console.WriteLine("Массив задач расширен, теперь доступно ещё больше дел!");
                    }

                    todos[taskCount++] = task;
                    Console.WriteLine($"Задача добавлена: \"{task}\"");
                    break;

                case "view":
                    Console.WriteLine("Ваши задачи:");
                    bool empty = true;
                    for (int i = 0; i < taskCount; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(todos[i]))
                        {
                            Console.WriteLine($" {i + 1}. {todos[i]}");
                            empty = false;
                        }
                    }
                    if (empty) Console.WriteLine(" (список пуст)");
                    break;

                case "exit":
                    Console.WriteLine("До свидания! Хорошего дня, " + name + "!");
                    return;

                default:
                    Console.WriteLine("Неизвестная команда. Введите 'help' для списка доступных.");
                    break;
            }
        }
    }
}
