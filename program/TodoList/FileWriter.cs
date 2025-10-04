// This file contains everything related to generating and reading paths, files - PoneMaurice
using System.Text;
namespace Task
{
    public static class FileWriter
    {
        public const string stringNull = "NULL";
        const string seporRows = "|";
        public static string CreatePath(string nameFile) // Function for creating file path - PoneMaurice
        {
            /*Создание актульного пути под каждый нужный файл находящийся в деректории с конфигами*/
            string dataPath = "/.config/RKIS-TodoList/"; // Расположение файла для UNIX и MacOSX
            string winDataPath = "\\RKIS-todoList\\"; // Расположение файла для Win32NT
            string fullPath;

            string? homePath = (Environment.OSVersion.Platform == PlatformID.Unix || // Если платформа UNIX или MacOSX, то homePath = $HOME
                   Environment.OSVersion.Platform == PlatformID.MacOSX)
                   ? Environment.GetEnvironmentVariable("HOME")
                   : Environment.ExpandEnvironmentVariables("%APPDATA%");   // Если платформа Win32NT, то homepath = \users\<username>\Documents 
            if (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
                fullPath = Path.Join(homePath, dataPath); // Если платформа UNIX или MacOSX, то мы соединяем homePath и dataPath
            else
                fullPath = Path.Join(homePath, winDataPath); // Если платформа Win32NT, то мы соединяем homePath и winDataPath
            DirectoryInfo? directory = new DirectoryInfo(fullPath); // Инициализируем объект класса для создания директории
            if (!directory.Exists) Directory.CreateDirectory(fullPath); // Если директория не существует, то мы её создаём по пути fullPath
            fullPath = Path.Join(fullPath, $"{nameFile}.csv");
            return fullPath;
        }
        public static string TitleRowWriter(string nameFile, string titleRow) // Function for writing rows in tasks titles - PoneMaurice
        {
            string fullPath = CreatePath(nameFile);
            if (!File.Exists(fullPath))
                using (var fs = new FileStream(fullPath, FileMode.CreateNew,
                FileAccess.Write, FileShare.Read))
                {
                    using (var sw = new StreamWriter(fs, Encoding.UTF8))
                    {
                        sw.WriteLine(titleRow);
                    }
                }
            return fullPath;
        }
        public static void WriteFile(string fullPath, string dataFile, bool noRewrite)
        {
            /*Запись в конец файла строки*/
            try
            {
                if (noRewrite)
                {
                    using (StreamWriter sw = new StreamWriter(fullPath, true, Encoding.UTF8))
                    {
                        sw.WriteLine(dataFile);
                    }
                }
                else
                {
                    using (StreamWriter sw = new StreamWriter(fullPath, false, Encoding.UTF8))
                    {
                        sw.WriteLine(dataFile);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex}\n");
            }
        }
        public static string GetLineFileDataOnPositionInRow(string fullPath, string dataFile, int positionInRow)
        {
            /*Возвращает строку если ее элемент по заданой позиции 
            соответствует введеным нами данным*/
            try
            {
                using (StreamReader reader = new StreamReader(fullPath, Encoding.UTF8))
                {
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] pathLine = line.Split(FormatRows.seporRows);
                        if (pathLine.Length > positionInRow)
                        {
                            if (pathLine[positionInRow] == dataFile)
                                return line;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex}\n");
            }
            return stringNull;
        }
        public static string GetLineFilePositionRow(string fullPath, int positionRow)
        {
            /*Возвращает строку если ее элемент по заданной позиции 
            соответствует введеным нами данным*/
            try
            {
                using (StreamReader reader = new StreamReader(fullPath, Encoding.UTF8))
                {
                    string? line;
                    int numLine = 0;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (numLine == positionRow)
                        {
                            return line;
                        }
                        ++numLine;
                    }
                }
            }
            catch (Exception)
            {
                System.Console.WriteLine("не найдено");    
            }
            return stringNull;
        }
        public static string GetLineFileData(string fullPath, string dataFile)
        {
            /*перегрузка метода только без позиции*/
            try
            {
                using (StreamReader reader = new StreamReader(fullPath, Encoding.UTF8))
                {
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line == dataFile)
                        {
                            return line;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex}\n");
            }
            return stringNull;
        }
        public static int GetLeghtFile(string fullPath)
        {
            int numLine = 0;
            try
            {
                if (File.Exists(fullPath))
                {
                    using (StreamReader reader = new StreamReader(fullPath, Encoding.UTF8))
                    {
                        string? line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            ++numLine;
                        }
                    }
                }
                else return 1;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            return numLine;
        }
    }
}