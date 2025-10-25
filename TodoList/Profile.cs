using System;
public class Profile
{
    private string FirstName;
    private string LastName;
    private int BirthYear;
    public Profile(string firstName, string lastName, int birthYear)
    {
        FirstName = firstName;
        LastName = lastName;
        BirthYear = birthYear;
    }
    public string GetInfo()
    {
        return $"{FirstName}, {LastName}, {BirthYear}";
    }
}