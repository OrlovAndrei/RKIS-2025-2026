namespace TodoList
{
    class Program
    {
        public static void Main()
        {
            Console.WriteLine("Выполнили курсанты группы 3833 Кучеренко и Мурза");
            Console.Write("Введите ваше имя: "); 
            string firstName = Console.ReadLine();
            Console.Write("Введите вашу фамилию: ");
            string lastName = Console.ReadLine();

            Console.Write("Введите ваш год рождения: ");
            int birthday = int.Parse(Console.ReadLine());

            string text = "Добавлен пользователь " + firstName + " " + lastName + ", возраст - " + (DateTime.Now.Year - birthday);
            Console.WriteLine(text);
        }
    }
}
