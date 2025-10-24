using System;
using System.Collections.Generic;

namespace Todolist
{
    class Program
    {
        private static TaskManager taskManager = new TaskManager();
        private static User user = new User();

        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Шегрикян и Агулов");
            
            InitializeUser();
            
            Console.WriteLine("Добро пожаловать в систему управления задачами!");
            Console.WriteLine("Введите 'help' для списка команд");
            
            RunMainLoop();
        }

        private static void InitializeUser()
        {
            Console.Write("Введите ваше имя: ");
            user.Name = ReadNotEmptyInput("имя");
            
            Console.Write("Введите вашу фамилию: ");
            user.Surname = ReadNotEmptyInput("фамилию");

            Console.Write("Введите ваш возраст: ");
            user.Age = ReadValidAge();
            
            Console.WriteLine($"Добавлен пользователь {user.Name} {user.Surname}");
        }

        private static string ReadNotEmptyInput(string fieldName)
        {
            while (true)
            {
                string input = Console.ReadLine() ?? "";
                if (!string.IsNullOrWhiteSpace(input))
                    return input.Trim();
                
                Console.Write($"{fieldName} не может быть пустым. Введите еще раз: ");
            }
        }

        private static int ReadValidAge()
        {
            while (true)
            {
                string input = Console.ReadLine() ?? "";
                if (int.TryParse(input, out int age) && age > 0 && age < 150)
                    return age;
                
                Console.Write("Возраст должен быть числом от 1 до 150. Введите еще раз: ");
            }
        }

        private static void RunMainLoop()
        {
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine() ?? "";
                
                if (string.IsNullOrWhiteSpace(input))
                    continue;
                
                ExecuteCommand(input.Trim());
            }
        }

        private static void ExecuteCommand(string input)
        {
            string command = input.Split(' ')[0].ToLower();
            
            switch (command)
            {
                case "help":    ShowHelp(); break;
                case "profile": ShowProfile(); break;
                case "add":     AddTask(input); break;
                case "view":    ViewTasks(input); break;
                case "read":    ReadTask(input); break;
                case "done":    MarkTaskDone(input); break;
                case "delete":  DeleteTask(input); break;
                case "exit":    ExitProgram(); break;
                default:        ShowUnknownCommand(command); break;
            }
        }

        private static void ShowHelp()
        {
            Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                       КОМАНДЫ УПРАВЛЕНИЯ                     ║");
            Console.WriteLine("╠══════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ help                    - показать справку                   ║");
            Console.WriteLine("║ profile                 - данные пользователя                ║");
            Console.WriteLine("║ add <текст>            - добавить задачу                     ║");
            Console.WriteLine("║ add --multiline         - многострочный ввод задачи          ║");
            Console.WriteLine("║ view                    - текст задач (по умолчанию)         ║");
            Console.WriteLine("║ view --index            - с индексами                        ║");
            Console.WriteLine("║ view --status           - со статусами                       ║");
            Console.WriteLine("║ view --update-date      - с датами изменений                 ║");
            Console.WriteLine("║ view -a                 - все данные                         ║");
            Console.WriteLine("║ read <номер>            - детали задачи                      ║");
            Console.WriteLine("║ done <номер>            - отметить выполненной               ║");
            Console.WriteLine("║ delete <номер>          - удалить задачу                     ║");
            Console.WriteLine("║ exit                    - выход                              ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
        }

        private static void ShowProfile()
        {
            Console.WriteLine("╔════════════════════════════════════════════════╗");
            Console.WriteLine("║                ПРОФИЛЬ ПОЛЬЗОВАТЕЛЯ            ║");
            Console.WriteLine("╠════════════════════════════════════════════════╣");
            Console.WriteLine($"║ Имя:    {user.Name,-35}    ║");
            Console.WriteLine($"║ Фамилия: {user.Surname,-34}    ║");
            Console.WriteLine($"║ Возраст: {user.Age,-34}    ║");
            Console.WriteLine("╠════════════════════════════════════════════════╣");
            Console.WriteLine($"║ Всего задач: {taskManager.TaskCount,-28}      ║");
            Console.WriteLine($"║ Выполнено: {taskManager.CompletedCount,-29}       ║");
            Console.WriteLine($"║ Осталось: {taskManager.PendingCount,-30}       ║");
            Console.WriteLine("╚════════════════════════════════════════════════╝");
        }

        private static void AddTask(string input)
        {
            if (input.Contains("--multiline") || input.Contains("-m"))
            {
                AddMultilineTask();
            }
            else
            {
                string taskText = GetTaskText(input);
                if (!string.IsNullOrWhiteSpace(taskText))
                {
                    taskManager.AddTask(taskText);
                }
            }
        }

        private static void AddMultilineTask()
        {
            Console.WriteLine("Многострочный ввод задачи. Вводите строки, для завершения введите !end");
            Console.WriteLine("Введите текст задачи:");

            List<string> lines = new List<string>();
            while (true)
            {
                Console.Write("> ");
                string line = Console.ReadLine() ?? "";
                
                if (line.Trim() == "!end")
                    break;
                
                if (!string.IsNullOrWhiteSpace(line))
                    lines.Add(line);
            }

            if (lines.Count == 0)
            {
                Console.WriteLine("Задача не добавлена: пустой текст");
                return;
            }

            string multilineText = string.Join("\n", lines);
            taskManager.AddTask(multilineText);
            Console.WriteLine("Многострочная задача добавлена!");
        }

        private static void ViewTasks(string input)
        {
            bool showIndex = input.Contains("--index") || input.Contains("-i");
            bool showStatus = input.Contains("--status") || input.Contains("-s");
            bool showDate = input.Contains("--update-date") || input.Contains("-d");
            bool showAll = input.Contains("-a");

            if (showAll)
            {
                showIndex = true;
                showStatus = true;
                showDate = true;
            }

            // Если нет флагов - показываем только текст
            if (!showIndex && !showStatus && !showDate)
            {
                taskManager.ShowTasksTextOnly();
            }
            else
            {
                taskManager.ShowTasksWithFlags(showIndex, showStatus, showDate);
            }
        }

        private static void ReadTask(string input)
        {
            int taskNumber = ParseTaskNumber(input);
            if (taskNumber > 0)
            {
                taskManager.ShowTaskDetails(taskNumber - 1);
            }
        }

        private static void MarkTaskDone(string input)
        {
            int taskNumber = ParseTaskNumber(input);
            if (taskNumber > 0)
            {
                taskManager.MarkAsDone(taskNumber - 1);
            }
        }

        private static void DeleteTask(string input)
        {
            int taskNumber = ParseTaskNumber(input);
            if (taskNumber > 0)
            {
                taskManager.DeleteTask(taskNumber - 1);
            }
        }

        private static void ExitProgram()
        {
            Console.WriteLine("Выход из программы...");
            Environment.Exit(0);
        }

        private static void ShowUnknownCommand(string command)
        {
            Console.WriteLine($"Неизвестная команда: {command}");
            Console.WriteLine("Введите 'help' для списка команд");
        }

        private static string GetTaskText(string input)
        {
            if (!input.ToLower().StartsWith("add "))
            {
                Console.WriteLine("Ошибка: используйте 'add <текст задачи>'");
                return "";
            }

            string taskText = input.Substring(4).Trim();
            if (string.IsNullOrWhiteSpace(taskText))
            {
                Console.WriteLine("Ошибка: текст задачи не может быть пустым");
                return "";
            }

            return taskText;
        }

        private static int ParseTaskNumber(string input)
        {
            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            
            if (parts.Length < 2)
            {
                Console.WriteLine("Ошибка: укажите номер задачи");
                return 0;
            }

            if (int.TryParse(parts[1], out int number) && number > 0)
            {
                return number;
            }

            Console.WriteLine("Ошибка: номер задачи должен быть положительным числом");
            return 0;
        }
    }

    class User
    {
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";
        public int Age { get; set; }
    }

    class TaskManager
    {
        private string[] tasks = new string[10];
        private bool[] statuses = new bool[10];
        private DateTime[] updateDates = new DateTime[10];
        private int count = 0;

        public int TaskCount => count;
        public int CompletedCount => GetCountByStatus(true);
        public int PendingCount => GetCountByStatus(false);

        public TaskManager()
        {
            for (int i = 0; i < updateDates.Length; i++)
            {
                updateDates[i] = DateTime.Now;
            }
        }

        public void AddTask(string text)
        {
            if (count >= tasks.Length)
            {
                Console.WriteLine("Достигнут лимит задач!");
                return;
            }

            tasks[count] = text;
            statuses[count] = false;
            updateDates[count] = DateTime.Now;
            count++;
            
            Console.WriteLine("Задача добавлена!");
        }

        public void ShowTasksTextOnly()
        {
            if (count == 0)
            {
                Console.WriteLine("Список задач пуст");
                return;
            }

            Console.WriteLine("Текст задач:");
            for (int i = 0; i < count; i++)
            {
                string taskText = GetShortText(tasks[i], 30);
                Console.WriteLine($"{taskText}");
            }
        }

        public void ShowTasksWithFlags(bool showIndex, bool showStatus, bool showDate)
        {
            if (count == 0)
            {
                Console.WriteLine("Список задач пуст");
                return;
            }

            // Определяем ширину колонок
            int indexWidth = showIndex ? 6 : 0;
            int statusWidth = showStatus ? 10 : 0;
            int dateWidth = showDate ? 19 : 0;
            int textWidth = 32;

            // Строим верхнюю границу таблицы
            string topBorder = "┌";
            if (showIndex) topBorder += new string('─', indexWidth) + "┬";
            topBorder += new string('─', textWidth) + "┬";
            if (showStatus) topBorder += new string('─', statusWidth) + "┬";
            if (showDate) topBorder += new string('─', dateWidth) + "┬";
            topBorder = topBorder.TrimEnd('┬') + "┐";
            Console.WriteLine(topBorder);

            // Заголовок таблицы
            string header = "│";
            if (showIndex) header += " №".PadRight(indexWidth - 1) + " │";
            header += " Текст задачи".PadRight(textWidth - 1) + " │";
            if (showStatus) header += " Статус".PadRight(statusWidth - 1) + " │";
            if (showDate) header += " Дата изменения".PadRight(dateWidth - 1) + " │";
            Console.WriteLine(header);

            // Разделитель
            string separator = "├";
            if (showIndex) separator += new string('─', indexWidth) + "┼";
            separator += new string('─', textWidth) + "┼";
            if (showStatus) separator += new string('─', statusWidth) + "┼";
            if (showDate) separator += new string('─', dateWidth) + "┼";
            separator = separator.TrimEnd('┼') + "┤";
            Console.WriteLine(separator);

            // Данные задач
            for (int i = 0; i < count; i++)
            {
                string row = "│";
                
                if (showIndex)
                    row += $" {i + 1}".PadRight(indexWidth - 1) + " │";
                
                string taskText = GetShortText(tasks[i], 30);
                row += $" {taskText}".PadRight(textWidth - 1) + " │";
                
                if (showStatus)
                {
                    string status = statuses[i] ? "Выполнена" : "Не выполнена";
                    row += $" {status}".PadRight(statusWidth - 1) + " │";
                }
                
                if (showDate)
                {
                    string date = updateDates[i].ToString("dd.MM.yyyy HH:mm");
                    row += $" {date}".PadRight(dateWidth - 1) + " │";
                }
                
                Console.WriteLine(row);
            }

            // Нижняя граница
            string bottomBorder = "└";
            if (showIndex) bottomBorder += new string('─', indexWidth) + "┴";
            bottomBorder += new string('─', textWidth) + "┴";
            if (showStatus) bottomBorder += new string('─', statusWidth) + "┴";
            if (showDate) bottomBorder += new string('─', dateWidth) + "┴";
            bottomBorder = bottomBorder.TrimEnd('┴') + "┘";
            Console.WriteLine(bottomBorder);
        }

        public void ShowCompletedTasks()
        {
            Console.WriteLine("Выполненные задачи:");
            ShowTasksByStatus(true);
        }

        public void ShowPendingTasks()
        {
            Console.WriteLine("Невыполненные задачи:");
            ShowTasksByStatus(false);
        }

        public void ShowTaskDetails(int index)
        {
            if (!IsValidIndex(index))
            {
                Console.WriteLine("Ошибка: неверный номер задачи");
                return;
            }

            string status = statuses[index] ? "Выполнена ✓" : "Не выполнена □";
            string date = updateDates[index].ToString("dd.MM.yyyy HH:mm");
            
            Console.WriteLine("┌────────────────────────────────┐");
            Console.WriteLine("│          ДЕТАЛИ ЗАДАЧИ        │");
            Console.WriteLine("├────────────────────────────────┤");
            Console.WriteLine($"│ Номер: {index + 1,-24} │");
            Console.WriteLine($"│ Статус: {status,-20} │");
            Console.WriteLine($"│ Изменена: {date,-18} │");
            Console.WriteLine("├────────────────────────────────┤");
            Console.WriteLine($"│ {GetShortText(tasks[index], 28),-30} │");
            Console.WriteLine("└────────────────────────────────┘");
        }

        public void MarkAsDone(int index)
        {
            if (!IsValidIndex(index))
            {
                Console.WriteLine("Ошибка: неверный номер задачи");
                return;
            }

            statuses[index] = true;
            updateDates[index] = DateTime.Now;
            Console.WriteLine($"Задача '{GetShortText(tasks[index], 50)}' отмечена как выполненная");
        }

        public void DeleteTask(int index)
        {
            if (!IsValidIndex(index))
            {
                Console.WriteLine("Ошибка: неверный номер задачи");
                return;
            }

            string task = tasks[index];
            
            for (int i = index; i < count - 1; i++)
            {
                tasks[i] = tasks[i + 1];
                statuses[i] = statuses[i + 1];
                updateDates[i] = updateDates[i + 1];
            }

            count--;
            Console.WriteLine($"Задача '{GetShortText(task, 50)}' удалена!");
        }

        private void ShowTasksByStatus(bool completed)
        {
            bool found = false;
            
            for (int i = 0; i < count; i++)
            {
                if (statuses[i] == completed)
                {
                    string symbol = completed ? "✓" : "□";
                    Console.WriteLine($"{i + 1}. {symbol} {GetShortText(tasks[i], 50)}");
                    found = true;
                }
            }

            if (!found)
            {
                string text = completed ? "выполненных" : "невыполненных";
                Console.WriteLine($"Нет {text} задач");
            }
        }

        private string GetShortText(string text, int maxLength)
        {
            if (text.Length <= maxLength)
                return text;
            
            return text.Substring(0, maxLength - 3) + "...";
        }

        private int GetCountByStatus(bool completed)
        {
            int result = 0;
            for (int i = 0; i < count; i++)
            {
                if (statuses[i] == completed)
                    result++;
            }
            return result;
        }

        private bool IsValidIndex(int index)
        {
            return index >= 0 && index < count;
        }
    }
}