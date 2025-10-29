namespace TodoList
{
    public class Program
    {
        public static void Main(string[] args)
        {
            System.Console.Write("Имя: ");
            string firstName = System.Console.ReadLine();
            if (string.IsNullOrWhiteSpace(firstName))
            {
                System.Console.WriteLine("Имя пустое.");
                return;
            }

            System.Console.Write("Фамилия: ");
            string lastName = System.Console.ReadLine();
            if (string.IsNullOrWhiteSpace(lastName))
            {
                System.Console.WriteLine("Фамилия пустая.");
                return;
            }

            System.Console.Write("Год рождения: ");
            if (!int.TryParse(System.Console.ReadLine(), out int birthYear))
            {
                System.Console.WriteLine("Неверный год.");
                return;
            }

            Profile profile = new Profile(firstName, lastName, birthYear);
            TodoList todoList = new TodoList();

            while (true)
            {
                System.Console.Write("> ");
                string input = System.Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(input))
                {
                    System.Console.WriteLine("Пусто.");
                    continue;
                }

                try
                {
                    ICommand command = CommandParser.Parse(input, todoList, profile);
                    command.Execute();
                }
                catch (System.ArgumentException ex)
                {
                    System.Console.WriteLine($"Ошибка: {ex.Message}");
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine($"Неожиданная ошибка: {ex.Message}");
                }
            }
        }
    }
}