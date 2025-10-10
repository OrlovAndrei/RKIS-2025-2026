using System;
using System.Text;

namespace TodoList
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("Работу выполнили Буряк Степан Геннадьевич и Голубев Данил Сергеевич");

            Console.Write("Введите имя: ");
            string? firstName = Console.ReadLine();

            Console.Write("Введите фамилию: ");
            string? lastName = Console.ReadLine();

            Console.Write("Введите год рождения: ");
            string? birthYearInput = Console.ReadLine();

            if (!int.TryParse(birthYearInput, out int birthYear))
            {
                Console.WriteLine("Некорректный год рождения. Пожалуйста, введите число.");
                return;
            }

            int currentYear = DateTime.Now.Year;
            int age = currentYear - birthYear;

            firstName = string.IsNullOrWhiteSpace(firstName) ? "Имя" : firstName.Trim();
            lastName = string.IsNullOrWhiteSpace(lastName) ? "Фамилия" : lastName.Trim();

            Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст - {age}");

            // Инициализация массива задач и бесконечный цикл команд
            string[] todos = new string[2];
            int todosCount = 0;

            Console.WriteLine("Введите 'help' чтобы увидеть список команд.");

            while (true)
            {
                Console.Write("> ");
                string? input = Console.ReadLine();
                if (input == null)
                {
                    continue;
                }

                string command = input.Trim();
                if (command.Length == 0)
                {
                    continue;
                }

                string verb = command.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[0].ToLowerInvariant();

                if (verb == "help")
                {
                    Console.WriteLine("Доступные команды:");
                    Console.WriteLine("help   — список доступных команд");
                    Console.WriteLine("profile— вывод данных пользователя");
                    Console.WriteLine("add \"текст задачи\" — добавить новую задачу");
                    Console.WriteLine("view   — показать все задачи");
                    Console.WriteLine("exit   — выход из программы");
                    continue;
                }

                if (verb == "exit")
                {
                    Console.WriteLine("Выход...");
                    break;
                }

                if (verb == "add")
                {
                    // Ожидается формат: add "текст задачи"
                    var parts = input.Split('"');
                    if (parts.Length >= 2)
                    {
                        string task = parts[1];
                        if (string.IsNullOrWhiteSpace(task))
                        {
                            Console.WriteLine("Пустой текст задачи. Используйте: add \"текст задачи\"");
                            continue;
                        }

                        // Проверка места, расширение массива при необходимости
                        if (todosCount >= todos.Length)
                        {
                            string[] bigger = new string[todos.Length * 2];
                            for (int i = 0; i < todos.Length; i++)
                            {
                                bigger[i] = todos[i];
                            }
                            todos = bigger;
                        }

                        todos[todosCount] = task.Trim();
                        todosCount++;
                        Console.WriteLine($"Добавлена задача: \"{task.Trim()}\"");
                        continue;
                    }

                    Console.WriteLine("Некорректный формат. Используйте: add \"текст задачи\"");
                    continue;
                }

                if (verb == "view")
                {
                    if (todosCount == 0)
                    {
                        Console.WriteLine("Список задач пуст.");
                        continue;
                    }

                    Console.WriteLine("Задачи:");
                    for (int i = 0; i < todosCount; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(todos[i]))
                        {
                            Console.WriteLine($"{i + 1}. {todos[i]}");
                        }
                    }
                    continue;
                }

                // Остальные команды будут добавлены далее
                Console.WriteLine("Неизвестная команда. Введите 'help' для списка команд.");
            }
        }
    }
}
