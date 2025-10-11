// This is the main file, it contains cruical components of the program - PoneMaurice

namespace Task
{
    public static class TaskExtensions
    {
        public static void Main()
        {
            int cycle = 0;
            Survey survey = new();
            Console.Clear();
            do
            {
                if (cycle == 0)
                {
                    //Commands.PrintProfile();
                }
                survey.GlobalCommand("-- ");
                ++cycle;
            }
            while (true);
        }
    }
}
