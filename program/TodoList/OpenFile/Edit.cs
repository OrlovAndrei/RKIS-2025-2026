using static TodoList.WriteToConsole;
namespace TodoList;

public partial class OpenFile
{
    public void ReIndexFile(bool message = false)
    {

        bool run = false;
        GetAllLine(out var allText);
        for (int i = 0; i < allText.Count(); i++)
        {
            if (int.TryParse(allText[i][0], out var j) && j != (i + 1))
            {
                allText[i][0] = (i + 1).ToString();
                run = true;
            }
        }
        if (run) { ColorMessage($"Изменения внесены.", ConsoleColor.Green); }
        WriteFile(allText, false);
    }

    public void EditingRow(string requiredData, int indexColumn, string modifiedData = "",
    int numberOfIterations = 1, int indexColumnWrite = -1)
    {
        if (indexColumnWrite == -1) { indexColumnWrite = indexColumn; }
        bool maxCounter = false;
        if (numberOfIterations == -1)
        {
            maxCounter = true;
        }
        GetAllLine(out var allText);
        int counter = 0;
        for (int i = 0; i < allText.Count(); i++)
        {
            if (counter >= numberOfIterations && !maxCounter)
            {
                break;
            }
            else if (allText[i][indexColumn] == requiredData)
            {
                if (modifiedData.Length != 0)
                {
                    allText[i][indexColumnWrite] = modifiedData;
                }
                else
                {
                    allText.RemoveAt(i);
                }
                counter++;
            }
        }
        ColorMessage($"Было перезаписано '{counter}' строк", ConsoleColor.Green);
        WriteFile(allText, false);
        ReIndexFile();
    }
}