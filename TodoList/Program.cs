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
            string birthYear = Console.ReadLine();

            int currentYear = 2025;
            int birthYear2 = int.Parse(birthYear);
            int age = currentYear - birthYear2;

            Console.WriteLine("Добавлен пользователь " + name + " " + surname + " Возраст - " + age);

            string[] todos = { };

            while (true)
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                if (line == null || line == "exit") break;
                switch(line)
                {
                    case "help":
                        Console.WriteLine("profile - комманда выводит данные о пользователе");
                        Console.WriteLine("add - добавляет новую задачу. Формат ввода: add 'текст задачи'");
                        Console.WriteLine("view - выводит все задачи из массива");
                        Console.WriteLine("exit - завершает цикл");
                        continue;
                }
            }
        }

    }
}
