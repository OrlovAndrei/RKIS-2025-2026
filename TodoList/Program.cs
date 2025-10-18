using System.Text.RegularExpressions;

namespace Todolist
{
    class Program
    {
        private static Person user;
        private static string[] todos = new string[2];
        private static bool[] statuses = new bool[2];
        private static DateTime[] dates = new DateTime[2];
        private static int taskCount = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Морозов и Прокопенко");
            SetUserData();

            while (true)
            {
                Console.Write("Введите команду: ");
                string fullInput = Console.ReadLine().Trim();
                
                if (string.IsNullOrEmpty(fullInput))
                    continue;

                string[] partInput = fullInput.Split(' ', 2);
                string command = partInput[0].ToLower();
                
                switch (command)
                {
                    case "help":
                        ShowHelp();
                        break;

                    case "profile":
                        ShowProfile();
                        break;

                    case "add_user":
                        SetUserData();
                        break;

                    case "add":
                        if (partInput.Length > 1)
                        {
                            AddTask(partInput[1]);
                        }
                        else
                        {
                            Console.WriteLine("Неправильный формат: add \"текст задачи\" или add текст задачи");
                        }
                        break;

                    case "read":
                        if (partInput.Length > 1)
                        {
                            ReadTask(partInput[1]);
                        }
                        else
                        {
                            Console.WriteLine("Неправильный формат: read номер_задачи");
                        }
                        break;

                    case "view":
                        string flags = partInput.Length > 1 ? partInput[1] : "";
                        ShowTasks(flags);
                        break;

                    case "done":
                        if (partInput.Length > 1)
                        {
                            MarkTaskDone(partInput[1]);
                        }
                        else
                        {
                            Console.WriteLine("Неправильный формат: done номер_задачи");
                        }
                        break;

                    case "delete":
                        if (partInput.Length > 1)
                        {
                            DeleteTask(partInput[1]);
                        }
                        else
                        {
                            Console.WriteLine("Неправильный формат: delete номер_задачи");
                        }
                        break;

                    case "update":
                        if (partInput.Length > 1)
                        {
                            UpdateTask(partInput[1]);
                        }
                        else
                        {
                            Console.WriteLine("Неправильный формат: update номер_задачи \"новый текст\"");
                        }
                        break;

                    case "exit":
                        return;

                    default:
                        Console.WriteLine($"Неизвестная команда: {command}");
                        Console.WriteLine("Введите 'help' для просмотра доступных команд");
                        break;
                }
            }
        }

        static void MarkTaskDone(string taskId)
        {
            if (int.TryParse(taskId, out int taskNumber) && IsValidTaskNumber(taskNumber))
            {
                int taskIndex = taskNumber - 1;
                statuses[taskIndex] = true;
                dates[taskIndex] = DateTime.Now;
                Console.WriteLine($"Задача №{taskNumber} отмечена как выполненная");
            }
            else
            {
                Console.WriteLine($"Неверный номер задачи. Должен быть числом от 1 до {taskCount}");
            }
        }

        static void DeleteTask(string taskId)
        {
            if (int.TryParse(taskId, out int taskNumber) && IsValidTaskNumber(taskNumber))
            {
                int taskIndex = taskNumber - 1;
                string deletedTask = todos[taskIndex];
                
                
                for (int i = taskIndex; i < taskCount - 1; i++)
                {
                    todos[i] = todos[i + 1];
                    statuses[i] = statuses[i + 1];
                    dates[i] = dates[i + 1];
                }
                
                
                todos[taskCount - 1] = null;
                statuses[taskCount - 1] = false;
                dates[taskCount - 1] = DateTime.MinValue;
                
                taskCount--;
                Console.WriteLine($"Задача №{taskNumber} '{deletedTask}' удалена");
            }
            else
            {
                Console.WriteLine($"Неверный номер задачи. Должен быть числом от 1 до {taskCount}");
            }
        }

        static void UpdateTask(string input)
        {
            string[] parts = input.Split(' ', 2);
            
            if (parts.Length < 2)
            {
                Console.WriteLine("Не указан новый текст задачи");
                return;
            }

            if (int.TryParse(parts[0], out int taskNumber) && IsValidTaskNumber(taskNumber))
            {
                int taskIndex = taskNumber - 1;
                string newText = parts[1];
                
                
                if (newText.StartsWith("\"") && newText.EndsWith("\""))
                {
                    newText = newText.Substring(1, newText.Length - 2);
                }
                
                string oldText = todos[taskIndex];
                todos[taskIndex] = newText;
                dates[taskIndex] = DateTime.Now;
                
                Console.WriteLine($"Задача обновлена: \nБыло: №{taskNumber} \"{oldText}\" \nСтало: №{taskNumber} \"{newText}\"");
            }
            else
            {
                Console.WriteLine($"Неверный номер задачи. Должен быть числом от 1 до {taskCount}");
            }
        }

        static void SetUserData()
        {
            Console.Write("Введите ваше имя: ");
            string firstName = Console.ReadLine();

            Console.Write("Введите вашу фамилию: ");
            string lastName = Console.ReadLine();

            Console.Write("Введите ваш год рождения: ");
            if (int.TryParse(Console.ReadLine(), out int yearBirth))
            {
                int age = DateTime.Now.Year - yearBirth;
                user = new Person(firstName, lastName, yearBirth, age);
                Console.WriteLine($"Добавлен пользователь: {firstName} {lastName}, Год рождения: {yearBirth}, возраст: {age}");
            }
            else
            {
                Console.WriteLine("Ошибка: год рождения должен быть числом");
                SetUserData(); 
            }
            Console.WriteLine();
        }

        static void ShowHelp()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("help - список всех доступных команд");
            Console.WriteLine("profile - данные пользователя");
            Console.WriteLine("add_user - изменить пользователя");
            Console.WriteLine("add - добавить новую задачу");
            Console.WriteLine("read - полный просмотр задачи");
            Console.WriteLine("view - список всех задач");
            Console.WriteLine("done - отметить задачу выполненной");
            Console.WriteLine("delete - удалить задачу по номеру");
            Console.WriteLine("update - изменить текст задачи");
            Console.WriteLine("exit - завершить программу");
            Console.WriteLine();
            Console.WriteLine("Флаги для команды 'view':");
            Console.WriteLine(" -i, --index - показывать индекс задачи");
            Console.WriteLine(" -s, --status - показывать статус задачи");
            Console.WriteLine(" -d, --update-date - показывать дату изменения");
            Console.WriteLine(" -a, --all - показывать все данные");
        }

        static void ShowProfile()
        {
            if (user != null)
            {
                Console.WriteLine($"Пользователь: {user.FirstName} {user.LastName}");
                Console.WriteLine($"Год рождения: {user.YearBirth}");
                Console.WriteLine($"Возраст: {user.Age}");
            }
            else
            {
                Console.WriteLine("Данные пользователя не найдены");
            }
        }

        static void AddTask(string taskText)
        {
            if (taskText == "--multiline" || taskText == "-m")
            {
                AddMultilineTask();
            }
            else
            {
                AddSingleTask(taskText);
            }
        }

        static void AddMultilineTask()
        {
            Console.WriteLine("Многострочный режим. Введите задачи (для завершения введите '!end'):");
            List<string> lines = new List<string>();
            
            string line;
            while ((line = Console.ReadLine()) != null && line.ToLower() != "!end")
            {
                lines.Add(line);
            }

            if (lines.Count == 0)
            {
                Console.WriteLine("Не было введено ни одной строки");
                return;
            }

            string multilineText = string.Join("\n", lines);
            AddTaskToArray(multilineText);
            Console.WriteLine($"Добавлена многострочная задача №{taskCount}:");
            Console.WriteLine(multilineText);
        }

        static void AddSingleTask(string taskText)
        {
            
            if (taskText.StartsWith("\"") && taskText.EndsWith("\""))
            {
                taskText = taskText.Substring(1, taskText.Length - 2);
            }

            AddTaskToArray(taskText);
            Console.WriteLine($"Добавлена задача №{taskCount}: {taskText}");
        }

        static void AddTaskToArray(string taskText)
        {
            if (taskCount >= todos.Length)
            {
                ExpandArrays();
            }

            todos[taskCount] = taskText;
            statuses[taskCount] = false;
            dates[taskCount] = DateTime.Now;
            taskCount++;
        }

        static void ExpandArrays()
        {
            int newSize = todos.Length * 2;

            string[] newTodos = new string[newSize];
            bool[] newStatuses = new bool[newSize];
            DateTime[] newDates = new DateTime[newSize];

            Array.Copy(todos, newTodos, taskCount);
            Array.Copy(statuses, newStatuses, taskCount);
            Array.Copy(dates, newDates, taskCount);

            todos = newTodos;
            statuses = newStatuses;
            dates = newDates;

            Console.WriteLine($"Массивы увеличены до {newSize} элементов");
        }

        static void ReadTask(string taskId)
        {
            if (int.TryParse(taskId, out int taskNumber) && IsValidTaskNumber(taskNumber))
            {
                int taskIndex = taskNumber - 1;
                Console.WriteLine($"Задача №{taskNumber}:");
                Console.WriteLine($"Текст: {todos[taskIndex]}");
                Console.WriteLine($"Статус: {(statuses[taskIndex] ? "Выполнена" : "Не выполнена")}");
                Console.WriteLine($"Дата изменения: {dates[taskIndex]:dd.MM.yyyy HH:mm}");
            }
            else
            {
                Console.WriteLine($"Неверный номер задачи. Должен быть числом от 1 до {taskCount}");
            }
        }

        static void ShowTasks(string flags)
        {
            if (taskCount == 0)
            {
                Console.WriteLine("Задачи отсутствуют");
                return;
            }

            bool showIndex = flags.Contains("-i") || flags.Contains("--index");
            bool showStatus = flags.Contains("-s") || flags.Contains("--status");
            bool showDate = flags.Contains("-d") || flags.Contains("--update-date");
            bool showAll = flags.Contains("-a") || flags.Contains("--all");

            if (showAll) 
            {
                showIndex = showStatus = showDate = true;
            }

            
            List<string> headers = new List<string>();
            if (showIndex) headers.Add("№");
            if (showStatus) headers.Add("Статус");
            headers.Add("Задача");
            if (showDate) headers.Add("Дата изменения");

            string header = string.Join(" | ", headers);
            Console.WriteLine(header);
            Console.WriteLine(new string('-', header.Length));

           
            for (int i = 0; i < taskCount; i++)
            {
                List<string> rowData = new List<string>();

                if (showIndex) rowData.Add((i + 1).ToString());
                if (showStatus) rowData.Add(statuses[i] ? "Сделано" : "Не сделано");
                
                
                string taskText = todos[i];
                if (taskText.Length > 50)
                    taskText = taskText.Substring(0, 47) + "...";
                rowData.Add(taskText);
                
                if (showDate) rowData.Add(dates[i].ToString("dd.MM.yyyy HH:mm"));

                Console.WriteLine(string.Join(" | ", rowData));
            }
        }

        static bool IsValidTaskNumber(int taskNumber)
        {
            return taskNumber > 0 && taskNumber <= taskCount;
        }
    }

    class Person
    {
        public string FirstName { get; }
        public string LastName { get; }
        public int YearBirth { get; }
        public int Age { get; }

        public Person(string firstName, string lastName, int yearBirth, int age)
        {
            FirstName = firstName;
            LastName = lastName;
            YearBirth = yearBirth;
            Age = age;
        }
    }
}