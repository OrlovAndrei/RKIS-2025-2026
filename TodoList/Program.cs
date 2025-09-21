namespace Jobcompleteby
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
            string yo = Console.ReadLine();

            int yeartoday = 2025;
            int yo2 = int.Parse(yo);
            int age = yeartoday - yo2;

            Console.WriteLine("Добавлен пользователь " + name + " " + surname + " Возраст - " + age);

        }

    }
}
