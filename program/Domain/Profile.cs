using Domain.Interfaces;

namespace Domain;

public class Profile
{
	public Guid ProfileId { get; private set; }
	public string FirstName { get; private set; }
	public string LastName { get; private set; }
	public DateTime DateOfBirth { get; private set; }
	public DateTime CreatedAt { get; private set; }
	public string PasswordHash { get; private set; }
	public Profile(
		string firstName,
		string lastName,
		DateTime dateOfBirth,
		string password,
		IPasswordHashed hashed)
	{
		if (string.IsNullOrWhiteSpace(password))
		{
			throw new ArgumentException("Password hash cannot be null or empty.", nameof(password));
		}
		if (password.Length < 8)
		{
			throw new ArgumentException("The password must be at least 8 characters long.", nameof(password));
		}
		ProfileId = Guid.NewGuid();
		FirstName = CheckFirstName(firstName);
		LastName = CheckLastName(lastName);
		DateOfBirth = CheckDateOfBirth(dateOfBirth);
		CreatedAt = DateTime.UtcNow;
		PasswordHash = hashed.HashedAsync(password, CreatedAt).Result;
	}
#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
	private Profile() { }
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
    public static Profile Restore(
		Guid profileId,
		string firstName,
		string lastName,
		DateTime dateOfBirth,
		DateTime createdAt,
		string passwordHash) => new()
		{
			ProfileId = profileId,
			FirstName = firstName,
			LastName = lastName,
			CreatedAt = createdAt,
			DateOfBirth = dateOfBirth,
			PasswordHash = passwordHash
		};
	public static Profile CreateUpdateObj(
		Guid profileId,
		string firstName,
		string lastName,
		DateTime dateOfBirth
	) => new()
	{
		ProfileId = profileId,
		FirstName = CheckFirstName(firstName),
		LastName = CheckLastName(lastName),
		DateOfBirth = CheckDateOfBirth(dateOfBirth)
	};
	private static string CheckFirstName(string firstName)
	{
		if (string.IsNullOrWhiteSpace(firstName))
		{
			throw new ArgumentException("First name cannot be null or empty.", nameof(firstName));
		}
		if (firstName.Length > 50)
		{
			throw new ArgumentException("First name cannot exceed 50 characters.", nameof(firstName));
		}
		return firstName;
	}
	private static string CheckLastName(string lastName)
	{
		if (string.IsNullOrWhiteSpace(lastName))
		{
			throw new ArgumentException("Last name cannot be null or empty.", nameof(lastName));
		}
		if (lastName.Length > 50)
		{
			throw new ArgumentException("Last name cannot exceed 50 characters.", nameof(lastName));
		}
		return lastName;
	}
	private static DateTime CheckDateOfBirth(DateTime dateOfBirth)
	{
		if (dateOfBirth > DateTime.Now)
		{
			throw new ArgumentException("Date of birth cannot be in the future.", nameof(dateOfBirth));
		}
		if (dateOfBirth < DateTime.Now.AddYears(-150))
		{
			throw new ArgumentException("Date of birth cannot be more than 150 years ago.", nameof(dateOfBirth));
		}
		return dateOfBirth;
	}
}