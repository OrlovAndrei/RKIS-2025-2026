using System;

namespace TodoList
{
    class Program
    {
        static Profile userProfile;
        static TodoList todoList = new TodoList();

        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Фучаджи и Клюев");

            CreateUser();

            Console.WriteLine("Введите help для вывода доступных команд");


            while (true)
            {

                Console.Write("Введите команду:");
                string CommandUser = Console.ReadLine();

                if (CommandUser == null || CommandUser.ToLower() == "exit") break;

                if (CommandUser == "help")
                {
                    CommandHelp();
                }
                else if (CommandUser == "profile")
                {
                    CommandProfile();
                }
                else if (CommandUser.StartsWith("add"))
                {
                    CommandAdd(CommandUser);
                }
                else if (CommandUser.StartsWith("done "))
                {
                    CommandDone(CommandUser);
                }
                else if (CommandUser.StartsWith("delete "))
                {
                    CommandDelete(CommandUser);
                }
                else if (CommandUser.StartsWith("update "))
                {
                    CommandUpdate(CommandUser);
                }
                else if (CommandUser.StartsWith("read "))
                {
                    CommandRead(CommandUser);
                }
                else if (CommandUser.StartsWith("view"))
                {
                    CommandView(CommandUser);
                }

            }
        }

        static void CreateUser()
        {
            Console.Write("Введите ваше имя: ");
            string firstName = Console.ReadLine();
            if (string.IsNullOrEmpty(firstName))
            {
                Console.WriteLine("Имя не может быть пустым");
                return;
            }

            Console.Write("Введите вашу Фамилию: ");
            string lastName = Console.ReadLine();

            Console.Write("Введите вашу дату рождения: ");
            string birthYearInput = Console.ReadLine();
            if (!int.TryParse(birthYearInput, out int birthYear) || birthYear <= 0)
            {
                Console.WriteLine("Введите реальный возраст");
                return;
            }
            userProfile = new Profile(firstName, lastName, birthYear);
            Console.WriteLine($"Добавлен пользователь {userProfile.GetInfo()}");
        }

        static void CommandHelp()
        {
            Console.WriteLine(@"Доступные команды:
        help - справка по командам
        profile - данные пользователя
        add - добавить задачу (--multiline/-m, --urgent/-u)
        view - список задач (--index/-i, --status/-s, --update-date/-d, --all/-a)
        read - полный текст задачи
        done - отметить выполненной
        delete - удалить задачу
        update - изменить текст задачи
        exit - выход из программы");
        }

        static void CommandProfile()
        {
            if (userProfile != null)
            {
                Console.WriteLine($"Пользователь: {userProfile.GetInfo()}");
            }
            else
            {
                Console.WriteLine("Профиль не создан");
            }
        }
        static void CommandAdd(string CommandUser)
        {
            if (string.IsNullOrEmpty(CommandUser))
            {
                Console.WriteLine("Ошибка: пустая команда");
                return;
            }

            bool isMultiline = CommandUser.Contains("--multiline") || CommandUser.Contains("-m");
            bool isUrgent = CommandUser.Contains("--urgent") || CommandUser.Contains("-u");

            if (CommandUser.Contains("-") && CommandUser.Length >= 3)
            {
                int flagIndex = CommandUser.IndexOf('-');
                if (flagIndex + 2 < CommandUser.Length)
                {
                    string flags = CommandUser.Substring(flagIndex + 1, 2);
                    if (flags.Contains("m") && flags.Contains("u"))
                    {
                        isMultiline = true;
                        isUrgent = true;
                    }
                }
            }

            string task = "";
            if (isMultiline)
            {
                Console.WriteLine("Введите задачу (для завершения введите пустую строку):");
                string line;
                while (!string.IsNullOrWhiteSpace(line = Console.ReadLine()))
                {
                    task += line + Environment.NewLine;
                }
                task = task.TrimEnd();
            }
            else
            {
                string[] parts = CommandUser.Split('"');
                if (parts.Length >= 2)
                {
                    task = parts[1];
                }
                else
                {
                    Console.WriteLine("Ошибка: используйте формат add \"текст задачи\" или add --multiline");
                    return;
                }
            }

            if (isUrgent)
            {
                task = "[Срочно] " + task;
            }
            TodoItem newItem = new TodoItem(task);
            todoList.Add(newItem);
            Console.WriteLine($"Задача добавлена: {task}");
        }

        static void CommandView(string CommandUser = "")
        {
            bool showIndex = CommandUser.Contains("--index") || CommandUser.Contains("-i");
            bool showStatus = CommandUser.Contains("--status") || CommandUser.Contains("-s");
            bool showUpdateDate = CommandUser.Contains("--update-date") || CommandUser.Contains("-d");
            bool showAll = CommandUser.Contains("--all") || CommandUser.Contains("-a");

            if (CommandUser.Contains("-") && CommandUser.Length >= 2)
            {
                int flagIndex = CommandUser.IndexOf('-');
                if (flagIndex + 1 < CommandUser.Length)
                {
                    string flagsPart = CommandUser.Substring(flagIndex + 1);
                    string[] words = flagsPart.Split(' ');
                    if (words.Length > 0 && words[0].Length > 0)
                    {
                        string flags = words[0];
                        if (flags.Length > 0 && flags[0] != '-')
                        {
                            foreach (char flag in flags)
                            {
                                if (flag == 'i') showIndex = true;
                                else if (flag == 's') showStatus = true;
                                else if (flag == 'd') showUpdateDate = true;
                                else if (flag == 'a') showAll = true;
                            }
                        }
                    }
                }
            }

            if (showAll)
            {
                showIndex = true;
                showStatus = true;
                showUpdateDate = true;
            }

            todoList.View(showIndex, showStatus, showUpdateDate);

        }

        static void CommandDone(string CommandUser)
        {
            string[] parts = CommandUser.Split(' ');
            if (parts.Length >= 2 && int.TryParse(parts[1], out int index))
            {
                index--;
                if (index >= 0 && index < todoList.Count)
                {
                    try
                    {
                        todoList.GetItem(index).MarkDone();
                        Console.WriteLine($"Задача {index + 1} отмечена как выполненная");

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка: неверный индекс задачи");
                }
            }
            else
            {
                Console.WriteLine("Ошибка: используйте формат done <номер_задачи>");
            }
        }


        static void CommandDelete(string CommandUser)
        {
            string[] parts = CommandUser.Split(' ');
            if (parts.Length >= 2 && int.TryParse(parts[1], out int index))
            {
                index--;
                if (index >= 0 && index < todoList.Count)
                {
                    try
                    {
                        todoList.Delete(index);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка: неверный индекс задачи");
                }
            }
            else
            {
                Console.WriteLine("Ошибка: используйте формат delete <номер_задачи>");
            }
        }

        static void CommandUpdate(string CommandUser)
        {
            string[] parts = CommandUser.Split(' ', 3);
            if (parts.Length >= 3)
            {
                if (int.TryParse(parts[1], out int index))
                {
                    index--;
                    if (index >= 0 && index < todoList.Count)
                    {
                        try
                        {
                            string newText = parts[2].Trim('"');
                            todoList.GetItem(index).UpdateText(newText);
                            Console.WriteLine($"Задача {index + 1} обновлена: {newText}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка: {ex.Message}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка: используйте формат update *номер_задачи* \"новый текст\"");
                }
            }
            else
            {
                Console.WriteLine("Ошибка: используйте формат update *номер_задачи* \"новый текст\"");
            }
        }

        static void CommandRead(string CommandUser)
        {
            string[] parts = CommandUser.Split(' ');
            if (parts.Length >= 2 && int.TryParse(parts[1], out int index))
            {
                index--;
                if (index >= 0 && index < todoList.Count)
                {
                    try
                    {
                        todoList.Read(index);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка: неверный индекс задачи");
                }
            }
            else
            {
                Console.WriteLine("Ошибка: используйте формат read <номер_задачи>");
            }
        }
    }
}