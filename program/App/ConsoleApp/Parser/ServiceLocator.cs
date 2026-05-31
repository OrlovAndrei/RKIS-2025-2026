namespace ConsoleApp.Parser;

/// <summary>
/// Простейшая заглушка для разрешения зависимостей из Presentation.
/// Замените реализацию на ваш DI-контейнер (или фабрику) в дальнейшем.
/// </summary>
internal static class ServiceLocator
{
    public static T Get<T>() where T : class
    {
        throw new NotImplementedException("Implement DI resolver in ServiceLocator.Get<T>()");
    }
}
