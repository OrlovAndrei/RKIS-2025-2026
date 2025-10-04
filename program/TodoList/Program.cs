// This is the main file, it contains cruical components of the program - PoneMaurice

namespace Task
{
    public static class TaskExtensions
    {
        public static void Main()
        {
            int cycle = 0;
            do
            {
                var sur = new Survey();
                if (cycle == 0)
                {
                    var cap = new Captions();
                    cap.WriteCaption();
                    Commands.PrintProfile();
                }
                sur.ProceStr("-- ");
                ++cycle;
            }
            while (true);
        }
    }
}
