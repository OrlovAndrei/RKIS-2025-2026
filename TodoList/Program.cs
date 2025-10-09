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
            bool[] statuses = new bool[2];
            DateTime[] dates = new DateTime[2];
            int count = 0;

            while (true)
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                if (line == null || line == "exit") break;
                if (line.StartsWith("add "))
                {
                    AddTask(ref todos, ref statuses, ref dates, ref count, line);
                    continue;
                }

                if (line.StartsWith("done "))
                {
                    MarkTaskDone(ref statuses, ref dates, count, line);
                    continue;
                }
                switch (line)
                {
                    case "help":
                        ShowHelp();
                        continue;
                    case "profile":
                        ShowProfile(name, surname, birthYear);
                        continue;
                    case "view":
                        ViewTasks(todos, statuses, dates, count);
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

        static void ShowProfile(string name, string surname, int birthYear)
        {
            Console.WriteLine(name + " " + surname + ", " + birthYear);
        }

        static void AddTask(ref string[] todos, ref bool[] statuses, ref DateTime[] dates, ref int count, string line)
        {
            string[] parts = line.Split(' ', 2);
            if (parts.Length > 1)
            {
                if (count >= todos.Length)
                {
                    ExpandArrays(ref todos, ref statuses, ref dates);
                }

                todos[count] = parts[1].Trim();
                statuses[count] = false;
                dates[count] = DateTime.Now;

                count++;
                Console.WriteLine("Задача добавлена!");
            }
            else
            {
                Console.WriteLine("Ошибка: не введён текст задачи");
            }
        }
        
        static void ExpandArrays(ref string[] todos, ref bool[] statuses, ref DateTime[] dates)
        {
            string[] newTodos = new string[todos.Length * 2];
            bool[] newStatuses = new bool[statuses.Length * 2];
            DateTime[] newDates = new DateTime[dates.Length * 2];

            for (int i = 0; i < todos.Length; i++)
            {
                newTodos[i] = todos[i];
                newStatuses[i] = statuses[i];
                newDates[i] = dates[i];
            }

            todos = newTodos;
            statuses = newStatuses;
            dates = newDates;
        }

        static void ViewTasks(string[] todos, bool[] statuses, DateTime[] dates, int count)
        {
            if (count == 0)
            {
                Console.WriteLine("Список задач пуст");
            }
            else
            {
                Console.WriteLine("Ваши задачи: ");
                for (int i = 0; i < count; i++)
                {
                    string statusText = statuses[i] ? "сделано" : "не сделано";
                    Console.WriteLine((i + 1) + ". " + todos[i] + " - " + statusText + " - " + dates[i]);
                }
            }
        }

        static void MarkTaskDone(ref bool[] statuses, ref DateTime[] dates, int count, string line)
        {
            string[] parts = line.Split(' ', 2);
            if (parts.Length > 1 && int.TryParse(parts[1], out int idx))
            {
                idx--;
                if (idx >= 0 && idx < count)
                {
                    statuses[idx] = true;
                    dates[idx] = DateTime.Now;
                    Console.WriteLine("Задача выполнена");
                }
                else
                {
                    Console.WriteLine("Ошибка: некорректный номер задачи");
                }
            }
            else
            {
                Console.WriteLine("Ошибка: укажите номер задачи");
            }
        }
    }
}
