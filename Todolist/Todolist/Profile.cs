namespace TodoList;

public class Profile
{
    public int BirthYear;
    public string FirstName;
    public string LastName;

    public Profile(string firstName, string lastName, int birthYear)
    {
        FirstName = firstName;
        LastName = lastName;
        BirthYear = birthYear;
    }

    public string GetInfo()
    {
        var age = DateTime.Now.Year - BirthYear;
        return $"{FirstName} {LastName}, {age}";
    }
}