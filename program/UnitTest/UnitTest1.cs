using Task;
namespace UnitTest;

public class UnitTest1
{
    [Fact]
    public void CSVLineNew()
    {
        string arg01 = "a|s|D|\"g\"";
        CSVLine actual = new(arg01);
        List<string> expected = ["a", "s", "D", "\"g\""];
        Assert.Equal(actual.Items, expected);
    }
    [Fact]
    public void CSVLineGet()
    {
        string arg01 = "a|s|D|\"g\"";
        CSVLine actual = new(arg01);
        Assert.Equal(actual.Get(), arg01);
    }
}
