## Todolist

Практическое задание по дисциплине **РКИС** (группа 3834)

### Что изменено

Хранение данных переведено с файлов на **SQLite** через **Entity Framework Core**.

- Удалён `FileManager`.
- Добавлен `AppDbContext` с подключением `Data Source=todos.db`.
- Добавлены репозитории:
  - `ProfileRepository`
  - `TodoRepository`
- Локальное хранилище `SqliteDataStorage` реализует `IDataStorage` и работает через репозитории.
- Данные профилей и задач сохраняются в `todos.db` и сохраняются между перезапусками приложения.

### Лабораторная 7: WPF и MVVM

Добавлена настольная версия приложения на **WPF** с разбиением на несколько проектов:

- `TodoApp.Models` — модели `Profile`, `TodoItem`, `TodoStatus`;
- `TodoApp.Data` — `AppDbContext`, EF Core, репозитории и миграция;
- `TodoApp.Desktop` — WPF-интерфейс, MVVM, навигация между экранами.

Реализованы экраны входа, регистрации, списка задач, добавления и редактирования.
В desktop-версии доступны просмотр задач, поиск и фильтрация, добавление, редактирование,
удаление и изменение статуса задачи.

### EF Core пакеты

В проект подключены:

- `Microsoft.EntityFrameworkCore` `9.0.5`
- `Microsoft.EntityFrameworkCore.Sqlite` `9.0.5`
- `Microsoft.EntityFrameworkCore.Tools` `9.0.5`
- локальный инструмент `dotnet-ef` `9.0.5` (через `.config/dotnet-tools.json`)

### Модели и ограничения

`Profile`:

- PK: `Id`
- обязательные поля: `Login`, `Password`, `FirstName`, `LastName`
- ограничение года рождения: `[Range(1900, 3000)]`
- поле, не сохраняемое в БД: `Age` (`[NotMapped]`)

`TodoItem`:

- PK: `Id`
- FK: `ProfileId`
- связь `Profile (1) -> (N) TodoItem`
- обязательные поля: `Text`, `Status`, `ProfileId`, `SortOrder`
- поле, не сохраняемое в БД: `IsCompleted` (`[NotMapped]`)

Дополнительно:

- уникальный индекс на `Profile.Login`
- каскадное удаление задач при удалении профиля
- сортировка задач по `SortOrder` внутри профиля

### Миграции

Создана миграция:

- `InitialCreate` (`Migrations/20260330090728_InitialCreate.cs`)

Команды:

```bash
dotnet dotnet-ef migrations add InitialCreate
dotnet dotnet-ef database update
```

### Репозиторий задач

`TodoRepository` содержит требуемые методы:

- `GetAll()`
- `Add()`
- `Update()`
- `Delete()`
- `SetStatus()`

### Запуск

```bash
dotnet build Todolist.csproj
dotnet run --project Todolist.csproj
```

После первого запуска/миграции рядом с проектом появляется файл базы данных `todos.db`.
