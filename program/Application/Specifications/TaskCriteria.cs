namespace Application.Specifications;

/// <summary>
/// Содержит критерии для фильтрации задач.
/// Поддерживает фильтрацию по ID, статусу, приоритету, названию, описанию и датам.
/// </summary>
public class TaskCriteria
{
    /// <summary>
    /// Критерий фильтрации по ID задачи.
    /// </summary>
    public CriteriaId<Guid>? TaskId { get; init; }
    
    /// <summary>
    /// Критерий фильтрации по ID состояния задачи.
    /// </summary>
    public CriteriaId<int>? StateId { get; init; }
    
    /// <summary>
    /// Критерий фильтрации по уровню приоритета задачи.
    /// </summary>
    public CriteriaRangeObj<RangeObj<int>, int>? PriorityLevel { get; init; }
    
    /// <summary>
    /// Критерий фильтрации по ID профиля владельца задачи.
    /// </summary>
    public CriteriaId<Guid>? ProfileId { get; init; }
    
    /// <summary>
    /// Критерий фильтрации по названию задачи.
    /// </summary>
    public CriteriaObj<string>? Name { get; init; }
    
    /// <summary>
    /// Критерий фильтрации по описанию задачи.
    /// </summary>
    public CriteriaObj<string>? Description { get; init; }
    
    /// <summary>
    /// Критерий фильтрации по дате создания задачи.
    /// </summary>
    public CriteriaRangeObj<RangeObj<DateTime>, DateTime>? CreatedAt { get; init; }
    
    /// <summary>
    /// Критерий фильтрации по сроку выполнения задачи.
    /// </summary>
    public CriteriaRangeObj<RangeObj<DateTime>, DateTime>? Deadline { get; init; }
    /// <summary>
    /// Создает критерий фильтрации задачи по ID.
    /// </summary>
    /// <param name="taskId">ID задачи для поиска.</param>
    /// <returns>Новый объект TaskCriteria с установленным критерием ID.</returns>
    public static TaskCriteria ByTaskId(Guid taskId) => new() { TaskId = new(taskId) };
    
    /// <summary>
    /// Создает критерий фильтрации задач по ID состояния.
    /// </summary>
    /// <param name="stateId">ID состояния для поиска.</param>
    /// <returns>Новый объект TaskCriteria с установленным критерием состояния.</returns>
    public static TaskCriteria ByStateId(int stateId) => new() { StateId = new(stateId) };
    
    /// <summary>
    /// Создает критерий фильтрации задач по диапазону уровня приоритета.
    /// </summary>
    /// <param name="from">Минимальный уровень приоритета. Null означает отсутствие нижней границы.</param>
    /// <param name="to">Максимальный уровень приоритета. Null означает отсутствие верхней границы.</param>
    /// <returns>Новый объект TaskCriteria с установленным критерием приоритета.</returns>
    public static TaskCriteria ByPriorityLevel(int? from, int? to) => new() { PriorityLevel = new(new(from, to)) };
    
    /// <summary>
    /// Создает критерий фильтрации задач по ID профиля владельца.
    /// </summary>
    /// <param name="profileId">ID профиля для поиска.</param>
    /// <returns>Новый объект TaskCriteria с установленным критерием профиля.</returns>
    public static TaskCriteria ByProfileId(Guid profileId) => new() { ProfileId = new(profileId) };
    
    /// <summary>
    /// Создает критерий фильтрации задач по названию.
    /// </summary>
    /// <param name="name">Название для поиска.</param>
    /// <returns>Новый объект TaskCriteria с установленным критерием названия.</returns>
    public static TaskCriteria ByName(string name) => new() { Name = new(name) };
    
    /// <summary>
    /// Создает критерий фильтрации задач по описанию.
    /// </summary>
    /// <param name="description">Описание для поиска.</param>
    /// <returns>Новый объект TaskCriteria с установленным критерием описания.</returns>
    public static TaskCriteria ByDescription(string description) => new() { Description = new(description) };
    
    /// <summary>
    /// Создает критерий фильтрации задач по диапазону дат создания.
    /// </summary>
    /// <param name="from">Начальная дата. Null означает отсутствие нижней границы.</param>
    /// <param name="to">Конечная дата. Null означает отсутствие верхней границы.</param>
    /// <returns>Новый объект TaskCriteria с установленным критерием даты создания.</returns>
    public static TaskCriteria ByCreatedAt(DateTime? from, DateTime? to) => new() { CreatedAt = new(new(from, to)) };
    
    /// <summary>
    /// Создает критерий фильтрации задач по диапазону сроков выполнения.
    /// </summary>
    /// <param name="from">Начальная дата. Null означает отсутствие нижней границы.</param>
    /// <param name="to">Конечная дата. Null означает отсутствие верхней границы.</param>
    /// <returns>Новый объект TaskCriteria с установленным критерием срока выполнения.</returns>
    public static TaskCriteria ByDeadline(DateTime? from, DateTime? to) => new() { Deadline = new(new(from, to)) };
	/// <summary>
    /// Инвертирует все критерии поиска.
    /// </summary>
    /// <returns>Новый объект TaskCriteria с инвертированными критериями.</returns>
    public TaskCriteria Not() => new()
	{
		TaskId = TaskId?.Not(),
		StateId = StateId?.Not(),
		PriorityLevel = PriorityLevel?.Not(),
		ProfileId = ProfileId?.Not(),
		Name = Name?.Not(),
		Description = Description?.Not(),
		CreatedAt = CreatedAt?.Not(),
		Deadline = Deadline?.Not(),
	};
	/// <summary>
    /// Устанавливает тип сравнения на "Содержит" для текстовых критериев.
    /// </summary>
    /// <returns>Новый объект TaskCriteria с критериями поиска по вхождению подстроки.</returns>
    public TaskCriteria Contains() => new()
	{
		Name = Name?.Contains(),
		Description = Description?.Contains(),
	};
	/// <summary>
    /// Устанавливает тип сравнения на "Начинается с" для текстовых критериев.
    /// </summary>
    /// <returns>Новый объект TaskCriteria с критериями поиска по началу подстроки.</returns>
    public TaskCriteria StartsWith() => new()
	{
		Name = Name?.StartsWith(),
		Description = Description?.StartsWith(),
	};
	/// <summary>
    /// Устанавливает тип сравнения на "Точное совпадение" для текстовых критериев.
    /// </summary>
    /// <returns>Новый объект TaskCriteria с критериями поиска по точному совпадению.</returns>
    public TaskCriteria Equals() => new()
	{
		Name = Name?.Equals(),
		Description = Description?.Equals(),
	};
	/// <summary>
    /// Устанавливает тип сравнения на "Заканчивается на" для текстовых критериев.
    /// </summary>
    /// <returns>Новый объект TaskCriteria с критериями поиска по окончанию подстроки.</returns>
    public TaskCriteria EndWith() => new()
	{
		Name = Name?.EndWith(),
		Description = Description?.EndWith(),
	};
	/// <summary>
    /// Объединяет текущие критерии с другими критериями, приоритет имеют текущие критерии.
    /// </summary>
    /// <param name="other">Другие критерии для объединения.</param>
    /// <returns>Новый объект TaskCriteria с объединенными критериями.</returns>
    public TaskCriteria Add(TaskCriteria other) => new()
	{
		TaskId = TaskId ?? other.TaskId,
		StateId = StateId ?? other.StateId,
		PriorityLevel = PriorityLevel ?? other.PriorityLevel,
		ProfileId = ProfileId ?? other.ProfileId,
		Name = Name ?? other.Name,
		Description = Description ?? other.Description,
		CreatedAt = CreatedAt ?? other.CreatedAt,
		Deadline = Deadline ?? other.Deadline,
	};
	/// <summary>
    /// Оператор сложения для объединения двух критериев задач.
    /// </summary>
    /// <param name="left">Левый операнд (имеет приоритет).</param>
    /// <param name="right">Правый операнд.</param>
    /// <returns>Новый объект TaskCriteria с объединенными критериями.</returns>
    public static TaskCriteria operator +(TaskCriteria left, TaskCriteria right)
    {
        return left.Add(right);
    }
}