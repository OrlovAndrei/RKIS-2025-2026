using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace Todolist.Models
{
    [Table("Profiles")]
    public class Profile
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(50)]
        public string Login { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; private set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [Range(1900, 2100)]
        public int BirthYear { get; set; }

        public virtual ICollection<TodoItem> Todos { get; set; } = new List<TodoItem>();

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        public Profile() { }

        public Profile(string login, string password, string firstName, string lastName, int birthYear)
        {
            Login = login;
            SetPassword(password);
            FirstName = firstName;
            LastName = lastName;
            BirthYear = birthYear;
        }

        public void SetPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            PasswordHash = Convert.ToBase64String(hashBytes);
        }

        public bool CheckPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hash = Convert.ToBase64String(hashBytes);
            return PasswordHash == hash;
        }

        public string GetInfo()
        {
            return $"Логин: {Login}\nИмя: {FirstName} {LastName}\nГод рождения: {BirthYear}";
        }
    }
}