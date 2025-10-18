using System.Text;
namespace Task
{
    static class Input
    {
        public static string DataType(string text)
        {
            /*Выводит на экран текст и запрашивает у пользователя 
            ввести тип данных и вводит его в бесконечный цикл 
            вводимая пользователем строка проверяеться на наличие 
            такого типа и если он есть возвращает его сокращение*/
            while (true)
            {
                string input = String(text);
                string res = SearchDataTypeOnJson.ConvertingInputValues(input);
                if (res.Length != 0)
                {
                    return res;
                }
                WriteToConsole.RainbowText("Вы ввели неподдерживаемый тип данных", ConsoleColor.Red);
            }
        }
        public static string String(string text)
        {
            /*выводит текст пользователю и запрашивает 
            ввести строковые данные, они проверяются на
            наличие и если строка пуста то возвращаеться 
            "NULL" если нет то возвращается обработаная 
            версия строки*/
            StringBuilder input = new();
            while (true)
            {
                Console.Write(text);
                input.Append((Console.ReadLine() ?? "").Trim());
                if (input.ToString().Length != 0)
                {
                    return input.ToString();
                }
                WriteToConsole.RainbowText("Строка не должна быть пустой", ConsoleColor.Red);
            }
        }
        public static int IntegerWithMinMax(string text, int min, int max)
        {
            /*Запрашивает у пользователя дату, проверяется
            на минимальное и максимальное допустимое значение,
            а так же возвращает простые цифры с нулем.
            Пример: 02, 00, 09 и тд.s*/
            int result;
            string input;
            while (true)
            {
                input = String(text);
                if (int.TryParse(input, out result) &&
                result >= min && result <= max)
                {
                    return result;
                }
                WriteToConsole.RainbowText($"'{input}' должно являться целым числом,", ConsoleColor.Red);
                WriteToConsole.RainbowText($"быть меньше или равно (<=) {min},", ConsoleColor.Red);
                WriteToConsole.RainbowText($"быть больше или равно (>=) {max}.", ConsoleColor.Red);
            }
        }
        public static int Integer(string text)
        {
            int result;
            while (true)
            {
                string input = String(text);
                if (int.TryParse(input, out result))
                {
                    return result;
                }
                WriteToConsole.RainbowText($"'{input}' должно являться целым числом.", ConsoleColor.Red);
            }
        }
        public static int PositiveInteger(string text)
        {
            int result;
            while (true)
            {
                string input = String(text);
                if (int.TryParse(input, out result) && result >= 0)
                {
                    return result;
                }
                WriteToConsole.RainbowText($"'{input}' должно являться целым числом,", ConsoleColor.Red);
                WriteToConsole.RainbowText($"быть больше или равняться (>=) 0.", ConsoleColor.Red);
            }
        }
        public static float Float(string text)
        {
            float result;
            while (true)
            {
                string input = String(text);
                if (float.TryParse(input, out result))
                {
                    return result;
                }
                WriteToConsole.RainbowText($"'{input}' должно являться десятичным числом.", ConsoleColor.Red);
            }
        }
        public static float PositiveFloat(string text)
        {
            float result;
            while (true)
            {
                string input = String(text);
                if (float.TryParse(input, out result) && result >= 0)
                {
                    return result;
                }
                WriteToConsole.RainbowText($"'{input}' должно являться десятичным числом,", ConsoleColor.Red);
                WriteToConsole.RainbowText($"быть больше или равняться (>=) 0.", ConsoleColor.Red);
            }
        }
        public static string ManualDate()
        {
            string exampleDate = DateTime.Now.ToShortDateString();
            string dateString;
            DateOnly dateOnly;
            while (true)
            {
                dateString = String($"Введите дату (Пример {exampleDate}): ");
                if (DateOnly.TryParse(dateString, out dateOnly))
                {
                    return dateOnly.ToShortDateString();
                }
                WriteToConsole.RainbowText($"'{dateString}' не может быть преобразовано,", ConsoleColor.Red);
                WriteToConsole.RainbowText($"пожалуйста повторите попытку опираясь на приведенный пример.", ConsoleColor.Red);
            }
        }
        public static string ManualTime()
        {
            string exampleDate = DateTime.Now.ToShortTimeString();
            string timeString;
            TimeOnly timeOnly;
            while (true)
            {
                timeString = String($"Введите время (Пример {exampleDate}): ");
                if (TimeOnly.TryParse(timeString, out timeOnly))
                {
                    return timeOnly.ToShortTimeString();
                }
                WriteToConsole.RainbowText($"'{timeString}' не может быть преобразовано,", ConsoleColor.Red);
                WriteToConsole.RainbowText($"пожалуйста повторите попытку опираясь на приведенный пример.", ConsoleColor.Red);
            }
        }
        public static string PointByPointDate()
        {
            int year = IntegerWithMinMax("Введите год: ", 1, 9999);
            int month = IntegerWithMinMax("Введите месяц: ", 1, 12);
            int day = IntegerWithMinMax("Введите день: ", 1,
                DateTime.DaysInMonth(year, month));
            DateOnly yearMonthDay = new(year, month, day);
            return yearMonthDay.ToShortDateString();
        }
        public static string PointByPointTime()
        {
            int hour = IntegerWithMinMax("Введите час: ", 0, 23);
            int minute = IntegerWithMinMax("Введите минуты: ", 0, 59);
            TimeOnly hourAndMinute = new(hour, minute);
            return hourAndMinute.ToShortTimeString();
        }
        public static string DateAndTime()
        {
            /*Запрашивает всю дату в двух вариантах опросом и 
            когда пользователя спрашивают по пунктам, 
            а так же если он не выберет какой-то из вариантов 
            ввода даты то программа автоматически введет "NULL"*/
            System.Console.WriteLine("---Ввод даты и времени---");
            string modeDate = String($"Выберете метод ввода даты: (Ручной('M'), Попунктный('P')): ").ToLower();
            string dateAndTime = "";
            if (modeDate == "m")
            {
                dateAndTime = ManualDate() + " " + ManualTime();
            }
            else if (modeDate == "p")
            {
                dateAndTime = PointByPointDate() + " " + PointByPointTime();
            }
            else WriteToConsole.RainbowText("Вы не выбрали режим, все даты по default будут 'Null'", ConsoleColor.Yellow);
            return dateAndTime;
        }
        public static string Date()
        {
            /*Запрашивает всю дату в двух вариантах опросом и 
            когда пользователя спрашивают по пунктам, 
            а так же если он не выберет какой-то из вариантов 
            ввода даты то программа автоматически введет "NULL"*/
            System.Console.WriteLine("---Ввод даты---");
            string modeDate = String($"Выберете метод ввода даты: (Ручной('M'), Попунктный('P')): ").ToLower();
            string dateAndTime = "";
            if (modeDate == "m")
            {
                dateAndTime = ManualDate();
            }
            else if (modeDate == "p")
            {
                dateAndTime = PointByPointDate();
            }
            else WriteToConsole.RainbowText("Вы не выбрали режим, все даты по default будут 'Null'", ConsoleColor.Yellow);
            return dateAndTime;
        }
        public static string Time()
        {
            /*Запрашивает всю дату в двух вариантах опросом и 
            когда пользователя спрашивают по пунктам, 
            а так же если он не выберет какой-то из вариантов 
            ввода даты то программа автоматически введет "NULL"*/
            System.Console.WriteLine("---Ввод времени---");
            string modeDate = String($"Выберете метод ввода даты: (Ручной('M'), Попунктный('P')): ");
            string dateAndTime = "";
            if (modeDate == "m")
            {
                dateAndTime = ManualTime();
            }
            else if (modeDate == "p")
            {
                dateAndTime = PointByPointTime();
            }
            else WriteToConsole.RainbowText("Вы не выбрали режим, все даты по default будут 'Null'", ConsoleColor.Yellow);
            return dateAndTime;
        }
        public static string NowDateTime()
        {
            /*возвращает сегодняшнюю дату и время в нужном формате*/
            DateTime nowDate = DateTime.Now;
            return nowDate.ToShortDateString() +
                " " + nowDate.ToShortTimeString();
        }
        public static void IfNull(string writeText, ref string text)
        {
            if (text == null || text.Length == 0)
            {
                text = String(writeText);
            }
        }
        public static string RowOnTitleAndConfig(string[] titleRowArray, string[] dataTypeRowArray, string nameData)
        {
            FormatterRows row = new(nameData);
            for (int i = 0; i < titleRowArray.Length; i++)
            {
                switch (dataTypeRowArray[i])
                {
                    case "s":
                        row.AddInRow(Input.String($"введите {titleRowArray[i]} (string): "));
                        break;
                    case "i":
                        row.AddInRow(Input.Integer($"введите {titleRowArray[i]} (int): ").ToString());
                        break;
                    case "pos_i":
                        row.AddInRow(Input.PositiveInteger($"введите {titleRowArray[i]} (pos. int): ").ToString());
                        break;
                    case "f":
                        row.AddInRow(Input.Float($"введите {titleRowArray[i]} (float): ").ToString());
                        break;
                    case "pos_f":
                        row.AddInRow(Input.PositiveFloat($"введите {titleRowArray[i]} (pos. float): ").ToString());
                        break;
                    case "d":
                        Console.WriteLine($"---ввод {titleRowArray[i]}---");
                        row.AddInRow(Input.Date());
                        break;
                    case "t":
                        Console.WriteLine($"---ввод {titleRowArray[i]}---");
                        row.AddInRow(Input.Time());
                        break;
                    case "dt":
                        Console.WriteLine($"---ввод {titleRowArray[i]}---");
                        row.AddInRow(Input.DateAndTime());
                        break;
                    case "ndt":
                        row.AddInRow(Input.NowDateTime());
                        break;
                    case "b":
                        row.AddInRow(Input.Bool($"введите {titleRowArray[i]} (bool): ").ToString());
                        break;
                }
            }
            return row.Row.ToString();
        }
        public static bool Bool(string text)
        {
            string input = String(text).ToLower();
            if (input == "t" || input == "true" ||
                input == "y" || input == "yes")
            {
                return true;
            }
            else return false;
        }
    }
    public class WriteToConsole
    {
        public static void RainbowText(string textError, ConsoleColor colorText)
        {
            Console.ForegroundColor = colorText;
            Console.WriteLine(textError);
            Console.ResetColor();
        }
        public static void Text(params string[] text)
        {
            foreach (string textItem in text)
            {
                System.Console.WriteLine(textItem);
            }
        }
    }
}