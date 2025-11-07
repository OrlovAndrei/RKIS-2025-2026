using System;

namespace Todolist
{
    // Интерфейс команды
    public interface ICommand
    {
        void Execute();
    }

    // Базовый класс для команд, работающих с задачами
    public abstract class TodoCommand : ICommand
    {
        protected TodoList todoList;
        
        public TodoCommand(TodoList todoList)
        {
            this.todoList = todoList;
        }
        
        public abstract void Execute();
    }

    // Команда показа команд (а как ещё сказать то)
    public class HelpCommand : ICommand
    {
        public void Execute()
        {
            var t = """
                Доступные команды
                help - вывести список команд
                profile - показать данные пользователя
                add - добавить задачу (однострочный режим)
                   --multiline или -m - добавить задачу (многострочный режим)
                view - показать только текст задачи
                   --index или -i - показать с индексами
                   --status или -s - показать со статусами
                   --update-date или -d - показать дату последнего изменения
                   --all или -a - показать все данные
                read <номер> - просмотреть полный текст задачи
                done <номер> - отметить задачу как выполненную
                delete <номер> - удалить задачу
                update <номер> "новый текст" - обновить текст задачи
                exit - выход из программы
                """;
            Console.WriteLine(t);
        }
    }

    // Команда показа профиля
    public class ProfileCommand : ICommand
    {
        private Profile userProfile;
        
        public ProfileCommand(Profile profile)
        {
            this.userProfile = profile;
        }
        
        public void Execute()
        {
            if (string.IsNullOrEmpty(userProfile.FirstName) || string.IsNullOrEmpty(userProfile.LastName))
            {
                Console.WriteLine("Данные пользователя не заполнены");
                return;
            }
            Console.WriteLine(userProfile.GetInfo());
        }
    }

    // Команда добавления задачи
    public class AddCommand : TodoCommand
    {
        public string TaskText { get; set; }
        public bool MultilineMode { get; set; }
        
        public AddCommand(TodoList todoList, string taskText, bool multilineMode = false) 
            : base(todoList)
        {
            TaskText = taskText;
            MultilineMode = multilineMode;
        }
        
        public override void Execute()
        {
            if (MultilineMode)
            {
                ExecuteMultiline();
            }
            else
            {
                ExecuteSingleLine();
            }
        }
        
        private void ExecuteSingleLine()
        {
            if (string.IsNullOrWhiteSpace(TaskText))
            {
                Console.WriteLine("Ошибка: задача не может быть пустой");
                return;
            }
            
            TodoItem newItem = new TodoItem(TaskText);
            todoList.Add(newItem);
            Console.WriteLine("Задача добавлена");
        }
        
        private void ExecuteMultiline()
        {
            Console.WriteLine("Введите задачу построчно. Для завершения введите '!end':");
            string[] lines = new string[100];
            int lineCount = 0;

            while (true)
            {
                Console.Write("> ");
                string line = Console.ReadLine();
                if (string.IsNullOrEmpty(line)) continue;

                if (line == "!end") break;

                if (lineCount >= lines.Length)
                {
                    Console.WriteLine("Достигнут лимит строк (100). Завершите ввод");
                    break;
                }
                lines[lineCount] = line;
                lineCount++;
            }
            
            if (lineCount == 0)
            {
                Console.WriteLine("Задача не была добавлена - пустой ввод");
                return;
            }
            
            string task = "";
            for (int i = 0; i < lineCount; i++)
            {
                task += lines[i];
                if (i < lineCount - 1)
                {
                    task += "\n";
                }
            }
            
            TodoItem newItem = new TodoItem(task);
            todoList.Add(newItem);
            Console.WriteLine("Многострочная задача добавлена");
        }
    }

    // Команда просмотра задач
    public class ViewCommand : TodoCommand
    {
        public bool ShowIndex { get; set; }
        public bool ShowStatus { get; set; }
        public bool ShowDate { get; set; }
        public bool ShowAll { get; set; }
        
        public ViewCommand(TodoList todoList, bool showIndex = false, bool showStatus = false, 
                          bool showDate = false, bool showAll = false) 
            : base(todoList)
        {
            ShowIndex = showIndex;
            ShowStatus = showStatus;
            ShowDate = showDate;
            ShowAll = showAll;
        }
        
        public override void Execute()
        {
            if (ShowAll)
            {
                ShowIndex = true;
                ShowStatus = true;
                ShowDate = true;
            }
            
            todoList.View(ShowIndex, ShowStatus, ShowDate);
        }
    }

    // Команда чтения полного текста задачи
    public class ReadCommand : TodoCommand
    {
        public int TaskNumber { get; set; }
        
        public ReadCommand(TodoList todoList, int taskNumber) : base(todoList)
        {
            TaskNumber = taskNumber;
        }
        
        public override void Execute()
        {
            if (TaskNumber > 0 && TaskNumber <= todoList.Count)
            {
                int index = TaskNumber - 1;
                TodoItem item = todoList.GetItem(index);
                Console.WriteLine("=======================================");
                Console.WriteLine(item.GetFullInfo());
                Console.WriteLine("=======================================");
            }
            else
            {
                Console.WriteLine("Неверный номер задачи");
            }
        }
    }

    // Команда отметки задачи как выполненной
    public class DoneCommand : TodoCommand
    {
        public int TaskNumber { get; set; }
        
        public DoneCommand(TodoList todoList, int taskNumber) : base(todoList)
        {
            TaskNumber = taskNumber;
        }
        
        public override void Execute()
        {
            if (TaskNumber > 0 && TaskNumber <= todoList.Count)
            {
                int index = TaskNumber - 1;
                TodoItem item = todoList.GetItem(index);
                item.MarkDone();
                Console.WriteLine($"Задача '{item.Text}' выполнена");
            }
            else
            {
                Console.WriteLine("Неверный номер задачи");
            }
        }
    }

    // Команда удаления задачи
    public class DeleteCommand : TodoCommand
    {
        public int TaskNumber { get; set; }
        
        public DeleteCommand(TodoList todoList, int taskNumber) : base(todoList)
        {
            TaskNumber = taskNumber;
        }
        
        public override void Execute()
        {
            if (TaskNumber > 0 && TaskNumber <= todoList.Count)
            {
                int index = TaskNumber - 1;
                string deletedTask = todoList.GetItem(index).Text;
                todoList.Delete(index);
                Console.WriteLine($"Задача '{deletedTask}' удалена");
            }
            else
            {
                Console.WriteLine("Неверный номер задачи");
            }
        }
    }

    // Команда обновления задачи
    public class UpdateCommand : TodoCommand
    {
        public int TaskNumber { get; set; }
        public string NewText { get; set; }
        
        public UpdateCommand(TodoList todoList, int taskNumber, string newText) 
            : base(todoList)
        {
            TaskNumber = taskNumber;
            NewText = newText;
        }
        
        public override void Execute()
        {
            if (string.IsNullOrWhiteSpace(NewText))
            {
                Console.WriteLine("Ошибка: новый текст не может быть пустым");
                return;
            }
            
            if (TaskNumber > 0 && TaskNumber <= todoList.Count)
            {
                int index = TaskNumber - 1;
                TodoItem item = todoList.GetItem(index);
                string oldTask = item.Text;
                item.UpdateText(NewText);
                Console.WriteLine($"Задача '{oldTask}' обновлена на '{NewText}'");
            }
            else
            {
                Console.WriteLine("Неверный номер задачи");
            }
        }
    }

    // Команда выхода из программы
    public class ExitCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("Выход из программы");
            Environment.Exit(0);
        }
    }

    class Program
    {
        static TodoList todoList = new TodoList();
        static Profile userProfile = new Profile();

        static void Main()
        {
            bool isValid = true;
            int currentYear = DateTime.Now.Year;

            Console.Write("Работу сделали Приходько и Бочкарёв\n");
            Console.Write("Введите свое имя: ");
            userProfile.FirstName = Console.ReadLine();
            Console.Write("Введите свою фамилию: ");
            userProfile.LastName = Console.ReadLine();
            Console.Write("Введите свой год рождения: ");

            try
            {
                userProfile.BirthYear = int.Parse(Console.ReadLine());
            }
            catch (Exception)
            {
                isValid = false;
            }

            if ((isValid == true) && (userProfile.BirthYear <= currentYear))
            {
                Console.WriteLine($"Добавлен пользователь:{userProfile.GetInfo()}");
            }
            else
            {
                Console.WriteLine("Неверно введен год рождения");
            }

            Console.WriteLine("Добро пожаловать в программу");
            Console.WriteLine("Введите 'help' для списка команд");
            
            while (true)
            {
                Console.WriteLine("=-=-=-=-=-=-=-=");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                ICommand command = ParseCommand(input);
                if (command != null)
                {
                    command.Execute();
                }
                else
                {
                    Console.WriteLine($"Неизвестная команда: {input.Split(' ')[0]}");
                }
            }
        }

        static ICommand ParseCommand(string input)
        {
            string[] parts = input.Split(' ');
            string commandName = parts[0].ToLower();

            switch (commandName)
            {
                case "help":
                    return new HelpCommand();
                    
                case "profile":
                    return new ProfileCommand(userProfile);
                    
                case "add":
                    return ParseAddCommand(parts);
                    
                case "view":
                    return ParseViewCommand(parts);
                    
                case "read":
                    if (parts.Length < 2) 
                    {
                        Console.WriteLine("Ошибка: не указан номер задачи");
                        return null;
                    }
                    if (int.TryParse(parts[1], out int readNumber))
                    {
                        return new ReadCommand(todoList, readNumber);
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: неверный номер задачи");
                        return null;
                    }
                    
                case "done":
                    if (parts.Length < 2) 
                    {
                        Console.WriteLine("Ошибка: не указан номер задачи");
                        return null;
                    }
                    if (int.TryParse(parts[1], out int doneNumber))
                    {
                        return new DoneCommand(todoList, doneNumber);
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: неверный номер задачи");
                        return null;
                    }
                    
                case "delete":
                    if (parts.Length < 2) 
                    {
                        Console.WriteLine("Ошибка: не указан номер задачи");
                        return null;
                    }
                    if (int.TryParse(parts[1], out int deleteNumber))
                    {
                        return new DeleteCommand(todoList, deleteNumber);
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: неверный номер задачи");
                        return null;
                    }
                    
                case "update":
                    return ParseUpdateCommand(parts);
                    
                case "exit":
                    return new ExitCommand();
                    
                default:
                    return null;
            }
        }

        static AddCommand ParseAddCommand(string[] parts)
        {
            bool multilineMode = false;
            for (int i = 1; i < parts.Length; i++)
            {
                if (!string.IsNullOrEmpty(parts[i]) &&
                    (parts[i] == "--multiline" || parts[i] == "-m"))
                {
                    multilineMode = true;
                    break;
                }
            }
            
            if (multilineMode)
            {
                return new AddCommand(todoList, "", true);
            }
            else
            {
                if (parts.Length < 2)
                {
                    Console.WriteLine("Ошибка: не указана задача");
                    return null;
                }
                string task = string.Join(" ", parts, 1, parts.Length - 1);
                return new AddCommand(todoList, task, false);
            }
        }

        static ViewCommand ParseViewCommand(string[] parts)
        {
            bool showIndex = false;
            bool showStatus = false;
            bool showDate = false;
            bool showAll = false;
            
            for (int i = 1; i < parts.Length; i++)
            {
                string flag = parts[i];
                if (flag == "--all" || flag == "-a") showAll = true;
                else if (flag == "--index" || flag == "-i") showIndex = true;
                else if (flag == "--status" || flag == "-s") showStatus = true;
                else if (flag == "--update-date" || flag == "-d") showDate = true;
                else if (flag.StartsWith("-") && flag.Length > 1 && !flag.StartsWith("--"))
                {
                    foreach (char c in flag.Substring(1))
                    {
                        switch (c)
                        {
                            case 'i': showIndex = true; break;
                            case 's': showStatus = true; break;
                            case 'd': showDate = true; break;
                            case 'a': showAll = true; break;
                        }
                    }
                }
            }
            
            return new ViewCommand(todoList, showIndex, showStatus, showDate, showAll);
        }

        static UpdateCommand ParseUpdateCommand(string[] parts)
        {
            if (parts.Length < 3)
            {
                Console.WriteLine("Ошибка: не указан номер задачи или новый текст");
                return null;
            }
            
            if (int.TryParse(parts[1], out int updateNumber))
            {
                string newText = string.Join(" ", parts, 2, parts.Length - 2);
                return new UpdateCommand(todoList, updateNumber, newText);
            }
            else
            {
                Console.WriteLine("Ошибка: неверный номер задачи");
                return null;
            }
        }
    }
}