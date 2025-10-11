using System.Text.Json;

namespace Task
{
    class DataTypeJson
    {
        public DataType[] DataTypes { get; set; } = new DataType[0];
    }
    class DataType
    {
        public string Name { get; set; } = "";
        public string[] SpellingOptions { get; set; } = new string[0];
    }
    class SearchDataTypeOnJson
    {
        static DataTypeJson? openJsonFile = JsonSerializer.Deserialize<DataTypeJson?>
        (FileWriter.StringFromFileInMainFolder("DataType.json"));
        public static string ConvertingInputValues(string inputValue)
        {
            if (inputValue != ConstProgram.StringNull && openJsonFile != null &&
            openJsonFile.DataTypes.Length > 0)
            {
                foreach (var DataType in openJsonFile.DataTypes)
                {
                    if (DataType.Name != "" && DataType.SpellingOptions.Length > 0 &&
                    DataType.SpellingOptions.Contains(inputValue))
                    {
                        return DataType.Name;
                    }
                }
            }
            return ConstProgram.StringNull;
        }
    }
}