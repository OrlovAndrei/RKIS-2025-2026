using System;

namespace ConsApp
{
    class MainClass
    {
        public static void Main()
        {
            Console.Write("Введите ваше имя: "); 
            string UserFirstName = Console.ReadLine();
            Console.Write("Введите вашу фамилию: ");
            string UserLastName = Console.ReadLine();

            short year = 1;
            byte month = 1;
            byte day = 1;

            do
            {
                Console.Write("Введите ваш год рождения: ");
                year = short.Parse(Console.ReadLine());
            }
            while (year < 1900 || year > DateTime.Now.Year);

            do
            {
                Console.Write("Введите ваш месяц рождения: ");
                month = byte.Parse(Console.ReadLine());
            }
            while (month > 12 || month < 0);

            do
            {
                Console.Write("Введите ваш день рождения: ");
                day = byte.Parse(Console.ReadLine());
            }
            while (day > 31 || day < 0);

            DateTime BirthDate = new DateTime(year, month, day);
            string UserName = UserFirstName + " " + UserLastName;

            string text = $"\n{DateTime.Now}: добавлен пользователь {UserName}, день рождения {BirthDate.ToLongDateString()}\nвозраст - {DateTime.Now.Year-BirthDate.Year}";

            Console.WriteLine(text);
        }
    }
}
