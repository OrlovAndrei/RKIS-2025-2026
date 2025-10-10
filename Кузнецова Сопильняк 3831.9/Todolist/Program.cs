using System;

namespace TodoList
{
    class Program
    {

        private const int INITIAL_ARRAY_SIZE = 2;
        private const int ARRAY_EXPANSION_MULTIPLIER = 2;
        
        static void Main(string[] args)
        {
            Console.WriteLine("Powered by Yar and Angelina");

            
            
            Console.Write("Введите имя: ");
            string firstName = Console.ReadLine();

            Console.Write("Введите фамилию: ");
            string lastName = Console.ReadLine();

            Console.Write("Введите год рождения: ");
            string yearInput = Console.ReadLine();

            int birthYear;
            while (!int.TryParse(yearInput, out birthYear))
            {
                Console.Write("Ошибка! Введите корректный год рождения: ");
                yearInput = Console.ReadLine();
            }

            // Результаты 
            Console.WriteLine($"Добро пожаловать пользователь {firstName} {lastName}, возраст - {birthYear}");

            // Это массивы для задач
            string[] taskDescriptions = new string[INITIAL_ARRAY_SIZE];
            bool[] taskStatuses = new bool[INITIAL_ARRAY_SIZE];
            DateTime[] taskDates = new DateTime[INITIAL_ARRAY_SIZE];
            int taskCount = 0;

            // Wbrk lkz rjvvfyl 
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                switch (input.ToLower())
                {
                    case "help":
                        ShowHelp();
                        break;
                    
                    case "profile":
                        ShowProfile(firstName, lastName, birthYear);
                        break;
                    
                    case "view":
                        ViewTasks(taskDescriptions, taskStatuses, taskDates, taskCount);
                        break;

                    //Ссылочка
                     case "link":
                        Console.WriteLine("https://youtu.be/dQw4w9WgXcQ?si=RqvXF3hYQQogSgMs");
                        break;

                    case "exit":
                        Console.WriteLine("Выход из программы...");
                        return;
                    
                   
                    

                    default:
                        if (input.ToLower().StartsWith("add "))
                        {
                            taskCount = AddTask(input, ref taskDescriptions, ref taskStatuses, ref taskDates, taskCount);
                        }
                        else if (input.ToLower().StartsWith("done "))
                        {
                            taskCount = MarkTaskAsDone(input, taskDescriptions, taskStatuses, taskDates, taskCount);
                        }
                        else if (input.ToLower().StartsWith("delete "))
                        {
                            taskCount = DeleteTask(input, ref taskDescriptions, ref taskStatuses, ref taskDates, taskCount);
                        }
                        else if (input.ToLower().StartsWith("update "))
                        {
                            taskCount = UpdateTask(input, ref taskDescriptions, ref taskStatuses, ref taskDates, taskCount);
                        }
                        else
                        {
                            Console.WriteLine("Неизвестная команда. Введите 'help' для просмотра доступных команд.");
                        }
                        break;
                }
            }
        }




        // Единственное, что может помочь
        static void ShowHelp()
        {
            Console.WriteLine("\n=== Доступные команды ===");
            Console.WriteLine("help - показать этот список команд");
            Console.WriteLine("profile - показать данные пользователя");
            Console.WriteLine("add \"текст задачи\" - добавить новую задачу");
            Console.WriteLine("view - показать все задачи");
            Console.WriteLine("done <индекс> - отметить задачу выполненной");
            Console.WriteLine("delete <индекс> - удалить задачу");
            Console.WriteLine("update <индекс> \"текст\" - обновить текст задачи");
            Console.WriteLine("exit - выйти из программы\n");
            Console.WriteLine("link - показывает ссылочку");
        }

        // Выводит данные пользователя
        static void ShowProfile(string firstName, string lastName, int birthYear)
        {
            Console.WriteLine($"\n=== Профиль пользователя ===");
            Console.WriteLine($"{firstName} {lastName}, {birthYear}\n");
        }



        // Эт чтобы добавить задачу
        static int AddTask(string input, ref string[] descriptions, ref bool[] statuses, ref DateTime[] dates, int taskCount)
        {
            string task = ExtractTaskText(input);

            if (string.IsNullOrWhiteSpace(task))
            {
                Console.WriteLine("Ошибка: Текст задачи не может быть пустым.");
                return taskCount;
            }

            // Проверку добавил
            if (taskCount >= descriptions.Length)
            {
                ExpandArrays(ref descriptions, ref statuses, ref dates);
                Console.WriteLine($"Массивы расширены до {descriptions.Length} элементов");
            }


            descriptions[taskCount] = task;
            statuses[taskCount] = false;
            dates[taskCount] = DateTime.Now;
            taskCount++;

            Console.WriteLine($"Задача добавлена: \"{task}\"");
            Console.WriteLine($"Всего задач: {taskCount}/{descriptions.Length}");

            return taskCount;
        }

        static void ViewTasks(string[] descriptions, bool[] statuses, DateTime[] dates, int taskCount)
        {
            if (taskCount == 0)
            {
                Console.WriteLine("Список задач пуст.");
                return;
            }

            Console.WriteLine("\n=== Список задач ===");
            for (int i = 0; i < taskCount; i++)
            {
                string statusText = statuses[i] ? "Сделано" : "Не сделано";
                string formattedDate = dates[i].ToString("dd.MM.yyyy HH:mm");
                Console.WriteLine($"{i}. {descriptions[i]} [{statusText}] {formattedDate}");
            }
            Console.WriteLine($"Всего задач: {taskCount}\n");
        }

        // Покажет задачи
        static int MarkTaskAsDone(string input, string[] descriptions, bool[] statuses, DateTime[] dates, int taskCount)
        {
            int taskIndex = ExtractTaskIndex(input, "done", taskCount);
            if (taskIndex == -1) return taskCount;

            statuses[taskIndex] = true;
            dates[taskIndex] = DateTime.Now; 
            
            Console.WriteLine($"Задача \"{descriptions[taskIndex]}\" отмечена как выполненная");
            return taskCount;
        }

        // Это удалит задачу
        static int DeleteTask(string input, ref string[] descriptions, ref bool[] statuses, ref DateTime[] dates, int taskCount)
        {
            int taskIndex = ExtractTaskIndex(input, "delete", taskCount);
            if (taskIndex == -1) return taskCount;

            string deletedTask = descriptions[taskIndex];
            
            for (int i = taskIndex; i < taskCount - 1; i++)
            {
                descriptions[i] = descriptions[i + 1];
                statuses[i] = statuses[i + 1];
                dates[i] = dates[i + 1];
            }
            
            descriptions[taskCount - 1] = null;
            statuses[taskCount - 1] = false;
            dates[taskCount - 1] = DateTime.MinValue;
            
            taskCount--;
            Console.WriteLine($"Задача \"{deletedTask}\" удалена");
            Console.WriteLine($"Осталось задач: {taskCount}");

            return taskCount;
        }

        static int UpdateTask(string input, ref string[] descriptions, ref bool[] statuses, ref DateTime[] dates, int taskCount)
        {

            string[] parts = input.Split(new char[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);
            
            if (parts.Length < 3 || !parts[2].StartsWith("\"") || !parts[2].EndsWith("\""))
            {
                Console.WriteLine("Ошибка: Неверный формат команды. Используйте: update <индекс> \"новый текст\"");
                return taskCount;
            }

            if (!int.TryParse(parts[1], out int taskIndex) || taskIndex < 0 || taskIndex >= taskCount)
            {
                Console.WriteLine($"Ошибка: Неверный индекс задачи. Допустимые значения: 0-{taskCount - 1}");
                return taskCount;
            }

            string newText = parts[2].Substring(1, parts[2].Length - 2).Trim();
            
            if (string.IsNullOrWhiteSpace(newText))
            {
                Console.WriteLine("Ошибка: Текст задачи не может быть пустым.");
                return taskCount;
            }

            string oldText = descriptions[taskIndex];
            descriptions[taskIndex] = newText;
            dates[taskIndex] = DateTime.Now; 
            
            Console.WriteLine($"Задача обновлена: \"{oldText}\" -> \"{newText}\"");
            return taskCount;
        }

        static void ExpandArrays(ref string[] descriptions, ref bool[] statuses, ref DateTime[] dates)
        {
            int newSize = descriptions.Length * ARRAY_EXPANSION_MULTIPLIER;
            
            string[] newDescriptions = new string[newSize];
            bool[] newStatuses = new bool[newSize];
            DateTime[] newDates = new DateTime[newSize];
            
            for (int i = 0; i < descriptions.Length; i++)
            {
                newDescriptions[i] = descriptions[i];
                newStatuses[i] = statuses[i];
                newDates[i] = dates[i];
            }
            
            descriptions = newDescriptions;
            statuses = newStatuses;
            dates = newDates;
        }

        static string ExtractTaskText(string input)
        {
            string[] parts = input.Split('"');
            
            if (parts.Length < 2 || string.IsNullOrWhiteSpace(parts[1]))
            {
                Console.WriteLine("Ошибка: Неверный формат команды. Используйте: add \"текст задачи\"");
                return null;
            }

            return parts[1].Trim();
        }

        static int ExtractTaskIndex(string input, string commandName, int taskCount)
        {
            string[] parts = input.Split(' ');
            
            if (parts.Length < 2 || !int.TryParse(parts[1], out int taskIndex))
            {
                Console.WriteLine($"Ошибка: Неверный формат команды. Используйте: {commandName} <индекс>");
                return -1;
            }

            if (taskIndex < 0 || taskIndex >= taskCount)
            {
                Console.WriteLine($"Ошибка: Неверный индекс задачи. Допустимые значения: 0-{taskCount - 1}");
                return -1;
            }

            return taskIndex;
        }
    }
}