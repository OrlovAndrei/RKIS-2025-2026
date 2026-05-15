using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Profiles")]
public class Profile
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Login { get; set; } = "";

    [Required]
    [MaxLength(100)]
    public string Password { get; set; } = "";

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = "";

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = "";

    [Range(1900, 2100)]
    public int BirthYear { get; set; }

    public ICollection<TodoItem> Todos { get; set; } = new List<TodoItem>();

    public Profile() { }

    public Profile(Guid id, string login, string password, string firstName, string lastName, int birthYear)
    {
        Id = id;
        Login = login ?? "user";
        Password = password ?? "";
        FirstName = firstName;
        LastName = lastName;
        BirthYear = birthYear;
    }

    public string GetInfo()
    {
        int age = DateTime.Now.Year - BirthYear;
        return $"{FirstName} {LastName}, возраст {age}, логин: {Login}";
    }

    public bool CheckPassword(string password) => Password == password;
}
