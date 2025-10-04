// This is the main file, it contains cruical components of the program - PoneMaurice
using System;
using System.Data;
using System.IO.Enumeration;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
namespace Task
{
    public class Commands
    {
        const string NameTask = "tasks";
        public string nameTask { get { return NameTask; } }
        const string StringChar = "s";
        const string IntegerChar = "i";
        const string DoubleChar = "f";
        const string TimeChar = "t";
        const string DateChar = "d";
        const string DateAndTime = "dt";
        const string NowDateTime = "ndt";
        public string InputDataType(string text)
        {

            /*Выводит на экран текст и запрашивает у пользователя 
            ввести тип данных и вводит его в бесконечный цикл 
            вводимая пользователем строка проверяеться на наличие 
            такого типа и если он есть возвращает его сокращение*/

            Dictionary<string, string> DataTypePros = new Dictionary<string, string>()
            {
                {"string", StringChar}, {"str", StringChar}, {"s", StringChar},
                {"integer", IntegerChar}, {"int", IntegerChar}, {"i", IntegerChar},
                {"double", DoubleChar}, {"float", DoubleChar}, {"f", DoubleChar},
                {"date", DateChar}, {"d", DateChar},
                {"time", TimeChar}, {"t", TimeChar},
                {"date and time", DateAndTime}, {"dt", DateAndTime}, {"datetime", DateAndTime},
                {"now date time", NowDateTime}, {"ndt", NowDateTime}, {"nowdatetime", NowDateTime}
            };
            while (true)
            {
                Console.Write(text);
                string input = Console.ReadLine() ?? FileWriter.stringNull;
                input = input.Trim();
                string inputLow = input.ToLower();
                foreach (var dataType in DataTypePros)
                {
                    if (inputLow == dataType.Key)
                    {
                        return dataType.Value;
                    }
                }
                Console.WriteLine("Вы ввели неподдерживаемый тип данных");
            }
        }
        public static string InputString(string text)
        {
            /*выводит текст пользователю и запрашивает 
            ввести строковые данные, они проверяются на
            наличие и если строка пуста то возвращаеться 
            "NULL" если нет то возвращается обработаная 
            версия строки*/
            Console.Write(text);
            StringBuilder input = new();
            input.Append((Console.ReadLine() ?? FileWriter.stringNull).Trim());
            if (input.ToString() == "") input.Append(FileWriter.stringNull);
            return input.ToString();
        }
        public static int InputDate(string text, int min, int max)
        {
            /*Запрашивает у пользователя дату, проверяется
            на миниммальное и максимальное допустимое значение,
            а так же возвращает простые цифры с нулем.
            Пример: 02, 00, 09 и тд.s*/
            int result = -1; // сродникоду об ошибке
            string input;
            do
            {
                input = InputString(text);
                int.TryParse(input, out result);
            }
            while (result < min || result > max); //условия выхода
            return result;
        }
        public static int InputDate(string text)
        {
            /*Перегрузка метода InputDate только без лимитов*/
            int result = -1; // сродникоду об ошибке
            do
            {
                string input = InputString(text);
                int.TryParse(input, out result);
            }
            while (result <= 0);
            return result;
        }
        private static string GetModeDateTime()
        {
            /*Запрашивает всю дату в двух вариантах опросом и 
            когда пользователя спрашивают по пунктам, 
            а так же если он не выберет какой-то из вариантов 
            ввода даты то программа автоматически введет "NULL"*/
            string modeDate = InputString($"Выберете метод ввода даты: (Стандартный('S'), Попунктный('P')): ");
            modeDate = modeDate.ToLower();
            if (modeDate == "s")
            {
                string exampleDate = FormatRows.GetNowDateTime();
                string dateString = InputString($"Введите дату (Пример {exampleDate}): ");
                return dateString;
            }
            else if (modeDate == "p")
            {
                int year = InputDate("Введите год: ");
                int month = InputDate("Введите месяц: ", 1, 12);
                int day = InputDate("Введите день: ", 1,
                    DateTime.DaysInMonth(year, month));
                DateOnly yearMonthDay = new(year, month, day);
                string date = yearMonthDay.ToShortDateString();
                int hour = InputDate("Введите час: ", 0, 23);
                int minute = InputDate("Введите минуты: ", 0, 59);
                TimeOnly hourAndMinute = new(hour, minute);
                string time = hourAndMinute.ToShortTimeString();
                string dateString = $"{date} {time}";
                return dateString;
            }
            else Console.WriteLine("Вы не выбрали режим, все даты по default будут 'NULL'");
            return FileWriter.stringNull;
        }
        private static string GetModeDate()
        {
            /*Запрашивает всю дату в двух вариантах опросом и 
            когда пользователя спрашивают по пунктам, 
            а так же если он не выберет какой-то из вариантов 
            ввода даты то программа автоматически введет "NULL"*/
            string modeDate = InputString($"Выберете метод ввода даты: (Стандартный('S'), Попунктный('P')): ");
            modeDate = modeDate.ToLower();
            if (modeDate == "s")
            {
                string exampleDate = DateTime.Now.ToShortDateString();
                string dateString = InputString($"Введите дату (Пример {exampleDate}): ");
                return dateString;
            }
            else if (modeDate == "p")
            {
                int year = InputDate("Введите год: ");
                int month = InputDate("Введите месяц: ", 1, 12);
                int day = InputDate("Введите день: ", 1,
                    DateTime.DaysInMonth(year, month));
                DateOnly yearMonthDay = new(year, month, day);
                return yearMonthDay.ToShortDateString();
            }
            else Console.WriteLine("Вы не выбрали режим, все даты по default будут 'NULL'");
            return FileWriter.stringNull;
        }
        private static string GetModeTime()
        {
            /*Запрашивает всю дату в двух вариантах опросом и 
            когда пользователя спрашивают по пунктам, 
            а так же если он не выберет какой-то из вариантов 
            ввода даты то программа автоматически введет "NULL"*/
            string modeDate = InputString($"Выберете метод ввода даты: (Стандартный('S'), Попунктный('P')): ");
            modeDate = modeDate.ToLower();
            if (modeDate == "s")
            {
                string exampleDate = DateTime.Now.ToShortTimeString();
                string dateString = InputString($"Введите время (Пример {exampleDate}): ");
                return dateString;
            }
            else if (modeDate == "p")
            {
                int hour = InputDate("Введите час: ", 0, 23);
                int minute = InputDate("Введите минуты: ", 0, 59);
                TimeOnly hourAndMinute = new(hour, minute);
                return hourAndMinute.ToShortTimeString();
            }
            else
            {
                Console.WriteLine("Вы не выбрали режим, все даты по default будут 'NULL'");
            }
            return FileWriter.stringNull;
        }
        public static void AddTask()
        {
            /*программа запрашивает у пользователя все необходимые ей данные
            и записывает их в файл tasks.csv с нужным форматированием*/
            string fileName = NameTask;
            FormatRows titleRow = new(fileName, true), row = new(fileName, false);
            FileWriter file = new();

            row.AddInRow(InputString("Введите название задания: "));
            row.AddInRow(InputString("Введите описание задания: "));
            System.Console.WriteLine("--- Ввод крайнего срока выполнения ---");
            row.AddInRow(FormatRows.GetNowDateTime());
            row.AddInRow(GetModeDateTime());


            string[] titleRowArray = { "nameTask", "description", "nowDateAndTime", "deadLine" };
            foreach (string pathTitleRow in titleRowArray)
                titleRow.AddInRow(pathTitleRow);

            string fullPath = file.TitleRowWriter(fileName, titleRow.Row.ToString());
            file.WriteFile(fullPath, row.Row.ToString(), true);
        }
        public static void AddTaskAndPrint()
        {
            /*программа запрашивает у пользователя все необходимые ей данные
            и записывает их в файл tasks.csv с нужным форматированием 
            после чего выводит сообщение о добовлении данных дублируя их 
            пользователю для проверки*/
            string fileName = NameTask;
            FormatRows titleRow = new(fileName, true), row = new(fileName, false);
            FileWriter file = new();

            row.AddInRow(InputString("Введите название задания: "));
            row.AddInRow(InputString("Введите описание задания: "));
            System.Console.WriteLine("--- Ввод крайнего срока выполнения ---");
            row.AddInRow(FormatRows.GetNowDateTime());
            row.AddInRow(GetModeDateTime());

            string[] titleRowArray = { "nameTask", "description", "nowDateAndTime", "deadLine" };
            foreach (string pathTitleRow in titleRowArray)
            { titleRow.AddInRow(pathTitleRow); }

            try
            {
                string fullPath = file.TitleRowWriter(fileName, titleRow.Row.ToString());
                file.WriteFile(fullPath, row.Row.ToString(), true);
                string[] titleRowString = titleRow.Row.ToString().Split(file.seporRows);
                string[] rowString = row.Row.ToString().Split(file.seporRows);

                for (int i = 0; i < titleRowString.Length; ++i)
                { Console.WriteLine($"{titleRowString[i]}: {rowString[i]}"); }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Возникла ошибка при записи в файл\n", ex.Message);
            }
        }
        public static void AddConfUserData(string fileName)
        {
            if (fileName == FileWriter.stringNull)
            {
                fileName = InputString("Введите название для файла с данными: ");
            }
            fileName = $"{fileName}_conf";
            FileWriter file = new();
            string fullPathConfig = file.CreatePath(fileName);
            string askFile = "y";
            string searchLine1 = FileWriter.stringNull;
            string searchLine2 = FileWriter.stringNull;
            if (File.Exists(fullPathConfig))
            {
                searchLine1 = file.GetLineFilePositionRow(fullPathConfig, 0);
                searchLine2 = file.GetLineFilePositionRow(fullPathConfig, 1);
                Console.WriteLine($"{searchLine1}\n{searchLine2}");
                askFile = InputString($"Вы точно уверены, что хотите перезаписать конфигурацию?(y/N): ");
            }
            if (askFile == "y")
            {
                FormatRows titleRow = new(fileName, true), dataTypeRow = new(fileName, false, true);

                do
                {
                    string intermediateResultString =
                        InputString("Введите название пункта титульного оформления файла: ");
                    if (intermediateResultString == "exit" &&
                    titleRow.GetLeghtRow() != 0) break;
                    else if (intermediateResultString == "exit")
                        Console.WriteLine("В титульном оформлении должен быть хотя бы один пункт: ");
                    else titleRow.AddInRow(intermediateResultString);
                }
                while (true);

                string[] titleRowArray = titleRow.Row.ToString().Split(file.seporRows);

                Commands config = new();
                foreach (string title in titleRowArray)
                {
                    if (title == "numbering") continue;
                    else dataTypeRow.AddInRow(config.InputDataType($"Введите тип данных для строки {title}: "));
                }

                file.TitleRowWriter(fileName, titleRow.Row.ToString());
                Console.WriteLine($"{titleRow.Row}\n{dataTypeRow.Row}");

                string lastTitleRow = file.GetLineFilePositionRow(fullPathConfig, 0);
                string lastDataTypeRow = file.GetLineFilePositionRow(fullPathConfig, 1);
                string askTitle = "y";
                string askDataType = "y";
                if (lastTitleRow != titleRow.Row.ToString() && lastTitleRow != FileWriter.stringNull)
                    askTitle = InputString($"Титульный лист отличается \nНыняшний: {titleRow}\nПрошлый: {lastTitleRow}\nЗаменить?(y/N): ");
                else if (lastDataTypeRow != dataTypeRow.Row.ToString() && lastDataTypeRow != FileWriter.stringNull)
                    askDataType = InputString($"Конфигурация уже имеется\nНынешняя: {dataTypeRow}\nПрошлая: {lastDataTypeRow}\nЗаменить?(y/N): ");
                if (askTitle == "y" || askDataType == "y")
                {
                    file.WriteFile(fullPathConfig, titleRow.Row.ToString(), false);
                    file.WriteFile(fullPathConfig, dataTypeRow.Row.ToString(), true);
                }
            }
            else
            {
                System.Console.WriteLine("Будет использована конфигурация: ");
                System.Console.WriteLine($"{searchLine1}\n{searchLine2}");
            }
        }
        public static void AddUserData(string nameData)
        {
            FileWriter file = new();
            FormatRows row = new(nameData, false);

            string fullPathConfig = file.CreatePath($"{nameData}_conf");
            if (File.Exists(fullPathConfig))
            {
                string titleRow = file.GetLineFilePositionRow(fullPathConfig, 0);
                string[] titleRowArray = titleRow.Split(file.seporRows);
                string dataTypeRow = file.GetLineFilePositionRow(fullPathConfig, 1);
                string[] dataTypeRowArray = dataTypeRow.Split(file.seporRows);
                for (int i = 0; i < titleRowArray.Length; i++)
                {
                    switch (dataTypeRowArray[i])
                    {
                        case Commands.StringChar:
                            row.AddInRow(InputString($"введите {titleRowArray[i]}: "));
                            break;
                        case Commands.IntegerChar:
                            row.AddInRow(InputString($"введите {titleRowArray[i]}: "));
                            break;
                        case Commands.DoubleChar:
                            row.AddInRow(InputString($"введите {titleRowArray[i]}: "));
                            break;
                        case Commands.DateChar:
                            Console.WriteLine($"---ввод {titleRowArray[i]}---");
                            row.AddInRow(GetModeDate());
                            break;
                        case Commands.TimeChar:
                            Console.WriteLine($"---ввод {titleRowArray[i]}---");
                            row.AddInRow(GetModeTime());
                            break;
                        case Commands.DateAndTime:
                            Console.WriteLine($"---ввод {titleRowArray[i]}---");
                            row.AddInRow(GetModeDateTime());
                            break;
                        case Commands.NowDateTime:
                            row.AddInRow(FormatRows.GetNowDateTime());
                            break;
                    }
                }
                string fullPath = file.TitleRowWriter(nameData, titleRow);
                string testTitleRow = file.GetLineFilePositionRow(fullPath, 0);
                if (testTitleRow != titleRow)
                    file.WriteFile(fullPath, titleRow, false);
                file.WriteFile(fullPath, row.Row.ToString(), true);
            }
            else Console.WriteLine($"Сначала создайте конфигурацию или проверьте правильность написания названия => '{nameData}'");
        }
        public static void ClearAllTasks()
        {
            if (InputString("Вы уверены что хотите очистить весь файл task? (y/N): ") == "y")
            {
                string fileName = NameTask;
                FileWriter file = new();
                FormatRows titleRow = new(fileName, true);
                string fullPath = file.CreatePath(fileName);
                string[] titleRowArray = { "nameTask", "description", "nowDateAndTime", "deadLine" };
                foreach (string pathTitleRow in titleRowArray)
                    titleRow.AddInRow(pathTitleRow);
                file.WriteFile(fullPath, titleRow.Row.ToString(), false);
            }
            else System.Console.WriteLine("Будте внимателны");
        }
        public void ClearPartTask(string text) // недописан обязательно исправить
        {
            string fileName = NameTask;
            FileWriter file = new();
            string fullPath = file.CreatePath(fileName);
            string[] titleRowArray = file.GetLineFilePositionRow(fullPath, 0).Split(file.seporRows);
            Dictionary<int, bool> tableClear = new Dictionary<int, bool>();

            System.Console.WriteLine($"Выберете в каком столбце искать {text} (t/F): ");
            for (int i = 0; i < titleRowArray.Length; ++i)
            {
                if (InputString($"{titleRowArray[i]}: ") == "t")
                    tableClear.Add(i, true);
                else tableClear.Add(i, false);
            }
        }
        public void SearchPartData(string text, string fileName = FileWriter.stringNull)
        {
            if (fileName == FileWriter.stringNull)
                fileName = InputString("Ведите название файла: ");
            if (text == FileWriter.stringNull)
                text = InputString("Поиск: ");
            FileWriter file = new();
            string fullPath = file.CreatePath(fileName);

            if (File.Exists(fullPath))
            {
                string[] titleRowArray = file.GetLineFilePositionRow(fullPath, 0).Split(file.seporRows);
                Dictionary<int, bool> tableClear = new Dictionary<int, bool>();


                System.Console.WriteLine($"Выберете в каком столбце искать {text} (t/F): ");
                for (int i = 0; i < titleRowArray.Length; ++i)
                {
                    if (InputString($"{titleRowArray[i]}: ") == "t")
                        tableClear.Add(i, true);
                    else tableClear.Add(i, false);
                }
                foreach (int i in tableClear.Keys)
                {
                    if (tableClear[i])
                        Console.WriteLine(file.GetLineFileDataOnPositionInRow(fullPath, text, i));
                }
            }
            else System.Console.WriteLine(fileName + ": такого файла не существует.");
        }
        public static void PrintData(string fileName)
        {
            if (fileName == FileWriter.stringNull)
                fileName = InputString("Ведите название файла: ");
            FileWriter file = new();
            string fullPath = file.CreatePath(fileName);

            try
            {
                using (StreamReader reader = new StreamReader(fullPath, Encoding.UTF8))
                {
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex}\n");
            }
        }
    }
}
