# Руководство по использованию системы Input/Output

## Быстрый старт

### Базовое использование

```csharp
using Presentation.Input;
using Presentation.Output;

// Инициализация
var input = new ConsoleInput();
var output = new ConsoleOutput();

// Вывод
output.WriteSuccess("Добро пожаловать!");

// Ввод
string name = input.GetNonEmptyText("Введите имя: ");
int age = input.GetNumericInRange("Возраст (18-100): ", 18, 100);

// Вывод результата
output.WriteLine($"Благодарим, {name}! Вам {age} лет.");
```

## Использование фасада IOFacade

`IOFacade` - это удобная обертка для одновременной работы с вводом и выводом.

```csharp
using Presentation.Adapters;

var io = new IOFacade();

// Вывести инфо
io.ShowInfo("Начало работы");

// Спросить и получить ответ
string answer = io.AskText("Как вас зовут?");

// Вывести результат
io.ShowSuccess($"Рады видеть, {answer}!");
```

### Методы IOFacade

| Метод | Описание |
|-------|---------|
| `AskText()` | Спросить текст |
| `AskNumeric()` | Спросить число |
| `AskYesNo()` | Спросить да/нет |
| `AskSelection()` | Спросить выбор из списка |
| `ShowError()` | Показать ошибку (красный) |
| `ShowSuccess()` | Показать успех (зелёный) |
| `ShowWarning()` | Показать предупреждение (жёлтый) |
| `ShowInfo()` | Показать информацию (голубой) |
| `ShowException()` | Показать исключение |
| `ShowSeparator()` | Показать разделитель |

## Операции ввода

### Текстовый ввод (ITextInput)

```csharp
var input = new ConsoleInput();

// Однострочный текст
string name = input.GetShortText("Имя: ", notNull: true);

// Многострочный текст
string description = input.GetLongText("Описание (введите \\end для завершения): ");

// Текст не должен быть пустым
string email = input.GetNonEmptyText("Email: ");
```

### Числовой ввод (INumericInput)

```csharp
var input = new ConsoleInput();

// Просто число
int count = input.GetNumeric("Количество: ");

// Число в диапазоне
int age = input.GetNumericInRange("Возраст: ", 18, 120);

// Число не меньше значения
int year = input.GetNumericWithMin("Год: ", 1900);

// Число не больше значения
int month = input.GetNumericWithMax("Месяц: ", 12);

// Positive number (>= 0)
int positive = input.GetPositiveNumeric("Положительное число: ");
```

### Ввод пароля (IPasswordInput)

```csharp
var input = new ConsoleInput();

// Получить пароль (маскируется)
string password = input.GetPassword("Пароль: ");

// Получить и проверить пароль (требует повторения)
string checkedPassword = input.GetCheckedPassword();

// Проверить длину пароля
bool isValid = input.ValidatePasswordLength("password123", minLength: 8);
```

### Кнопки и выбор (IButtonInput)

```csharp
var input = new ConsoleInput();

// Да/нет выбор
bool agree = input.GetYesNoChoice("Согласны?");

// Выбор из списка строк
var options = new[] { "Опция 1", "Опция 2", "Опция 3" };
string selected = input.GetSelectionFromList(
    options: options,
    title: "Выберите вариант:"
);

// Выбор из словаря (возвращает ключ и значение)
var menu = new Dictionary<int, string>
{
    { 1, "Добавить" },
    { 2, "Удалить" },
    { 3, "Выход" }
};
var choice = input.GetSelectionFromDictionary(menu, "Меню:");

// Выбор клавиши
ConsoleKey key = input.GetKeyFromSet(
    "Нажмите (Y/N/C): ",
    defaultKey: ConsoleKey.Y,
    allowedKeys: ConsoleKey.N, ConsoleKey.C
);
```

## Операции вывода

### Простой вывод (IOutputProvider)

```csharp
var output = new ConsoleOutput();

// Вывести текст
output.WriteText("Текст без новой строки");

// Вывести текст с новой строкой
output.WriteLine("Текст с новой строкой");

// Пустая строка
output.WriteEmptyLine();
```

### Цветной вывод (IColoredOutput)

```csharp
var output = new ConsoleOutput();

// Вывести текст определённого цвета
output.WriteColoredMessage("Красный текст", ConsoleColor.Red);
output.WriteColoredLine("Зелёный текст", ConsoleColor.Green);

// Специализированные методы
output.WriteSuccess("Успешно!");     // Зелёный
output.WriteWarning("Предупреждение!"); // Жёлтый
output.WriteInfo("Информация");      // Голубой
```

### Вывод ошибок (IErrorOutput)

```csharp
var output = new ConsoleOutput();

// Просто ошибка
output.WriteError("Произошла ошибка!");

// Информация об исключении
try
{
    throw new Exception("Тестовая ошибка");
}
catch (Exception ex)
{
    // Краткая информация
    output.WriteException(ex);

    // Полная информация с трассировкой стека
    output.WriteDetailedException(ex);

    // Несколько ошибок
    output.WriteErrorLines("Ошибка 1", "Ошибка 2", "Ошибка 3");
}
```

## Адаптеры

### InputAdapter - работа только с вводом

```csharp
var input = new InputAdapter();

// Использует любой из интерфейсов ввода
string text = input.GetNonEmptyText("Текст: ");
int number = input.GetNumericInRange("Число: ", 1, 10);
string password = input.GetPassword("Пароль: ");
bool choice = input.GetYesNoChoice("Да/нет: ");
```

### OutputAdapter - работа только с выводом

```csharp
var output = new OutputAdapter();

// Использует все интерфейсы вывода
output.WriteSuccess("Успех!");
output.WriteError("Ошибка!");

// Специализированные методы
output.WriteMenu("Меню:",
    ("1", "Опция 1"),
    ("2", "Опция 2"),
    ("3", "Выход")
);

output.WriteList(new[] { "Item 1", "Item 2", "Item 3" }, item => item);

var data = new[] { "Row 1", "Row 2", "Row 3" };
output.WriteTable(data, row => row);
```

## Сложные примеры

### Пример 1: Интерактивное меню

```csharp
var io = new IOFacade();

while (true)
{
    io.ShowSeparator("Главное меню");
    
    var options = new[] { "Просмотр", "Добавить", "Удалить", "Выход" };
    string choice = io.AskSelection("Выберите:", options);
    
    switch (choice)
    {
        case "Просмотр":
            io.ShowInfo("Просмотр данных...");
            break;
        case "Добавить":
            io.ShowInfo("Добавление данных...");
            break;
        case "Удалить":
            io.ShowInfo("Удаление данных...");
            break;
        case "Выход":
            io.ShowSuccess("До свидания!");
            return;
    }
}
```

### Пример 2: Валидация формы

```csharp
var io = new IOFacade();

io.ShowSeparator("Регистрация");

// Ввод данных с валидацией
string username;
while (true)
{
    username = io.AskText("Имя пользователя (минимум 3 символа):");
    if (username.Length >= 3) break;
    io.ShowError("Слишком короткое имя!");
}

int age;
while (true)
{
    age = io.AskNumeric("Возраст:");
    if (age >= 18 && age <= 120) break;
    io.ShowError("Возраст должен быть от 18 до 120!");
}

// Подтверждение
if (io.AskYesNo("Подтвердить данные?"))
{
    io.ShowSuccess("Регистрация завершена!");
}
else
{
    io.ShowWarning("Отменено.");
}
```

### Пример 3: Обработка исключений

```csharp
var io = new IOFacade();

try
{
    // Какой-то код
    throw new InvalidOperationException("Критическая ошибка");
}
catch (Exception ex)
{
    io.Output.WriteDetailedException(ex);
    io.ShowWarning("Операция не выполнена. Попробуйте ещё раз.");
}
```

## Архитектурные принципы

### Разделение ответственности
- **Input** - только операции ввода
- **Output** - только операции вывода
- **Adapters** - комбинируют функциональность

### Интерфейсы как контракты
Каждый тип операции имеет свой интерфейс:
- `ITextInput` - текст
- `INumericInput` - числа
- `IPasswordInput` - пароли
- `IButtonInput` - выбор
- `IColoredOutput` - цвет
- `IErrorOutput` - ошибки

### Расширяемость
Можно создать свои реализации интерфейсов:
```csharp
public class FileOutput : IErrorOutput
{
    // Реализация для записи в файл
}

public class GUIInput : IButtonInput
{
    // Реализация для графического интерфейса
}
```

## Типичные паттерны использования

### Паттерн 1: Простой диалог

```csharp
var io = new IOFacade();
string name = io.AskText("Имя:");
io.ShowSuccess($"Здравствуйте, {name}!");
```

### Паттерн 2: Меню с опциями

```csharp
var io = new IOFacade();
var choice = io.AskSelection("Выберите:", new[] { "A", "B", "C" });
// Обработка choice
```

### Паттерн 3: Валидация

```csharp
var input = new ConsoleInput();
var output = new ConsoleOutput();

int value;
while (!int.TryParse(input.GetShortText("Число: "), out value) || value < 0)
{
    output.WriteError("Введите положительное число!");
}
```

## Рекомендации

1. **Используйте IOFacade** для большинства задач - это удобнее всего
2. **Используйте адаптеры**, если нужна только часть функциональности
3. **Передавайте интерфейсы в конструкторы**, а не конкретные реализации
4. **Обрабатывайте исключения** и выводите информацию через `WriteDetailedException`
5. **Следуйте соглашениям о цветах**:
   - Зелёный - успех
   - Жёлтый - предупреждение
   - Красный - ошибка
   - Голубой - информация

## Файловая структура

```
Presentation/
├── Input/
│   ├── Interfaces/
│   │   ├── IInputProvider.cs
│   │   ├── ITextInput.cs
│   │   ├── INumericInput.cs
│   │   ├── IPasswordInput.cs
│   │   └── IButtonInput.cs
│   ├── ConsoleInput.cs
│   ├── Button.cs
│   ├── Numeric.cs
│   ├── OneOf.cs
│   ├── Password.cs
│   ├── Text.cs
│   └── When.cs
│
├── Output/
│   ├── Interfaces/
│   │   ├── IOutputProvider.cs
│   │   ├── IColoredOutput.cs
│   │   └── IErrorOutput.cs
│   └── ConsoleOutput.cs
│
├── Adapters/
│   ├── IOFacade.cs
│   ├── InputAdapter.cs
│   └── OutputAdapter.cs
│
└── Examples/
    ├── InputOutputExamples.cs
    └── AdapterExamples.cs
```
