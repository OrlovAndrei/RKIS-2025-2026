using System;
using System.Windows.Forms;
using Microsoft.VisualBasic;

class HelloWorld
{
    static void Main()
    {
        Console.WriteLine("выполнил работу Турищев Иван");
        Console.ReadLine();
    }
}
class EnteringInteger {
    public void lol()
    {
        string res, txt;
        int year = DateTime.Now.Year, age = -1, born = -1;
        res = Interaction.InputBox("в каком году вы поделись ?", "год рождения");
        if (int.TryParse(res, out born))
        //born = Int32.Parse(res);
        age = year - born;
        txt = $"ну ты старый, {age} лет";
        MessageBox.Show(txt, "возраст");
    }
}

