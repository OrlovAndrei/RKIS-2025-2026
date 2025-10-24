public class Profile
{
    // Свойства
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int BirthYear { get; set; }
    
    // Конструктор
    public Profile(string firstName, string lastName, int birthYear)
    {
        FirstName = firstName;
        LastName = lastName;
        BirthYear = birthYear;
    }
    
    // Метод GetInfo() - возвращает строку "Имя Фамилия, возраст N"
    public string GetInfo()
    {
        int currentYear = DateTime.Now.Year;
        int age = currentYear - BirthYear;
        
        return $"{FirstName} {LastName}, возраст {age}";
    }
    
    // Метод для удобного вывода (опционально)
    public override string ToString()
    {
        return GetInfo();
    }
}