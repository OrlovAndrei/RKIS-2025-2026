// This file contains everything related to generating and reading paths, files - PoneMaurice
using System.Text;
namespace Task
{
    public class FileWriter
    {
        public string fullPath;
        public string nameFile;
        public FileWriter(string fileName)
        {
            nameFile = fileName;
            fullPath = CreatePath(nameFile);
        }
        public string CreatePath(string nameFile) // Function for creating file path - PoneMaurice
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
        public static string GetPathToZhopa()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string[] huis = baseDirectory.Split("/");
            StringBuilder huiBolshoy = new();
            foreach (string indexHui in huis)
            {
                if (indexHui != "bin")
                {
                    huiBolshoy.Append(indexHui + "/");
                }
                else
                {
                    break;
                }
            }
            return huiBolshoy.ToString();
        }
        public static string StringFromFileInMainFolder(string fileName)
        {
            string huiBolshoy = FileWriter.GetPathToZhopa();
            string sex = Path.Join(huiBolshoy, fileName);
            return File.ReadAllText(sex);
        }
        public string TitleRowWriter(string titleRow) // Function for writing rows in tasks titles - PoneMaurice
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
        public void WriteFile(string dataFile, bool noRewrite)
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
        public string GetLineFileDataOnPositionInRow(string dataFile, int positionInRow)
        {
            /*Возвращает строку если ее элемент по заданной позиции 
            соответствует введенным нами данным*/
            try
            {
                using (StreamReader reader = new StreamReader(fullPath, Encoding.UTF8))
                {
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] pathLine = line.Split(ConstProgram.SeparRows);
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
            return ConstProgram.StringNull;
        }
        public string GetLineFilePositionRow(int positionRow)
        {
            /*Возвращает строку если ее элемент по заданной позиции 
            соответствует введенным нами данным*/
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
            return ConstProgram.StringNull;
        }
        public string GetLineFileData(string dataFile)
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
            return ConstProgram.StringNull;
        }
        public int GetLengthFile()
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
        static public void AddRowInFile(string nameFile, string[] titleRowArray, string[] dataTypeRowArray)
        {
            FileWriter file = new(nameFile);
            FormatterRows titleRow = new(nameFile, FormatterRows.Type.title);
            string row = Commands.GetRowOnTitleAndConfig(titleRowArray, dataTypeRowArray);
            titleRow.AddArrayInRow(titleRowArray);
            file.TitleRowWriter(titleRow.Row.ToString());
            file.WriteFile(row, true);
        }
    }
}