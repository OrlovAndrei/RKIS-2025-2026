using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApp.Models;

public class Profile
{
    public Profile()
    {
        Id = Guid.NewGuid();
        Login = string.Empty;
        Password = string.Empty;
        FirstName = string.Empty;
        LastName = string.Empty;
        BirthYear = DateTime.Now.Year;
    }

    public Profile(string login, string password, string firstName, string lastName, int birthYear)
    {
        Id = Guid.NewGuid();
        Login = login ?? string.Empty;
        Password = password ?? string.Empty;
        FirstName = firstName ?? string.Empty;
        LastName = lastName ?? string.Empty;
        BirthYear = birthYear;
    }

    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(64)]
    public string Login { get; set; }

    [Required]
    [MaxLength(128)]
    public string Password { get; set; }

    [Required]
    [MaxLength(64)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(64)]
    public string LastName { get; set; }

    [Range(1900, 3000)]
    public int BirthYear { get; set; }

    public List<TodoItem> Todos { get; set; } = new();

    [NotMapped]
    public int Age => DateTime.Now.Year - BirthYear;

    public string DisplayName => $"{FirstName} {LastName} ({Login})";
}
