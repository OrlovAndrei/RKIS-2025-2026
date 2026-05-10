using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TodoList.Models;

namespace TodoListDesktop.Services;

public sealed class TodoApiClient
{
    private readonly HttpClient _http;

    public TodoApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<LoginResponse> LoginAsync(string email, string password)
    {
        var response = await _http.PostAsJsonAsync("api/auth/login", new LoginRequest
        {
            Email = email,
            Password = password
        });

        return await ReadAuthorizedResponseAsync<LoginResponse>(response);
    }

    public async Task<LoginResponse> RegisterAsync(RegisterRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/auth/register", request);
        return await ReadAuthorizedResponseAsync<LoginResponse>(response);
    }

    public void SetToken(string token)
    {
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public void ClearToken()
    {
        _http.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<IReadOnlyList<TodoItemResponse>> GetTodosAsync()
    {
        var response = await _http.GetAsync("api/todos");
        return await ReadAuthorizedResponseAsync<List<TodoItemResponse>>(response);
    }

    public async Task<TodoItemResponse> CreateTodoAsync(string text)
    {
        var response = await _http.PostAsJsonAsync("api/todos", new CreateTodoRequest
        {
            Text = text
        });

        return await ReadAuthorizedResponseAsync<TodoItemResponse>(response);
    }

    public async Task UpdateTodoAsync(int id, string text, TodoStatus status)
    {
        var response = await _http.PutAsJsonAsync($"api/todos/{id}", new UpdateTodoRequest
        {
            Text = text,
            Status = status
        });

        await EnsureAuthorizedSuccessAsync(response);
    }

    public async Task UpdateStatusAsync(int id, TodoStatus status)
    {
        var response = await _http.PatchAsJsonAsync($"api/todos/{id}/status", new SetStatusRequest
        {
            Status = status
        });

        await EnsureAuthorizedSuccessAsync(response);
    }

    public async Task DeleteTodoAsync(int id)
    {
        var response = await _http.DeleteAsync($"api/todos/{id}");
        await EnsureAuthorizedSuccessAsync(response);
    }

    private static async Task<T> ReadAuthorizedResponseAsync<T>(HttpResponseMessage response)
    {
        await EnsureAuthorizedSuccessAsync(response);
        var value = await response.Content.ReadFromJsonAsync<T>();
        return value ?? throw new InvalidOperationException("Сервер вернул пустой ответ.");
    }

    private static async Task EnsureAuthorizedSuccessAsync(HttpResponseMessage response)
    {
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException("Сессия истекла. Войдите снова.");
        }

        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var message = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(message))
        {
            message = $"Ошибка API: {(int)response.StatusCode} {response.ReasonPhrase}";
        }

        throw new InvalidOperationException(message);
    }
}
