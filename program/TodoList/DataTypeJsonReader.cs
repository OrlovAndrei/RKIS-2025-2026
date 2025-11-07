using System.Text.Json;

namespace Task;

internal class DataTypeJson
{
	public DataType[] DataTypes { get; set; } = new DataType[0];
}

internal class DataType
{
	public string Name { get; set; } = "";
	public string[] SpellingOptions { get; set; } = new string[0];
}

internal class SearchDataTypeOnJson
{
	private static DataTypeJson? openJsonFile = JsonSerializer.Deserialize<DataTypeJson?>
	(OpenFile.GaySex("DataType.json"));
	public static string ConvertingInputValues(string inputValue)
	{
		if (inputValue.Length != 0 && openJsonFile != null &&
		openJsonFile.DataTypes.Length > 0)
		{
			foreach (var dataType in openJsonFile.DataTypes)
			{
				if (dataType.Name.Length != 0 && dataType.SpellingOptions.Length > 0 &&
				dataType.SpellingOptions.Contains(inputValue))
				{
					return dataType.Name;
				}
			}
		}
		return "";
	}
}