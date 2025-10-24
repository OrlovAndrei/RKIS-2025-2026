using System;
using System.Text.RegularExpressions;

namespace Todolist
{
    class Program
    {
        private static TaskManager taskManager = new TaskManager();

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
            string name = ReadNotEmptyInput("имя");
            
            Console.Write("Введите вашу фамилию: ");
            string surname = ReadNotEmptyInput("фамилию");
            
            Console.WriteLine($"Добавлен пользователь {name} {surname}");
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
            Console.WriteLine("╔════════════════════════════════════════════════╗");
            Console.WriteLine("║               КОМАНДЫ УПРАВЛЕНИЯ               ║");
            Console.WriteLine("╠════════════════════════════════════════════════╣");
            Console.WriteLine("║ help          - показать справку               ║");
            Console.WriteLine("║ add <текст>   - добавить задачу                ║");
            Console.WriteLine("║ view          - все задачи                     ║");
            Console.WriteLine("║ view -c       - выполненные задачи             ║");
            Console.WriteLine("║ view -p       - невыполненные задачи           ║");
            Console.WriteLine("║ read <номер>  - детали задачи                  ║");
            Console.WriteLine("║ done <номер>  - отметить выполненной           ║");
            Console.WriteLine("║ delete <номер>- удалить задачу                 ║");
            Console.WriteLine("║ exit          - выход                          ║");
            Console.WriteLine("╚════════════════════════════════════════════════╝");
        }

        private static void AddTask(string input)
        {
            string taskText = GetTaskText(input);
            if (!string.IsNullOrWhiteSpace(taskText))
            {
                taskManager.AddTask(taskText);
            }
        }

        private static void ViewTasks(string input)
        {
            if (input.Contains("-c")) taskManager.ShowCompletedTasks();
            else if (input.Contains("-p")) taskManager.ShowPendingTasks();
            else taskManager.ShowAllTasks();
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

    class TaskManager
    {
        private string[] tasks = new string[10];
        private bool[] statuses = new bool[10];
        private int count = 0;

        public void AddTask(string text)
        {
            if (count >= tasks.Length)
            {
                Console.WriteLine("Достигнут лимит задач!");
                return;
            }

            tasks[count] = text;
            statuses[count] = false;
            count++;
            
            Console.WriteLine("Задача добавлена!");
        }

        public void ShowAllTasks()
        {
            if (count == 0)
            {
                Console.WriteLine("Список задач пуст");
                return;
            }

            Console.WriteLine("Список задач:");
            for (int i = 0; i < count; i++)
            {
                string status = statuses[i] ? "✓" : " ";
                Console.WriteLine($"{i + 1}. [{status}] {tasks[i]}");
            }
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
            
            Console.WriteLine("┌────────────────────────────────┐");
            Console.WriteLine("│          ДЕТАЛИ ЗАДАЧИ         │");
            Console.WriteLine("├────────────────────────────────┤");
            Console.WriteLine($"│ Номер: {index + 1,-24} │");
            Console.WriteLine($"│ Статус: {status,-20} │");
            Console.WriteLine("├────────────────────────────────┤");
            Console.WriteLine($"│ {tasks[index],-30} │");
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
            Console.WriteLine($"Задача '{tasks[index]}' отмечена как выполненная");
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
            }

            count--;
            Console.WriteLine($"Задача '{task}' удалена!");
        }

        private void ShowTasksByStatus(bool completed)
        {
            bool found = false;
            
            for (int i = 0; i < count; i++)
            {
                if (statuses[i] == completed)
                {
                    string symbol = completed ? "✓" : "□";
                    Console.WriteLine($"{i + 1}. {symbol} {tasks[i]}");
                    found = true;
                }
            }

            if (!found)
            {
                string text = completed ? "выполненных" : "невыполненных";
                Console.WriteLine($"Нет {text} задач");
            }
        }

        private bool IsValidIndex(int index)
        {
            return index >= 0 && index < count;
        }
    }
}