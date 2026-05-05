using TodoList.Interfaces;

namespace TodoList.Entity;

public class Profile
{
	public const int MaxLoginLength = 15;
	public const int MaxFirstNameLength = 15;
	public const int MaxLastNameLength = 15;
	private readonly IClock _clock;
	private readonly IHasher _hasher;
	public Guid Id { get; private set; }
	public string Login
	{
		get;
		private set
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(value);
			if (value.Length > MaxLoginLength)
			{
				throw new ArgumentException(value);
			}
			field = value;
		}
	}
	public string? FirstName
	{
		get;
		private set
		{
			if (value?.Length > MaxFirstNameLength)
			{
				throw new ArgumentException(value);
			}
			field = value;
		}
	}
	public string? LastName
	{
		get;
		private set
		{
			if (value?.Length > MaxLastNameLength)
			{
				throw new ArgumentException(value);
			}
			field = value;
		}
	}
	public DateOnly DateOfBirth
	{
		get;
		private set
		{
			DateTime now = _clock.Now();
			if (value > new DateOnly(now.Year, now.Month, now.Day))
			{
				throw new ArgumentException();
			}
			field = value;
		}
	}
	public string PasswordHash { get; private set; }
	public ICollection<TodoItem>? TodoItems { get; private set; }
	public Profile(
		string login,
		DateOnly birthYear,
		string password,
		IClock clock,
		IHasher hasher
	)
	{
		_hasher = hasher;
		Login = login;
		DateOfBirth = birthYear;
		PasswordHash = _hasher.Hashed(password);
		_clock = clock;
	}
	public Profile(
		string login,
		DateOnly birthYear,
		string password,
		IClock clock,
		IHasher hasher,
		string? firstName = null,
		string? lastName = null) : this(
			login: login,
			birthYear: birthYear,
			password: password,
			clock: clock,
			hasher: hasher
		)
	{
		FirstName = firstName;
		LastName = lastName;
	}
#pragma warning disable CS9264, CS8618
	private Profile() { }
#pragma warning restore CS9264, CS8618
	public void UpdateLogin(string login)
	{
		Login = login;
	}
	public void UpdateFirstName(string? firstName)
	{
		FirstName = firstName;
	}
	public void UpdateLastName(string? lastName)
	{
		LastName = lastName;
	}
	public void UpdateDateOfBirth(DateOnly dateOfBirth)
	{
		DateOfBirth = dateOfBirth;
	}
}