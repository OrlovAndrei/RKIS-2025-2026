namespace TodoList
{
    class Program
    {
        public static void Main()
        {
            Console.WriteLine("Работу выполнил: Измайлов");

            Console.Write("Введите ваше имя: ");
            string name = Console.ReadLine();
            Console.Write("Введите вашу фамилию: ");
            string surname = Console.ReadLine();

            Console.Write("Введите ваш год рождения: ");
            int year = int.Parse(Console.ReadLine());
            int age = DateTime.Now.Year - year;

            Console.WriteLine($"Добавлен пользователь {name} {surname}, возраст - {age}");
        }
    }
}