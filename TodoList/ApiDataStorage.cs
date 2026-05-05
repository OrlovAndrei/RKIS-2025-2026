using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using TodoList.Models;

namespace TodoList;

public class ApiDataStorage : IDataStorage
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    private static readonly byte[] Key = Encoding.UTF8.GetBytes("0123456789abcdef0123456789abcdef");
    private static readonly byte[] IV = Encoding.UTF8.GetBytes("abcdefghijklmnop");

    public ApiDataStorage(string baseUrl = "http://localhost:5000/")
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            Converters = { new JsonStringEnumConverter() }
        };
    }

    public void SaveProfiles(IEnumerable<Profile> profiles) => SaveProfilesAsync(profiles).Wait();
    public IEnumerable<Profile> LoadProfiles() => LoadProfilesAsync().Result;
    public void SaveTodos(Guid userId, IEnumerable<TodoItem> todos) => SaveTodosAsync(userId, todos).Wait();
    public IEnumerable<TodoItem> LoadTodos(Guid userId) => LoadTodosAsync(userId).Result;

    private async Task SaveProfilesAsync(IEnumerable<Profile> profiles)
    {
        string json = JsonSerializer.Serialize(profiles, _jsonOptions);
        byte[] plain = Encoding.UTF8.GetBytes(json);
        byte[] encrypted = await EncryptAsync(plain);

        using var content = new ByteArrayContent(encrypted);
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
        var response = await _httpClient.PostAsync("profiles", content);
        response.EnsureSuccessStatusCode();
    }

    private async Task<IEnumerable<Profile>> LoadProfilesAsync()
    {
        var response = await _httpClient.GetAsync("profiles");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return new List<Profile>();

        response.EnsureSuccessStatusCode();
        byte[] encrypted = await response.Content.ReadAsByteArrayAsync();
        byte[] plain = await DecryptAsync(encrypted);
        string json = Encoding.UTF8.GetString(plain);
        return JsonSerializer.Deserialize<List<Profile>>(json, _jsonOptions) ?? new List<Profile>();
    }

    private async Task SaveTodosAsync(Guid userId, IEnumerable<TodoItem> todos)
    {
        string json = JsonSerializer.Serialize(todos, _jsonOptions);
        byte[] plain = Encoding.UTF8.GetBytes(json);
        byte[] encrypted = await EncryptAsync(plain);

        using var content = new ByteArrayContent(encrypted);
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
        var response = await _httpClient.PostAsync($"todos/{userId}", content);
        response.EnsureSuccessStatusCode();
    }

    private async Task<IEnumerable<TodoItem>> LoadTodosAsync(Guid userId)
    {
        var response = await _httpClient.GetAsync($"todos/{userId}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return new List<TodoItem>();

        response.EnsureSuccessStatusCode();
        byte[] encrypted = await response.Content.ReadAsByteArrayAsync();
        byte[] plain = await DecryptAsync(encrypted);
        string json = Encoding.UTF8.GetString(plain);
        return JsonSerializer.Deserialize<List<TodoItem>>(json, _jsonOptions) ?? new List<TodoItem>();
    }

    private static async Task<byte[]> EncryptAsync(byte[] data)
    {
        using var aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;

        using var ms = new MemoryStream();
        using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            await cs.WriteAsync(data, 0, data.Length);
        return ms.ToArray();
    }

    private static async Task<byte[]> DecryptAsync(byte[] data)
    {
        using var aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;

        using var ms = new MemoryStream(data);
        using var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
        using var result = new MemoryStream();
        await cs.CopyToAsync(result);
        return result.ToArray();
    }
}