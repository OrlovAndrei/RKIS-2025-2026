using System;

public class Profile
{
    private string firstName;
    private string lastName;
    private int birthYear;
    
    public string FirstName
    {
        get { return firstName; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Имя не может быть пустым");
            firstName = value;
        }
    }
    
    public string LastName
    {
        get { return lastName; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Фамилия не может быть пустой");
            lastName = value;
        }
    }
    
    public int BirthYear
    {
        get { return birthYear; }
        set
        {
            int currentYear = DateTime.Now.Year;
            if (value < 1900 || value > currentYear)
                throw new ArgumentException($"Год рождения должен быть между 1900 и {currentYear}");
            birthYear = value;
        }
    }
    
    public int Age
    {
        get { return DateTime.Now.Year - BirthYear; }
    }
    
    public Profile(string firstName, string lastName, int birthYear)
    {
        FirstName = firstName;
        LastName = lastName;
        BirthYear = birthYear;
    }
    
    public string GetInfo()
    {
        return $"{FirstName} {LastName}, возраст {Age}";
    }
    
    public void UpdateProfile(string firstName, string lastName, int birthYear)
    {
        FirstName = firstName;
        LastName = lastName;
        BirthYear = birthYear;
    }
    
    public override string ToString()
    {
        return GetInfo();
    }
}