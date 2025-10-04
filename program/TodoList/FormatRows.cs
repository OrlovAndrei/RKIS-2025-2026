// This file contains row and date formatting - PoneMaurice

using System.Data;
using System.Globalization;
using System.Text;

namespace Task
{
    public class FormatRows(string nameFile, bool TitleRow = false, bool dataTypeRow = false)
    {
        public const string seporRows = "|";
        public StringBuilder Row = new();
        int Num;        
        public string PathToFile = FileWriter.CreatePath(nameFile);
        public void AddInRow(string pathRow)
        {
            /*Форматирует массив данных под будущию таблицу csv*/
            
            if (PathToFile == null)
            {
                PathToFile = FileWriter.CreatePath(nameFile);
            }
            if (Row.ToString() == "" && dataTypeRow)
            {
                Num = FileWriter.GetLeghtFile(PathToFile);
                Row.Append("counter" + FormatRows.seporRows + pathRow);
            }
            else if (Row.ToString() == "" && !TitleRow)
            {
                Num = FileWriter.GetLeghtFile(PathToFile);
                Row.Append(Num + FormatRows.seporRows + pathRow);
            }
            else if (Row.ToString() == "" && TitleRow)
            {
                Num = 0;
                Row.Append("numbering" + FormatRows.seporRows + pathRow);
            }
            else Row.Append(FormatRows.seporRows + pathRow);
        }
        public void AddInRow(string[] pathRow)
        {
            
            if (PathToFile == null)
            {
                PathToFile = FileWriter.CreatePath(nameFile);
            }
        }
        public int GetLeghtRow()
        {
            if (Row.Length != 0)
            {
                
                return Row.ToString().Split(FormatRows.seporRows).Count();
            }
            else return 0;
        }
        public static string GetNowDateTime()
        {
            /*возвращает сегодняшнюю дату и время в нужном формате*/
            DateTime nowDate = DateTime.Now;
            StringBuilder dateTime = new();
            dateTime.Append(nowDate.ToShortDateString() +
                " " + nowDate.ToShortTimeString());
            return dateTime.ToString();
        }
    }
}