namespace TodoList
{
    class Program
    {
        public static void Main()
        {
            Console.WriteLine("Работу выполнили Зусикова и Кабачек 3833.9");
            Console.WriteLine("Введите ваше имя:"); 
            string name = Console.ReadLine();
            Console.WriteLine("Введите вашу фамилию:");
            string surname = Console.ReadLine();

            Console.WriteLine("Введите ваш год рождения:");
            int year = int.Parse(Console.ReadLine());
            int age = DateTime.Now.Year - year;
            
            Console.WriteLine("Добавлен пользователь " + name + " " + surname + ", возраст - " + age);
        }
    }
}