using System;

namespace Todolist
{
    class Program
    {
        static string[] todos = new string[2];
        static bool[] done = new bool[2];
        static DateTime[] dates = new DateTime[2];
        static int taskCount = 0;
        static string firstName = "";
        static string lastName = "";
        static int birthYear = 0;

        static void Main()
        {
            InitializeUser();
            RunTodoList();
        }

        static void InitializeUser()
        {
            Console.WriteLine("Работу выполнили Дайнеко и Гергель 3832");
            Console.WriteLine();
            
            Console.Write("Введите имя: ");
            firstName = Console.ReadLine();
            
            Console.Write("Введите фамилию: ");
            lastName = Console.ReadLine();
            
            Console.Write("Введите год рождения: ");
            birthYear = int.Parse(Console.ReadLine());
            
            int age = DateTime.Now.Year - birthYear;
            Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст = {age}");
            Console.WriteLine();
        }

        static void RunTodoList()
        {
            while (true)
            {
                Console.Write("Введите команду (help - список команд): ");
                string input = Console.ReadLine();
                
                if (input == "help")
                {
                    ShowHelp();
                }
                else if (input == "profile")
                {
                    ShowProfile();
                }
                else if (input.StartsWith("add "))
                {
                    AddTask(input);
                }
                else if (input == "view")
                {
                    ViewTasks();
                }
                else if (input.StartsWith("done "))
                {
                    MarkTaskDone(input);
                }
                else if (input.StartsWith("delete "))
                {
                    DeleteTask(input);
                }
                else if (input.StartsWith("update "))
                {
                    UpdateTask(input);
                }
                else if (input == "exit")
                {
                    Console.WriteLine("Выход из программы...");
                    break;
                }
                else
                {
                    Console.WriteLine("Неизвестная команда. Введите 'help' для списка команд.");
                }
                
                Console.WriteLine();
            }
        }

        static void ShowHelp()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("help - вывести список команд");
            Console.WriteLine("profile - показать данные пользователя");
            Console.WriteLine("add \"текст задачи\" - добавить задачу");
            Console.WriteLine("view - показать все задачи");
            Console.WriteLine("done <номер> - отметить задачу выполненной");
            Console.WriteLine("delete <номер> - удалить задачу");
            Console.WriteLine("update <номер> \"новый текст\" - изменить задачу");
            Console.WriteLine("exit - выйти из программы");
        }

        static void ShowProfile()
        {
            Console.WriteLine($"{firstName} {lastName}, {birthYear} год рождения");
        }

        static void AddTask(string input)
        {
            string[] parts = input.Split('"');
            if (parts.Length >= 2)
            {
                string task = parts[1];
                
                // Проверяем, нужно ли расширять массивы
                if (taskCount >= todos.Length)
                {
                    ResizeArrays();
                }
                
                // Добавляем задачу
                todos[taskCount] = task;
                done[taskCount] = false;
                dates[taskCount] = DateTime.Now;
                taskCount++;
                Console.WriteLine($"Задача добавлена: {task}");
            }
            else
            {
                Console.WriteLine("Ошибка: используйте формат add \"текст задачи\"");
            }
        }

        static void ViewTasks()
        {
            Console.WriteLine("Список задач:");
            bool hasTasks = false;
            for (int i = 0; i < taskCount; i++)
            {
                if (!string.IsNullOrEmpty(todos[i]))
                {
                    string status = done[i] ? "✓ ВЫПОЛНЕНО" : "✗ НЕ ВЫПОЛНЕНО";
                    string date = dates[i].ToString("dd.MM.yyyy HH:mm");
                    Console.WriteLine($"{i + 1}. {todos[i]} [{status}] - {date}");
                    hasTasks = true;
                }
            }
            
            if (!hasTasks)
            {
                Console.WriteLine("Задач нет");
            }
        }

        static void MarkTaskDone(string input)
        {
            if (int.TryParse(input.Substring(5), out int index))
            {
                int taskIndex = index - 1;
                if (taskIndex >= 0 && taskIndex < taskCount)
                {
                    done[taskIndex] = true;
                    dates[taskIndex] = DateTime.Now;
                    Console.WriteLine($"Задача {index} отмечена как выполненная");
                }
                else
                {
                    Console.WriteLine("Ошибка: неверный номер задачи");
                }
            }
            else
            {
                Console.WriteLine("Ошибка: используйте формат done <номер>");
            }
        }

        static void DeleteTask(string input)
        {
            if (int.TryParse(input.Substring(7), out int index))
            {
                int taskIndex = index - 1;
                if (taskIndex >= 0 && taskIndex < taskCount)
                {
                    string deletedTask = todos[taskIndex];
                    
                    // Сдвигаем все элементы влево
                    for (int i = taskIndex; i < taskCount - 1; i++)
                    {
                        todos[i] = todos[i + 1];
                        done[i] = done[i + 1];
                        dates[i] = dates[i + 1];
                    }
                    
                    // Очищаем последний элемент
                    todos[taskCount - 1] = null;
                    done[taskCount - 1] = false;
                    dates[taskCount - 1] = DateTime.MinValue;
                    
                    taskCount--;
                    Console.WriteLine($"Задача удалена: {deletedTask}");
                }
                else
                {
                    Console.WriteLine("Ошибка: неверный номер задачи");
                }
            }
            else
            {
                Console.WriteLine("Ошибка: используйте формат delete <номер>");
            }
        }

        static void UpdateTask(string input)
        {
            string[] parts = input.Split('"');
            if (parts.Length >= 2)
            {
                string numberPart = parts[0].Substring(7).Trim();
                if (int.TryParse(numberPart, out int index))
                {
                    int taskIndex = index - 1;
                    if (taskIndex >= 0 && taskIndex < taskCount)
                    {
                        string newText = parts[1];
                        string oldText = todos[taskIndex];
                        todos[taskIndex] = newText;
                        dates[taskIndex] = DateTime.Now;
                        Console.WriteLine($"Задача обновлена: '{oldText}' -> '{newText}'");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: неверный номер задачи");
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка: используйте формат update <номер> \"новый текст\"");
                }
            }
            else
            {
                Console.WriteLine("Ошибка: используйте формат update <номер> \"новый текст\"");
            }
        }

        static void ResizeArrays()
        {
            int newSize = todos.Length * 2;
            
            string[] newTodos = new string[newSize];
            bool[] newDone = new bool[newSize];
            DateTime[] newDates = new DateTime[newSize];
            
            for (int i = 0; i < todos.Length; i++)
            {
                newTodos[i] = todos[i];
                newDone[i] = done[i];
                newDates[i] = dates[i];
            }
            
            todos = newTodos;
            done = newDone;
            dates = newDates;
            
            Console.WriteLine($"Массивы расширены до {newSize} элементов");
        }
    }
}
