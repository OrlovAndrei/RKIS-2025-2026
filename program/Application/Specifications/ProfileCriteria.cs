using Application.Specifications.Criteria;

namespace Application.Specifications;

/// <summary>
/// Содержит критерии для фильтрации профилей пользователей.
/// Поддерживает фильтрацию по ID, имени, фамилии и датам.
/// </summary>
public class ProfileCriteria
{
    /// <summary>
    /// Критерий фильтрации по ID профиля.
    /// </summary>
    public CriteriaId<Guid>? ProfileId { get; init; }
    
    /// <summary>
    /// Критерий фильтрации по имени пользователя.
    /// </summary>
    public CriteriaObj<string>? FirstName { get; init; }
    
    /// <summary>
    /// Критерий фильтрации по фамилии пользователя.
    /// </summary>
    public CriteriaObj<string>? LastName { get; init; }
    
    /// <summary>
    /// Критерий фильтрации по дате создания профиля.
    /// </summary>
    public CriteriaRangeObj<RangeObj<DateTime>, DateTime>? CreatedAt { get; init; }
    
    /// <summary>
    /// Критерий фильтрации по дате рождения.
    /// </summary>
    public CriteriaRangeObj<RangeObj<DateTime>, DateTime>? DateOfBirth { get; init; }
    /// <summary>
    /// Создает критерий фильтрации профиля по ID.
    /// </summary>
    /// <param name="profileId">ID профиля для поиска.</param>
    /// <returns>Новый объект ProfileCriteria с установленным критерием ID.</returns>
    public static ProfileCriteria ByProfileId(Guid profileId) => new() { ProfileId = new(profileId) };
    
    /// <summary>
    /// Создает критерий фильтрации профилей по имени.
    /// </summary>
    /// <param name="firstName">Имя для поиска.</param>
    /// <returns>Новый объект ProfileCriteria с установленным критерием имени.</returns>
    public static ProfileCriteria ByFirstName(string firstName) => new() { FirstName = new(firstName) };
    
    /// <summary>
    /// Создает критерий фильтрации профилей по фамилии.
    /// </summary>
    /// <param name="lastName">Фамилия для поиска.</param>
    /// <returns>Новый объект ProfileCriteria с установленным критерием фамилии.</returns>
    public static ProfileCriteria ByLastName(string lastName) => new() { LastName = new(lastName) };
    
    /// <summary>
    /// Создает критерий фильтрации профилей по диапазону дат создания.
    /// </summary>
    /// <param name="from">Начальная дата. Null означает отсутствие нижней границы.</param>
    /// <param name="to">Конечная дата. Null означает отсутствие верхней границы.</param>
    /// <returns>Новый объект ProfileCriteria с установленным критерием даты создания.</returns>
    public static ProfileCriteria ByCreatedAt(DateTime? from, DateTime? to) => new() { CreatedAt = new(new(from, to)) };
    
    /// <summary>
    /// Создает критерий фильтрации профилей по диапазону дат рождения.
    /// </summary>
    /// <param name="from">Начальная дата. Null означает отсутствие нижней границы.</param>
    /// <param name="to">Конечная дата. Null означает отсутствие верхней границы.</param>
    /// <returns>Новый объект ProfileCriteria с установленным критерием даты рождения.</returns>
    public static ProfileCriteria ByDateOfBirth(DateTime? from, DateTime? to) => new() { DateOfBirth = new(new(from, to)) };
    /// <summary>
    /// Инвертирует все критерии поиска.
    /// </summary>
    /// <returns>Новый объект ProfileCriteria с инвертированными критериями.</returns>
    public ProfileCriteria Not()
    {
        return new()
        {
            ProfileId = ProfileId?.Not(),
            FirstName = FirstName?.Not(),
            LastName = LastName?.Not(),
            CreatedAt = CreatedAt?.Not(),
            DateOfBirth = DateOfBirth?.Not(),
        };
    }
    /// <summary>
    /// Устанавливает тип сравнения на "Содержит" для текстовых критериев.
    /// </summary>
    /// <returns>Новый объект ProfileCriteria с критериями поиска по вхождению подстроки.</returns>
    public ProfileCriteria Contains()
    {
        return new()
        {
            FirstName = FirstName?.Contains(),
            LastName = LastName?.Contains(),
        };
    }
    /// <summary>
    /// Устанавливает тип сравнения на "Начинается с" для текстовых критериев.
    /// </summary>
    /// <returns>Новый объект ProfileCriteria с критериями поиска по началу подстроки.</returns>
    public ProfileCriteria StartsWith()
    {
        return new()
        {
            FirstName = FirstName?.StartsWith(),
            LastName = LastName?.StartsWith(),
        };
    }
    /// <summary>
    /// Устанавливает тип сравнения на "Точное совпадение" для текстовых критериев.
    /// </summary>
    /// <returns>Новый объект ProfileCriteria с критериями поиска по точному совпадению.</returns>
    public ProfileCriteria Equals()
    {
        return new()
        {
            FirstName = FirstName?.Equals(),
            LastName = LastName?.Equals(),
        };
    }
    /// <summary>
    /// Устанавливает тип сравнения на "Заканчивается на" для текстовых критериев.
    /// </summary>
    /// <returns>Новый объект ProfileCriteria с критериями поиска по окончанию подстроки.</returns>
    public ProfileCriteria EndWith()
    {
        return new()
        {
            FirstName = FirstName?.EndWith(),
            LastName = LastName?.EndWith(),
        };
    }
    /// <summary>
    /// Объединяет текущие критерии с другими критериями, приоритет имеют текущие критерии.
    /// </summary>
    /// <param name="other">Другие критерии для объединения.</param>
    /// <returns>Новый объект ProfileCriteria с объединенными критериями.</returns>
    public ProfileCriteria Add(ProfileCriteria other)
    {
        return new()
        {
            ProfileId = ProfileId ?? other.ProfileId,
            FirstName = FirstName ?? other.FirstName,
            LastName = LastName ?? other.LastName,
            CreatedAt = CreatedAt ?? other.CreatedAt,
            DateOfBirth = DateOfBirth ?? other.DateOfBirth,
        };
    }
    /// <summary>
    /// Оператор сложения для объединения двух критериев профилей.
    /// </summary>
    /// <param name="left">Левый операнд (имеет приоритет).</param>
    /// <param name="right">Правый операнд.</param>
    /// <returns>Новый объект ProfileCriteria с объединенными критериями.</returns>
    public static ProfileCriteria operator +(ProfileCriteria left, ProfileCriteria right)
    {
        return left.Add(right);
    }
}