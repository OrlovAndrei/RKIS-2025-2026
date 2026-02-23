# Архитектура системы Ввода/Вывода

## Описание

Система разделена на два основных направления: **Input** (ввод информации) и **Output** (вывод информации).

## Структура папок

```
Presentation/
├── Input/                      # Операции ввода информации
│   ├── Interfaces/            # Интерфейсы для ввода
│   │   ├── IInputProvider.cs      # Базовый интерфейс ввода
│   │   ├── ITextInput.cs          # Интерфейс ввода текста
│   │   ├── INumericInput.cs       # Интерфейс ввода чисел
│   │   ├── IPasswordInput.cs      # Интерфейс ввода пароля
│   │   └── IButtonInput.cs        # Интерфейс кнопок и выбора
│   ├── ConsoleInput.cs        # Реализация консольного ввода
│   ├── Button.cs              # Операции с кнопками
│   ├── Numeric.cs             # Операции с числами
│   ├── OneOf.cs               # Выбор из списка
│   ├── Password.cs            # Операции с пароль
│   ├── Text.cs                # Операции с текстом
│   └── When.cs                # Операции с датой и временем
│
└── Output/                     # Операции вывода информации
    ├── Interfaces/            # Интерфейсы для вывода
    │   ├── IOutputProvider.cs     # Базовый интерфейс вывода
    │   ├── IColoredOutput.cs      # Интерфейс цветного вывода
    │   └── IErrorOutput.cs        # Интерфейс вывода ошибок
    ├── ConsoleOutput.cs       # Реализация консольного вывода
    └── InputOutputExamples.cs # Примеры использования
```

## Интерфейсы

### INPUT - Ввод информации

#### IInputProvider
Базовый интерфейс для всех операций ввода.

```csharp
public interface IInputProvider
{
    string GetText(string prompt);
    int GetNumeric(string prompt);
    string GetPassword(string prompt);
}
```

#### ITextInput
Специализированный интерфейс для ввода текста.

```csharp
public interface ITextInput
{
    string GetShortText(string prompt, bool notNull = true);
    string GetLongText(string prompt);
    string GetNonEmptyText(string prompt);
}
```

#### INumericInput
Специализированный интерфейс для ввода чисел.

```csharp
public interface INumericInput
{
    int GetNumeric(string prompt);
    int GetNumericInRange(string prompt, int min, int max);
    int GetNumericWithMin(string prompt, int min);
    int GetNumericWithMax(string prompt, int max);
    int GetPositiveNumeric(string prompt);
}
```

#### IPasswordInput
Специализированный интерфейс для ввода пароля.

```csharp
public interface IPasswordInput
{
    string GetPassword(string prompt);
    string GetCheckedPassword();
    bool ValidatePasswordLength(string password, int minLength = 8);
}
```

#### IButtonInput
Специализированный интерфейс для кнопок и выбора вариантов.

```csharp
public interface IButtonInput
{
    bool GetYesNoChoice(string prompt);
    string GetSelectionFromList(IEnumerable<string> options, string? title = null, int pageSize = 3);
    KeyValuePair<int, string> GetSelectionFromDictionary(Dictionary<int, string> options, string? title = null, int pageSize = 3);
    ConsoleKey GetKeyFromSet(string prompt, ConsoleKey defaultKey = ConsoleKey.Y, params ConsoleKey[] allowedKeys);
}
```

### OUTPUT - Вывод информации

#### IOutputProvider
Базовый интерфейс для вывода текста.

```csharp
public interface IOutputProvider
{
    void WriteText(string message);
    void WriteLine(string message);
    void WriteEmptyLine();
}
```

#### IColoredOutput
Интерфейс для цветного вывода.

```csharp
public interface IColoredOutput : IOutputProvider
{
    void WriteColoredMessage(string message, ConsoleColor color);
    void WriteColoredLine(string message, ConsoleColor color);
    void WriteSuccess(string message);      // Зелёный
    void WriteWarning(string message);      // Жёлтый
    void WriteInfo(string message);         // Голубой
}
```

#### IErrorOutput
Интерфейс для вывода ошибок и исключений.

```csharp
public interface IErrorOutput : IColoredOutput
{
    void WriteError(string message);
    void WriteException(Exception exception);
    void WriteDetailedException(Exception exception);
    void WriteErrorLines(params string[] messages);
}
```

## Реализации

### ConsoleInput
Полная реализация всех интерфейсов ввода для консоли.

**Использование:**
```csharp
var input = new ConsoleInput();

// Текст
string name = input.GetNonEmptyText("Введите имя: ");

// Числа
int age = input.GetNumericInRange("Возраст (18-65): ", 18, 65);

// Пароль
string password = input.GetCheckedPassword();

// Выбор
bool agree = input.GetYesNoChoice("Согласны?");

// Из списка
string choice = input.GetSelectionFromList(
    new[] { "Опция 1", "Опция 2", "Опция 3" },
    "Выберите:"
);
```

### ConsoleOutput
Полная реализация всех интерфейсов вывода для консоли.

**Использование:**
```csharp
var output = new ConsoleOutput();

// Простой вывод
output.WriteLine("Обычное сообщение");

// Цветной вывод
output.WriteSuccess("Успешно!");
output.WriteWarning("Предупреждение!");
output.WriteError("Ошибка!");
output.WriteInfo("Информация");

// Вывод ошибок
try
{
    throw new Exception("Тестовая ошибка");
}
catch (Exception ex)
{
    output.WriteDetailedException(ex);
}
```

## Преимущества архитектуры

1. **Разделение ответственности** - Input и Output четко разделены
2. **Расширяемость** - легко добавить новые реализации (например, FileOutput, GUIInput)
3. **Тестируемость** - интерфейсы позволяют создавать mock-объекты для тестов
4. **Гибкость** - каждый класс использует то, что ему нужно из интерфейсов
5. **Консистентность** - единый способ ввода и вывода по всему приложению

## Примеры использования

См. класс `InputOutputExamples.cs` для полных примеров использования всех операций.

```csharp
// Пример полного потока
InputOutputExamples.ExampleCompleteFlow();
```
