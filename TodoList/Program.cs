namespace TodoList
{

    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Работу выполнили Шелепов и Кузьменко");

            Console.WriteLine("Введите имя");
            string name = Console.ReadLine();

            Console.WriteLine("Введите фамилию");
            string surname = Console.ReadLine();

            Console.WriteLine("Введите год рождения");
            string birthYearInput = Console.ReadLine();

            int currentYear = DateTime.Now.Year;
            int birthYear = int.Parse(birthYearInput);
            int age = currentYear - birthYear;

            Console.WriteLine("Добавлен пользователь " + name + " " + surname + " Возраст - " + age);

            string[] todos = new string[2];
            int count = 0;

            while (true)
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                if (line == null || line == "exit") break;
                if (line.StartsWith("add "))
                {
                    string[] parts = line.Split(' ', 2);
                    if (parts.Length > 1)
                    {
                        if (count >= todos.Length)
                        {
                            string[] newTodos = new string[todos.Length * 2];
                            for (int i = 0; i < todos.Length; i++)
                            {
                                newTodos[i] = todos[i];
                            }
                            todos = newTodos;
                        }
                        todos[count] = parts[1].Trim();
                        count++;
                        Console.WriteLine("Задача добавлена!");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: не введён текст задачи");
                    }
                }
                switch (line)
                {
                    case "help":
                        ShowHelp()
                        continue;
                    case "profile":
                        Console.WriteLine(name + " " + surname + ", " + birthYear);
                        continue;
                    case "view":
                        if (count == 0)
                        {
                            Console.WriteLine("Список задач пуст");
                        }
                        else
                        {
                            Console.WriteLine("Ваши задачи: ");
                            for (int i = 0; i < count; i++)
                            {
                                Console.WriteLine((i + 1) + ". " + todos[i]);
                            }
                        }
                        continue;
                }
            }
        }

        static void ShowHelp()
        {
            Console.WriteLine("profile - комманда выводит данные о пользователе");
            Console.WriteLine("add - добавляет новую задачу. Формат ввода: add 'текст задачи'");
            Console.WriteLine("view - выводит все задачи из массива");
            Console.WriteLine("exit - завершает цикл");
        }

    }
}
