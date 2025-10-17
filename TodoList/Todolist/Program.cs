using System;
using System.Collections.Generic;
using System.Linq;

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
                else if (input.StartsWith("add"))
                {
                    AddTask(input);
                }
                else if (input.StartsWith("view"))
                {
                    ViewTasks(input);
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
                else if (input.StartsWith("read "))
                {
                    ReadTask(input);
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
            Console.WriteLine("add \"текст задачи\" - добавить задачу (однострочный режим)");
            Console.WriteLine("add --multiline (-m) - добавить задачу (многострочный режим)");
            Console.WriteLine("view [флаги] - показать задачи");
            Console.WriteLine("  Флаги: --index (-i), --status (-s), --update-date (-d), --all (-a)");
            Console.WriteLine("done <номер> - отметить задачу выполненной");
            Console.WriteLine("delete <номер> - удалить задачу");
            Console.WriteLine("update <номер> \"новый текст\" - изменить задачу");
            Console.WriteLine("read <номер> - посмотреть полный текст задачи");
            Console.WriteLine("exit - выйти из программы");
        }

        static void ShowProfile()
        {
            Console.WriteLine($"{firstName} {lastName}, {birthYear} год рождения");
        }

        static void AddTask(string input)
        {
            // Проверяем флаги
            if (input.Contains("--multiline") || input.Contains("-m"))
            {
                AddMultilineTask();
            }
            else
            {
                AddSingleLineTask(input);
            }
        }

        static void AddSingleLineTask(string input)
        {
            string[] parts = input.Split('"');
            if (parts.Length >= 2)
            {
                string task = parts[1];
                AddTaskToArray(task);
                Console.WriteLine($"Задача добавлена: {task}");
            }
            else
            {
                Console.WriteLine("Ошибка: используйте формат add \"текст задачи\" или add --multiline");
            }
        }

        static void AddMultilineTask()
        {
            Console.WriteLine("Многострочный режим. Вводите задачи (для завершения введите 'end'):");
            
            List<string> lines = new List<string>();
            while (true)
            {
                string line = Console.ReadLine();
                if (line?.ToLower() == "end")
                    break;
                    
                if (!string.IsNullOrEmpty(line))
                    lines.Add(line);
            }
            
            if (lines.Count > 0)
            {
                string task = string.Join("\n", lines);
                AddTaskToArray(task);
                Console.WriteLine($"Добавлена многострочная задача ({lines.Count} строк)");
            }
            else
            {
                Console.WriteLine("Не добавлено ни одной строки");
            }
        }

        static void AddTaskToArray(string task)
        {
            if (taskCount >= todos.Length)
            {
                ResizeArrays();
            }
            
            todos[taskCount] = task;
            done[taskCount] = false;
            dates[taskCount] = DateTime.Now;
            taskCount++;
        }

        static void ViewTasks(string input)
        {
            // Парсим флаги
            bool showIndex = input.Contains("--index") || input.Contains("-i");
            bool showStatus = input.Contains("--status") || input.Contains("-s");
            bool showDate = input.Contains("--update-date") || input.Contains("-d");
            bool showAll = input.Contains("--all") || input.Contains("-a");
            
            // Если указан --all, показываем всё
            if (showAll)
            {
                showIndex = true;
                showStatus = true;
                showDate = true;
            }
            
            // Если нет флагов, показываем только текст
            if (!showIndex && !showStatus && !showDate)
            {
                ShowSimpleView();
            }
            else
            {
                ShowTableView(showIndex, showStatus, showDate);
            }
        }

        static void ShowSimpleView()
        {
            Console.WriteLine("Список задач:");
            bool hasTasks = false;
            for (int i = 0; i < taskCount; i++)
            {
                if (!string.IsNullOrEmpty(todos[i]))
                {
                    string shortText = GetShortText(todos[i]);
                    Console.WriteLine($"{shortText}");
                    hasTasks = true;
                }
            }
            
            if (!hasTasks)
            {
                Console.WriteLine("Задач нет");
            }
        }

        static void ShowTableView(bool showIndex, bool showStatus, bool showDate)
        {
            if (taskCount == 0)
            {
                Console.WriteLine("Задач нет");
                return;
            }
            
            // Собираем данные для таблицы
            List<string[]> tableData = new List<string[]>();
            
            // Заголовки
            List<string> headers = new List<string>();
            if (showIndex) headers.Add("№");
            headers.Add("Задача");
            if (showStatus) headers.Add("Статус");
            if (showDate) headers.Add("Изменено");
            
            tableData.Add(headers.ToArray());
            
            // Данные
            for (int i = 0; i < taskCount; i++)
            {
                if (string.IsNullOrEmpty(todos[i])) continue;
                
                List<string> row = new List<string>();
                if (showIndex) row.Add((i + 1).ToString());
                row.Add(GetShortText(todos[i]));
                if (showStatus) row.Add(done[i] ? "✓ Выполнена" : "✗ Не выполнена");
                if (showDate) row.Add(dates[i].ToString("dd.MM.yy HH:mm"));
                
                tableData.Add(row.ToArray());
            }
            
            // Выводим таблицу
            PrintTable(tableData);
        }

        static void PrintTable(List<string[]> tableData)
        {
            if (tableData.Count == 0) return;
            
            int columns = tableData[0].Length;
            int[] columnWidths = new int[columns];
            
            // Вычисляем ширину колонок
            foreach (var row in tableData)
            {
                for (int i = 0; i < columns; i++)
                {
                    if (row[i] != null && row[i].Length > columnWidths[i])
                    {
                        columnWidths[i] = row[i].Length;
                    }
                }
            }
            
            // Выводим таблицу
            foreach (var row in tableData)
            {
                for (int i = 0; i < columns; i++)
                {
                    string cell = row[i] ?? "";
                    Console.Write(cell.PadRight(columnWidths[i] + 2));
                }
                Console.WriteLine();
                
                // Разделитель после заголовка
                if (row == tableData[0])
                {
                    for (int i = 0; i < columns; i++)
                    {
                        Console.Write(new string('-', columnWidths[i]) + "  ");
                    }
                    Console.WriteLine();
                }
            }
        }

        static string GetShortText(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            
            // Берем первую строку для краткого отображения
            string firstLine = text.Split('\n')[0];
            if (firstLine.Length <= 30) return firstLine;
            
            return firstLine.Substring(0, 27) + "...";
        }

        static void ReadTask(string input)
        {
            if (int.TryParse(input.Substring(5).Trim(), out int index))
            {
                int taskIndex = index - 1;
                if (taskIndex >= 0 && taskIndex < taskCount && !string.IsNullOrEmpty(todos[taskIndex]))
                {
                    Console.WriteLine($"=== Задача {index} ===");
                    Console.WriteLine($"Текст: {todos[taskIndex]}");
                    Console.WriteLine($"Статус: {(done[taskIndex] ? "✓ ВЫПОЛНЕНА" : "✗ НЕ ВЫПОЛНЕНА")}");
                    Console.WriteLine($"Изменено: {dates[taskIndex]:dd.MM.yyyy HH:mm}");
                }
                else
                {
                    Console.WriteLine("Ошибка: неверный номер задачи или задача не существует");
                }
            }
            else
            {
                Console.WriteLine("Ошибка: используйте формат read <номер>");
            }
        }

        static void MarkTaskDone(string input)
        {
            if (int.TryParse(input.Substring(5), out int index))
            {
                int taskIndex = index - 1;
                if (taskIndex >= 0 && taskIndex < taskCount && !string.IsNullOrEmpty(todos[taskIndex]))
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
                if (taskIndex >= 0 && taskIndex < taskCount && !string.IsNullOrEmpty(todos[taskIndex]))
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
                    if (taskIndex >= 0 && taskIndex < taskCount && !string.IsNullOrEmpty(todos[taskIndex]))
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
