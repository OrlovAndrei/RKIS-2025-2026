namespace TodoList
{
    public class ProfileCommand : ICommand
    {
        public Profile Profile { get; set; }

        public void Execute()
        {
            if (Profile != null)
            {
                Console.WriteLine(Profile.GetInfo());
            }
            else
            {
                Console.WriteLine("Данные пользователя не найдены");
            }
        }
    }
}