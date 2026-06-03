# TodoApp

Console todo application on .NET 8.0.

## Solution Structure

```text
TodoApp.sln
├── TodoApp.Models      - shared model library
├── TodoApp.Data        - EF Core context and repositories
├── TodoApp.Desktop     - WPF MVVM desktop application
├── TodoApp             - console application
└── TodoList.Server     - HttpListener sync server
```

`TodoApp.Desktop` depends on `TodoApp.Models` and `TodoApp.Data`.
`TodoApp.Data` depends on `TodoApp.Models`.
`TodoApp.Models` has no project dependencies.

## Storage

Local profile and task data is stored in SQLite through Entity Framework Core.

- Database file: `todos.db`
- DbContext: `Data/AppDbContext.cs`
- Migrations: `Migrations/`
- Profile repository: `Services/ProfileRepository.cs`
- Todo repository: `Services/TodoRepository.cs`

`FileManager` is obsolete and is not used for local todo storage.

## Desktop App

The WPF desktop app is implemented in `TodoApp.Desktop` using an MVVM-style structure.

- `MainViewModel`
- `LoginViewModel`
- `RegisterViewModel`
- `TodoListViewModel`
- `TaskFormViewModel`

Implemented desktop screens:

- login
- registration
- task list with search and status filtering
- add task form
- edit/delete task form

## EF Core Commands

Create a migration:

```powershell
dotnet ef migrations add InitialCreate --project .\TodoApp.csproj --startup-project .\TodoApp.csproj --output-dir Migrations
```

Apply migrations:

```powershell
dotnet ef database update --project .\TodoApp.csproj --startup-project .\TodoApp.csproj
```

## Commands

```text
help
profile [-o]
add "text"
add -m / --multiline
view [-i] [-s] [-d] [-a]
read <index>
status <index> <status>
update <index> "new text"
delete <index>
search [filters]
load <downloads_count> <download_size>
sync --pull
sync --push
undo
redo
exit
```

## HTTP Sync

The solution also contains `TodoList.Server`, a console HTTP server based on `HttpListener`.

Server URL:

```text
http://localhost:5000/
```

Supported endpoints:

```text
POST /profiles
GET  /profiles
POST /todos/{userId}
GET  /todos/{userId}
```

The server stores encrypted bytes only. The client-side `ApiDataStorage` serializes data to JSON, encrypts it with AES, and transfers it via `HttpClient`.
