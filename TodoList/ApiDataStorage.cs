using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TodoList;

public class ApiDataStorage : IDataStorage
{
    private readonly string _baseUrl;
    private readonly byte[] _key;
    private readonly byte[] _iv;
    private readonly HttpClient _httpClient;

    public ApiDataStorage(string baseUrl, byte[] key, byte[] iv)
    {
        _baseUrl = baseUrl;
        _key = key;
        _iv = iv;
        _httpClient = new HttpClient();
        _httpClient.Timeout = TimeSpan.FromSeconds(10);
    }

    public async Task<bool> IsServerAvailableAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/profiles");
            return response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.NotFound;
        }
        catch
        {
            return false;
        }
    }

    public void SaveProfiles(IEnumerable<Profile> profiles)
    {
        var task = SaveProfilesAsync(profiles);
        task.Wait();
    }

    public IEnumerable<Profile> LoadProfiles()
    {
        var task = LoadProfilesAsync();
        task.Wait();
        return task.Result;
    }

    public void SaveTodos(Guid userId, IEnumerable<TodoItem> todos)
    {
        var task = SaveTodosAsync(userId, todos);
        task.Wait();
    }

    public IEnumerable<TodoItem> LoadTodos(Guid userId)
    {
        var task = LoadTodosAsync(userId);
        task.Wait();
        return task.Result;
    }

    private async Task SaveProfilesAsync(IEnumerable<Profile> profiles)
    {
        // Сериализация в JSON
        var profilesDto = profiles.Select(p => new ProfileDto
        {
            Id = p.Id,
            Login = p.Login,
            Password = p.Password,
            FirstName = p.FirstName,
            LastName = p.LastName,
            BirthYear = p.BirthYear
        });

        var json = JsonSerializer.Serialize(profilesDto);
        var jsonBytes = Encoding.UTF8.GetBytes(json);

        // Шифрование
        var encryptedData = await EncryptAsync(jsonBytes);

        // Отправка на сервер
        var content = new ByteArrayContent(encryptedData);
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
        var response = await _httpClient.PostAsync($"{_baseUrl}/profiles", content);
        response.EnsureSuccessStatusCode();
    }

    private async Task<List<Profile>> LoadProfilesAsync()
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/profiles");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return new List<Profile>();

        response.EnsureSuccessStatusCode();
        var encryptedData = await response.Content.ReadAsByteArrayAsync();

        if (encryptedData.Length == 0)
            return new List<Profile>();

        // Расшифровка
        var jsonBytes = await DecryptAsync(encryptedData);
        var json = Encoding.UTF8.GetString(jsonBytes);
        var profilesDto = JsonSerializer.Deserialize<List<ProfileDto>>(json);
        return profilesDto?.Select(dto => new Profile(dto.Id, dto.Login, dto.Password, dto.FirstName, dto.LastName, dto.BirthYear)).ToList() ?? new List<Profile>();
    }

    private async Task SaveTodosAsync(Guid userId, IEnumerable<TodoItem> todos)
    {
        var todosDto = todos.Select(t => new TodoItemDto
        {
            Text = t.Text,
            Status = t.Status.ToString(),
            LastUpdate = t.LastUpdate
        });

        var json = JsonSerializer.Serialize(todosDto);
        var jsonBytes = Encoding.UTF8.GetBytes(json);
        var encryptedData = await EncryptAsync(jsonBytes);

        var content = new ByteArrayContent(encryptedData);
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
        var response = await _httpClient.PostAsync($"{_baseUrl}/todos/{userId}", content);
        response.EnsureSuccessStatusCode();
    }

    private async Task<List<TodoItem>> LoadTodosAsync(Guid userId)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/todos/{userId}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return new List<TodoItem>();

        response.EnsureSuccessStatusCode();
        var encryptedData = await response.Content.ReadAsByteArrayAsync();

        if (encryptedData.Length == 0)
            return new List<TodoItem>();

        var jsonBytes = await DecryptAsync(encryptedData);
        var json = Encoding.UTF8.GetString(jsonBytes);
        var todosDto = JsonSerializer.Deserialize<List<TodoItemDto>>(json);
        return todosDto?.Select(dto => new TodoItem(dto.Text, Enum.Parse<TodoStatus>(dto.Status), dto.LastUpdate)).ToList() ?? new List<TodoItem>();
    }

    private async Task<byte[]> EncryptAsync(byte[] data)
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;

        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
        await cryptoStream.WriteAsync(data, 0, data.Length);
        await cryptoStream.FlushFinalBlockAsync();
        return memoryStream.ToArray();
    }

    private async Task<byte[]> DecryptAsync(byte[] encryptedData)
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;

        using var memoryStream = new MemoryStream(encryptedData);
        using var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
        using var resultStream = new MemoryStream();
        await cryptoStream.CopyToAsync(resultStream);
        return resultStream.ToArray();
    }

    // DTO классы для сериализации
    private class ProfileDto
    {
        public Guid Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int BirthYear { get; set; }
    }

    private class TodoItemDto
    {
        public string Text { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime LastUpdate { get; set; }
    }
}