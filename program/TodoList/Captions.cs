//This file contains overly engineered logic for the welcome message - PoneMaurice
using System.Text;

namespace Task
{
    public class Captions
    {
        StringBuilder TextCaptions = new();
        public void WriteCaption()
        {
            /*спрашивает и выводит текст субтитров созданный 
            методом CompText*/
            if (TextCaptions.ToString() == "") CompText();
            string Char = Commands.InputString("Вывести титры?(y/N): ");
            Char = Char.ToLower();
            if (Char.Equals("y", StringComparison.CurrentCultureIgnoreCase))
                Console.WriteLine(TextCaptions.ToString());
        }
        void CompText()
        {
            /*Составляет текст для субтитров*/
            /*WHAT THE HAY IS THAT?! I think i actually like that;) - PoneMaurice */
            TextCaptions.Append("За работу ответственны:\n");
            TextCaptions.Append("\tШевченко Э. - README, исходный код, некоторые аспекты git;\n");
            TextCaptions.Append("\tТитов М. - github, .gitignore, некоторый части исходного кода;\n");
        }
    }
}