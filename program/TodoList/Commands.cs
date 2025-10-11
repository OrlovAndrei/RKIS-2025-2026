// This is the main file, it contains cruical components of the program - PoneMaurice
using System;
using System.Data;
using System.IO.Enumeration;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Task
{
    public class Commands
    {
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

            string[,] DataTypePros =
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

                string input = InputString(text);
                int rows = DataTypePros.GetUpperBound(0) + 1;
                for (int i = 0; i < rows; i++)
                {
                    if (DataTypePros[i, 0] == input)
                    {
                        return DataTypePros[i, 1].ToString();
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
            input.Append((Console.ReadLine() ?? ConstProgram.StringNull).Trim());
            if (input.ToString() == "") input.Append(ConstProgram.StringNull);
            return input.ToString();
        }
        public static int InputIntegerWithMinMax(string text, int min, int max)
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
        public static int InputInteger(string text)
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
        private static string GetDateAndTime()
        {
            /*Запрашивает всю дату в двух вариантах опросом и 
            когда пользователя спрашивают по пунктам, 
            а так же если он не выберет какой-то из вариантов 
            ввода даты то программа автоматически введет "NULL"*/
            string dateAndTime = GetDate() + " " + GetTime();
            return dateAndTime;
        }
        private static string GetDate()
        {
            /*Запрашивает всю дату в двух вариантах опросом и 
            когда пользователя спрашивают по пунктам, 
            а так же если он не выберет какой-то из вариантов 
            ввода даты то программа автоматически введет "NULL"*/
            System.Console.WriteLine("---Ввод даты---");
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
                int year = InputInteger("Введите год: ");
                int month = InputIntegerWithMinMax("Введите месяц: ", 1, 12);
                int day = InputIntegerWithMinMax("Введите день: ", 1,
                    DateTime.DaysInMonth(year, month));
                DateOnly yearMonthDay = new(year, month, day);
                return yearMonthDay.ToShortDateString();
            }
            else Console.WriteLine("Вы не выбрали режим, все даты по default будут 'NULL'");
            return ConstProgram.StringNull;
        }
        private static string GetTime()
        {
            /*Запрашивает всю дату в двух вариантах опросом и 
            когда пользователя спрашивают по пунктам, 
            а так же если он не выберет какой-то из вариантов 
            ввода даты то программа автоматически введет "NULL"*/
            System.Console.WriteLine("---Ввод времени---");
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
                int hour = InputIntegerWithMinMax("Введите час: ", 0, 23);
                int minute = InputIntegerWithMinMax("Введите минуты: ", 0, 59);
                TimeOnly hourAndMinute = new(hour, minute);
                return hourAndMinute.ToShortTimeString();
            }
            else
            {
                Console.WriteLine("Вы не выбрали режим, все даты по default будут 'NULL'");
            }
            return ConstProgram.StringNull;
        }
        public static void AddTask()
        {
            /*программа запрашивает у пользователя все необходимые ей данные
            и записывает их в файл tasks.csv с нужным форматированием*/
            FileWriter.AddRowInFile(ConstProgram.TaskName, ConstProgram.TaskTitle, ConstProgram.TaskTypeData);
        }
        public static void AddTaskAndPrint()
        {
            /*программа запрашивает у пользователя все необходимые ей данные
            и записывает их в файл tasks.csv с нужным форматированием 
            после чего выводит сообщение о добовлении данных дублируя их 
            пользователю для проверки*/
            FileWriter file = new(ConstProgram.TaskName);
            FileWriter.AddRowInFile(ConstProgram.TaskName, ConstProgram.TaskTitle, ConstProgram.TaskTypeData);
            try
            {
                string[] titleRowString = file.GetLineFilePositionRow(0).Split(ConstProgram.SeparRows);
                string[] rowString = file.GetLineFilePositionRow(file.GetLengthFile() - 1).Split(ConstProgram.SeparRows);
                for (int i = 0; i < titleRowString.Length; ++i)
                { Console.WriteLine($"{titleRowString[i]}: {rowString[i]}"); }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Возникла ошибка при записи в файл\n", ex.Message);
            }
        }
        public static void AddConfUserData(string fileName = "")
        {
            if (fileName == "")
            {
                fileName = InputString("Введите название для файла с данными: ");
            }
            fileName = fileName + ConstProgram.PrefConfigFile;
            FileWriter file = new(fileName);

            string fullPathConfig = file.CreatePath(fileName);
            string? askFile = null;
            string searchLine1 = ConstProgram.StringNull;
            string searchLine2 = ConstProgram.StringNull;
            if (File.Exists(fullPathConfig))
            {
                searchLine1 = file.GetLineFilePositionRow(0);
                searchLine2 = file.GetLineFilePositionRow(1);
                Console.WriteLine($"{searchLine1}\n{searchLine2}");
                askFile = InputString($"Вы точно уверены, что хотите перезаписать конфигурацию?(y/N): ");
            }
            if (askFile == ConstProgram.Yes || askFile == null)
            {
                FormatterRows titleRow = new(fileName, FormatterRows.Type.title), dataTypeRow = new(fileName, FormatterRows.Type.dataType);

                while (true)
                {
                    string intermediateResultString =
                        InputString("Введите название пункта титульного оформления файла: ");
                    if (intermediateResultString == "exit" &&
                    titleRow.GetLengthRow() != 0) break;
                    else if (intermediateResultString == "exit")
                        Console.WriteLine("В титульном оформлении должен быть хотя бы один пункт: ");
                    else titleRow.AddInRow(intermediateResultString);
                }

                string[] titleRowArray = titleRow.Row.ToString().Split(ConstProgram.SeparRows);

                Commands config = new();
                foreach (string title in titleRowArray)
                {
                    if (title == ConstProgram.TitleFirstObject) continue;
                    else dataTypeRow.AddInRow(config.InputDataType($"Введите тип данных для строки {title}: "));
                }

                file.TitleRowWriter(titleRow.Row.ToString());
                Console.WriteLine($"{titleRow.Row}\n{dataTypeRow.Row}");

                string lastTitleRow = file.GetLineFilePositionRow(0);
                string lastDataTypeRow = file.GetLineFilePositionRow(1);
                string askTitle = ConstProgram.Yes;
                string askDataType = ConstProgram.Yes;
                if (lastTitleRow != titleRow.Row.ToString() && lastTitleRow != ConstProgram.StringNull)
                    askTitle = InputString($"Титульный лист отличается \nНыняшний: {titleRow.Row}\nПрошлый: {lastTitleRow}\nЗаменить?(y/N): ");
                else if (lastDataTypeRow != dataTypeRow.Row.ToString() && lastDataTypeRow != ConstProgram.StringNull)
                    askDataType = InputString($"Конфигурация уже имеется\nНынешняя: {dataTypeRow.Row}\nПрошлая: {lastDataTypeRow}\nЗаменить?(y/N): ");
                if (askTitle == ConstProgram.Yes || askDataType == ConstProgram.Yes)
                {
                    file.WriteFile(titleRow.Row.ToString(), false);
                    file.WriteFile(dataTypeRow.Row.ToString(), true);
                }
            }
            else
            {
                System.Console.WriteLine("Будет использована конфигурация: ");
                System.Console.WriteLine($"{searchLine1}\n{searchLine2}");
            }
        }
        public static void AddUserData(string nameData = "")
        {
            if (nameData == "")
            {
                nameData = InputString("Введите название для файла с данными: ");
            }
            FileWriter fileConf = new(nameData + ConstProgram.PrefConfigFile);
            FileWriter file = new(nameData);

            string fullPathConfig = fileConf.CreatePath(nameData + ConstProgram.PrefConfigFile);
            if (File.Exists(fullPathConfig))
            {
                string titleRow = fileConf.GetLineFilePositionRow(0);
                string dataTypeRow = fileConf.GetLineFilePositionRow(1);
                string[] titleRowArray = titleRow.Split(ConstProgram.SeparRows);
                string[] dataTypeRowArray = dataTypeRow.Split(ConstProgram.SeparRows);

                string row = GetRowOnTitleAndConfig(titleRowArray, dataTypeRowArray, nameData);

                file.TitleRowWriter(titleRow);
                string testTitleRow = file.GetLineFilePositionRow(0);
                if (testTitleRow != titleRow)
                    file.WriteFile(titleRow, false);
                file.WriteFile(row, true);
            }
            else Console.WriteLine($"Сначала создайте конфигурацию или проверьте правильность написания названия => '{nameData}'");
        }
        public static string GetRowOnTitleAndConfig(string[] titleRowArray, string[] dataTypeRowArray, string nameData = ConstProgram.TaskName)
        {
            FormatterRows row = new(nameData);
            for (int i = 0; i < titleRowArray.Length; i++)
            {
                switch (dataTypeRowArray[i])
                {
                    case Commands.StringChar:
                        row.AddInRow(InputString($"введите {titleRowArray[i]}: "));
                        break;
                    case Commands.IntegerChar:
                        row.AddInRow(InputInteger($"введите {titleRowArray[i]}: ").ToString());
                        break;
                    case Commands.DoubleChar:
                        row.AddInRow(InputInteger($"введите {titleRowArray[i]}: ").ToString());
                        break;
                    case Commands.DateChar:
                        Console.WriteLine($"---ввод {titleRowArray[i]}---");
                        row.AddInRow(GetDate());
                        break;
                    case Commands.TimeChar:
                        Console.WriteLine($"---ввод {titleRowArray[i]}---");
                        row.AddInRow(GetTime());
                        break;
                    case Commands.DateAndTime:
                        Console.WriteLine($"---ввод {titleRowArray[i]}---");
                        row.AddInRow(GetDateAndTime());
                        break;
                    case Commands.NowDateTime:
                        row.AddInRow(FormatterRows.GetNowDateTime());
                        break;
                }
            }
            return row.Row.ToString();
        }
        public static void ClearAllTasks()
        {
            if (InputString("Вы уверены что хотите очистить весь файл task? (y/N): ") == ConstProgram.Yes)
            {
                string fileName = ConstProgram.TaskName;

                FormatterRows titleRow = new(fileName, FormatterRows.Type.title);
                FileWriter file = new(fileName);
                string[] titleRowArray = ConstProgram.TaskTitle;
                foreach (string pathTitleRow in titleRowArray)
                    titleRow.AddInRow(pathTitleRow);
                file.WriteFile(titleRow.Row.ToString(), false);
            }
            else System.Console.WriteLine("Будте внимателны");
        }
        public void ClearPartTask(string text) // недописан обязательно исправить
        {
            string fileName = ConstProgram.TaskName;
            FileWriter file = new(fileName);

            string[] titleRowArray = file.GetLineFilePositionRow(0).Split(ConstProgram.SeparRows);
            Dictionary<int, bool> tableClear = new Dictionary<int, bool>();

            System.Console.WriteLine($"Выберете в каком столбце искать {text} (y/N): ");
            for (int i = 0; i < titleRowArray.Length; ++i)
            {
                if (InputString($"{titleRowArray[i]}: ") == ConstProgram.Yes)
                    tableClear.Add(i, true);
                else tableClear.Add(i, false);
            }
        }
        public static void SearchPartData(string fileName = ConstProgram.StringNull, string text = ConstProgram.StringNull)
        {
            if (fileName == ConstProgram.StringNull)
                fileName = InputString("Ведите название файла: ");
            if (text == ConstProgram.StringNull)
                text = InputString("Поиск: ");

            FileWriter file = new(fileName);


            if (File.Exists(file.fullPath))
            {
                string[] titleRowArray = file.GetLineFilePositionRow(0).Split(ConstProgram.SeparRows);
                Dictionary<int, bool> tableClear = new Dictionary<int, bool>();


                System.Console.WriteLine($"Выберете в каком столбце искать {text} (y/N): ");
                for (int i = 0; i < titleRowArray.Length; ++i)
                {
                    if (InputString($"{titleRowArray[i]}: ") == ConstProgram.Yes)
                        tableClear.Add(i, true);
                    else tableClear.Add(i, false);
                }
                foreach (int i in tableClear.Keys)
                {
                    if (tableClear[i])
                        Console.WriteLine(file.GetLineFileDataOnPositionInRow(text, i));
                }
            }
            else System.Console.WriteLine(fileName + ": такого файла не существует.");
        }
        public static void PrintData(string fileName = "")
        {
            if (fileName == "")
                fileName = InputString("Ведите название файла: ");
            
            

            FileWriter file = new(fileName);

            try
            {
                using (StreamReader reader = new StreamReader(file.fullPath, Encoding.UTF8))
                {
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine($"Ошибка при чтении файла");
            }
        }
        public static void AddProfile()
        {
            FileWriter.AddRowInFile(ConstProgram.ProfileName, ConstProgram.ProfileTitle, ConstProgram.ProfileDataType);
        }
        public static void WriteCaption()
        {
            /*спрашивает и выводит текст субтитров созданный 
            методом CompText*/
            System.Console.WriteLine("За работу ответственны:");
            System.Console.WriteLine("\tШевченко Э. - README, исходный код, некоторые аспекты git;");
            System.Console.WriteLine("\tТитов М. - github, .gitignore, некоторый части исходного кода;");
        }
        public static void ProfileHelp()
        {
            Console.WriteLine("Команда для работы с профилями;");
            Console.WriteLine("При простом вызове, выводится первый добавленный пользователь: profile;");
            Console.WriteLine("При использовании как аргумент с командой add - добавляется новый пользователь: add profile;");
        }
        public static void Help()
        {
            Console.WriteLine("Данная программа позволяет пользователю создавать свой список заданий и контролировать их выполнение");
            Console.WriteLine("help - Выводит помощь по программе и её командам например: add help");
            Console.WriteLine("profile - Команда для работы с профилями");
            Console.WriteLine("add - Добавляет запись по базовой конфигурации: add task;");
            Console.WriteLine("Добавляет файл конфигурации: add config <File>;");
            Console.WriteLine("Добавляет запись по заранее созданной конфигурации: add <File>;");
            Console.WriteLine("clear - очищает выбранный файл");
            Console.WriteLine("search - Ищет все идентичные строчки в файле");
            Console.WriteLine("exit - Выход из программы либо из текущего действия");
            Console.WriteLine("print - Выводит всё содержимое файла");
        }
        public static void AddHelp()
        {
            Console.WriteLine("add - Добавляет записи(задания);");
            Console.WriteLine("Добавляет запись по базовой конфигурации: add task;");
            Console.WriteLine("Добавляет файл конфигурации: add config <File>;");
            Console.WriteLine("Добавляет запись по заранее созданной конфигурации: add <File>;");
            Console.WriteLine("Создаёт новый профиль: add profile;");
            Console.WriteLine("При добавлении print в конце команды, выводится добавленный текст");
        }
        public static void PrintHelp()
        {
            Console.WriteLine("print - Команда позволяющая получить содержимое файла;");
            Console.WriteLine("Примеры: print task; print <File>;");
            Console.WriteLine("Также может использоваться как аргумент в командах add task print/add <File> print,");
            Console.WriteLine("после создания записи её содержимое будет выведено в консоль;");
        }
        public static void SearchHelp()
        {
            Console.WriteLine("search - Ищет все идентичные строчки в файле;");
        }
    }
}
