using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Globalization;

namespace TodoList
{
    class MainClass
    {
        public static string ProcessInput(string text)
        {
            text = text.Trim();

            if (text == "exit") Environment.Exit(0);
            else if (text == "") return "NULL";

            return text;
        }
        
        public static int Survey(string text, int min, int max)
        /*метод выводящий сообшение и
        создаюший цикл пока пользователь не введет допустимые значения */
        {
            int result = -1; // сродникоду об ошибке
            string input;
            do
            {
                Console.Write(text); // перветственное сообщение 
                input = Console.ReadLine() ?? "NULL";
                if (!int.TryParse(input, out result)) input = ProcessInput(input);
            }
            while (result < min || result > max); //условия выхода
            return result;
        }
        public static string Survey(string text)
        {
            Console.Write(text);
            string str = Console.ReadLine() ?? "Неизвестно";
            str = ProcessInput(str);
            return str;
        }

        public static void Main()
        {
            Console.Write("Нужны ли титры(y/N): "); //Преветственная строка
            string? answer = Console.ReadLine();
            const string Captions = @"Работу сделали: 
Ответственный по исходному коду и README: Шевченко Э. 3831.9
Ответственный по .gitignore, всей git составляющей и некоторыми частями исходного кода: Титов М. 3831.9
";
            if (answer == "y") Console.WriteLine(Captions);
            var userFirstName = Survey("Введите ваше имя: ");
            var userLastName = Survey("Введите вашу фамилию: ");

            int oldestPersonYear = 123; //возраст самого старого человека на момент 2025 г.
            int NowYear = DateTime.Now.Year; // сегодняшнее время 

            int year = Survey("Введите ваш год рождения: ", NowYear - oldestPersonYear, NowYear);
            int month = Survey("Введите ваш месяц рождения: ", 1, 12);
            int day = Survey("Введите ваш день рождения: ", 1, 31);
            string fullDate = $"{day}.{month}.{year}";
            // опрос с допуcтимыми значениями для выхода из опроса

            CultureInfo dateProvider = CultureInfo.InvariantCulture;

            string dateFormat = "d.M.yyyy";
            DateTime birthDate = DateTime.ParseExact(fullDate, dateFormat, CultureInfo.InvariantCulture); // Переводим полученые значения в класс DateTime
            var userName = userFirstName + " " + userLastName; // Объединение для дальнейшего вывода 
            var text = $"\n{DateTime.Now}: добавлен пользователь {userName}, день рождения {birthDate.ToLongDateString()}\nвозраст - {DateTime.Now.Year - birthDate.Year}";
            string WriteTextStart01 = $"Date;userLastName;userFirstName;birthDate"; // создание верха таблици по формату Markdown
            string WriteText = $"{DateTime.Now};{userLastName};{userFirstName};{birthDate.ToShortDateString()}"; // Вводимые значения
            string dataPath = "/.config/RKIS-TodoList/"; // Расположение файла для UNIX и MacOSX
            string winDataPath = "\\RKIS-todoList\\"; // Расположение файла для Win32NT

            string? homePath = (Environment.OSVersion.Platform == PlatformID.Unix || // Если платформа UNIX или MacOSX, то homePath = $HOME
                   Environment.OSVersion.Platform == PlatformID.MacOSX)
                   ? Environment.GetEnvironmentVariable("HOME")
                   : Environment.ExpandEnvironmentVariables("%APPDATA%");   // Если платформа Win32NT, то homepath = \users\<username>\Documents 
            string fullPath;
            if (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
                fullPath = Path.Join(homePath, dataPath); // Если платформа UNIX или MacOSX, то мы соединяем homePath и dataPath
            else
                fullPath = Path.Join(homePath, winDataPath); // Если платформа Win32NT, то мы соединяем homePath и winDataPath
            string filePath = Path.Join(fullPath, "data.csv"); // Полный путь к файлу, используется для его создания и/или чтения

            bool startText = true; // флаг наличия или отсутствия заголовка таблици
            FileStream? file = null; // инициализация класса файла
            DirectoryInfo? directory = new DirectoryInfo(fullPath); // Инициализируем объект класса для создания директории
            if (!directory.Exists) Directory.CreateDirectory(fullPath); // Если директория не существует, то мы её создаём по пути fullPath
            try
            {
                using (file = new FileStream(filePath, FileMode.OpenOrCreate)) {}
                // Проверка на наличие файла и если его нет создает новый 

                using (StreamReader reader = new StreamReader(filePath, Encoding.UTF8))
                // Поиск заголовка таблици
                {
                    string? line = reader.ReadLine();
                    if (line != WriteTextStart01) startText = false;
                }

                using (StreamWriter writer = new StreamWriter(filePath, true, Encoding.UTF8))
                // добовление строк в таблицу 
                {
                    if (!startText) writer.WriteLine(WriteTextStart01);
                    writer.WriteLine(WriteText);
                    writer.Close();
                }
                Console.WriteLine(text); // оповещение о завершении операции 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка при записи файла: {ex.Message}");
            }
        }
    }
}
