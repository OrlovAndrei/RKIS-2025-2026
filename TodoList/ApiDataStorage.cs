using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class ApiDataStorage : IDataStorage
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly byte[] _aesKey;
    private readonly byte[] _aesIV;

    public ApiDataStorage(string baseUrl = "http://localhost:5000/")
    {
        _baseUrl = baseUrl;
        _httpClient = new HttpClient();
        _httpClient.Timeout = TimeSpan.FromSeconds(30);
        
        _aesKey = Encoding.UTF8.GetBytes("12345678901234567890123456789012");
        _aesIV = Encoding.UTF8.GetBytes("1234567890123456");
    }

    public void SaveProfiles(IEnumerable<Profile> profiles)
    {
        var task = Task.Run(async () => await SaveProfilesAsync(profiles));
        task.Wait();
    }

    private async Task SaveProfilesAsync(IEnumerable<Profile> profiles)
    {
        string json = JsonSerializer.Serialize(profiles);

        byte[] encryptedData = Encrypt(json);

        var content = new ByteArrayContent(encryptedData);
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
        
        HttpResponseMessage response = await _httpClient.PostAsync($"{_baseUrl}profiles", content);
        response.EnsureSuccessStatusCode();
    }

    public IEnumerable<Profile> LoadProfiles()
    {
        var task = Task.Run(async () => await LoadProfilesAsync());
        return task.Result;
    }

    private async Task<IEnumerable<Profile>> LoadProfilesAsync()
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"{_baseUrl}profiles");
        
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return new List<Profile>();
        }
        
        response.EnsureSuccessStatusCode();

        byte[] encryptedData = await response.Content.ReadAsByteArrayAsync();
        
        if (encryptedData.Length == 0)
        {
            return new List<Profile>();
        }

        string json = Decrypt(encryptedData);
        
        return JsonSerializer.Deserialize<List<Profile>>(json) ?? new List<Profile>();
    }

    public void SaveTodos(Guid userId, IEnumerable<TodoItem> todos)
    {
        var task = Task.Run(async () => await SaveTodosAsync(userId, todos));
        task.Wait();
    }

    private async Task SaveTodosAsync(Guid userId, IEnumerable<TodoItem> todos)
    {
        string json = JsonSerializer.Serialize(todos);

        byte[] encryptedData = Encrypt(json);

        var content = new ByteArrayContent(encryptedData);
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
        
        HttpResponseMessage response = await _httpClient.PostAsync($"{_baseUrl}todos/{userId}", content);
        response.EnsureSuccessStatusCode();
    }

    public IEnumerable<TodoItem> LoadTodos(Guid userId)
    {
        var task = Task.Run(async () => await LoadTodosAsync(userId));
        return task.Result;
    }

    private async Task<IEnumerable<TodoItem>> LoadTodosAsync(Guid userId)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"{_baseUrl}todos/{userId}");
        
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return new List<TodoItem>();
        }
        
        response.EnsureSuccessStatusCode();

        byte[] encryptedData = await response.Content.ReadAsByteArrayAsync();
        
        if (encryptedData.Length == 0)
        {
            return new List<TodoItem>();
        }

        string json = Decrypt(encryptedData);

        return JsonSerializer.Deserialize<List<TodoItem>>(json) ?? new List<TodoItem>();
    }

    private byte[] Encrypt(string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = _aesKey;
        aes.IV = _aesIV;
        
        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
        
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
        cryptoStream.FlushFinalBlock();
        
        return memoryStream.ToArray();
    }

    private string Decrypt(byte[] cipherText)
    {
        using var aes = Aes.Create();
        aes.Key = _aesKey;
        aes.IV = _aesIV;
        
        using var memoryStream = new MemoryStream(cipherText);
        using var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
        using var reader = new StreamReader(cryptoStream, Encoding.UTF8);
        
        return reader.ReadToEnd();
    }
}