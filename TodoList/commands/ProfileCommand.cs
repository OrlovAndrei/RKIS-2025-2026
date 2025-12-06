namespace TodoList.commands
{
    public class ProfileCommand : ICommand
    {
        public Profile Profile { get; set; }

        public void Execute()
        {
            Console.WriteLine(Profile.GetInfo());
        }
        
        public Profile SetProfile()
        {
            Console.Write("Введите ваше имя: ");
            string name = Console.ReadLine();
            Console.Write("Введите вашу фамилию: ");
            string surname = Console.ReadLine();
            Console.Write("Введите год рождения: ");
            int year = int.Parse(Console.ReadLine());

            return new Profile(name, surname, year);
        }
    }
}
