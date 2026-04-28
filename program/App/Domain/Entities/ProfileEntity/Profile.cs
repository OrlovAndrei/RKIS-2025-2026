namespace Domain.Entities.ProfileEntity;

public class Profile
{
	public const int MaxFirstNameLength = 100;
	public const int MaxLastNameLength = 100;
	public Guid ProfileId { get; private set; }
	public string FirstName
	{
		get; private set
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				throw new ArgumentException("First name cannot be null or empty.", nameof(FirstName));
			}
			if (value.Length > MaxFirstNameLength)
			{
				throw new ArgumentException($"First name cannot exceed {MaxFirstNameLength} characters.", nameof(FirstName));
			}
			field = value;
		}
	}
	public string LastName
	{
		get; private set
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				throw new ArgumentException("Last name cannot be null or empty.", nameof(LastName));
			}
			if (value.Length > MaxLastNameLength)
			{
				throw new ArgumentException($"Last name cannot exceed {MaxLastNameLength} characters.", nameof(LastName));
			}
			field = value;
		}
	}
	public DateTime DateOfBirth
	{
		get; private set
		{
			if (value > DateTime.Now)
			{
				throw new ArgumentException("Date of birth cannot be in the future.", nameof(DateOfBirth));
			}
			if (value < DateTime.Now.AddYears(-150))
			{
				throw new ArgumentException("Date of birth cannot be more than 150 years ago.", nameof(DateOfBirth));
			}
			field = value;
		}
	}
	public DateTime CreatedAt
	{
		get; private set
		{
			if (value > DateTime.Now)
			{
				throw new ArgumentException("Created at cannot be in the future.", nameof(CreatedAt));
			}
			field = value;
		}
	}
	public string PasswordHash
	{
		get; private set
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				throw new ArgumentException("Password hash cannot be null or empty.", nameof(PasswordHash));
			}
			field = value;
		}
	}
	public Profile(
		string firstName,
		string lastName,
		DateTime dateOfBirth,
		string passwordHash)
	{
		ProfileId = Guid.NewGuid();
		FirstName = firstName;
		LastName = lastName;
		DateOfBirth = dateOfBirth;
		CreatedAt = DateTime.UtcNow;
		PasswordHash = passwordHash;
	}
#pragma warning disable CS9264 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
	private Profile() { }
#pragma warning restore CS9264 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
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
		FirstName = firstName,
		LastName = lastName,
		DateOfBirth = dateOfBirth
	};
	public void UpdateFirstName(string firstName)
	{
		FirstName = firstName;
	}
	public void UpdateLastName(string lastName)
	{
		LastName = lastName;
	}
	public void UpdateDateOfBirth(DateTime dateOfBirth)
	{
		DateOfBirth = dateOfBirth;
	}
}