using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Todolist
{
    public class ApiDataStorage : IDataStorage, IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly byte[] _key;
        private readonly byte[] _iv;
        private bool _disposed;

        public ApiDataStorage(string baseUrl = "http://localhost:5000/")
        {
            _baseUrl = baseUrl;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_baseUrl),
                Timeout = TimeSpan.FromSeconds(30)
            };

            _key = Encoding.UTF8.GetBytes("0123456789ABCDEF0123456789ABCDEF");
            _iv = Encoding.UTF8.GetBytes("0123456789ABCDEF");
        }

        private byte[] EncryptData(byte[] data)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _iv;

                using (var ms = new MemoryStream())
                using (var cryptoStream = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(data, 0, data.Length);
                    cryptoStream.FlushFinalBlock();
                    return ms.ToArray();
                }
            }
        }

        private byte[] DecryptData(byte[] encryptedData)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _iv;

                using (var ms = new MemoryStream(encryptedData))
                using (var cryptoStream = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                using (var resultMs = new MemoryStream())
                {
                    cryptoStream.CopyTo(resultMs);
                    return resultMs.ToArray();
                }
            }
        }

        public void SaveProfiles(IEnumerable<Profile> profiles)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };
            string json = JsonSerializer.Serialize(profiles, options);
            byte[] jsonData = Encoding.UTF8.GetBytes(json);

            byte[] encryptedData = EncryptData(jsonData);

            var content = new ByteArrayContent(encryptedData);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            var response = _httpClient.PostAsync("/profiles", content).GetAwaiter().GetResult();
            response.EnsureSuccessStatusCode();
        }

        public IEnumerable<Profile> LoadProfiles()
        {
            var response = _httpClient.GetAsync("/profiles").GetAwaiter().GetResult();
            response.EnsureSuccessStatusCode();

            byte[] encryptedData = response.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();

            if (encryptedData == null || encryptedData.Length == 0)
            {
                return new List<Profile>();
            }

            byte[] decryptedData = DecryptData(encryptedData);

            string json = Encoding.UTF8.GetString(decryptedData);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<List<Profile>>(json, options) ?? new List<Profile>();
        }

        public void SaveTodos(Guid userId, IEnumerable<TodoItem> todos)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("ID пользователя не может быть пустым", nameof(userId));

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };
            string json = JsonSerializer.Serialize(todos, options);
            byte[] jsonData = Encoding.UTF8.GetBytes(json);

            byte[] encryptedData = EncryptData(jsonData);

            var content = new ByteArrayContent(encryptedData);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            var response = _httpClient.PostAsync($"/todos/{userId}", content).GetAwaiter().GetResult();
            response.EnsureSuccessStatusCode();
        }

        public IEnumerable<TodoItem> LoadTodos(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("ID пользователя не может быть пустым", nameof(userId));

            var response = _httpClient.GetAsync($"/todos/{userId}").GetAwaiter().GetResult();
            response.EnsureSuccessStatusCode();

            byte[] encryptedData = response.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();

            if (encryptedData == null || encryptedData.Length == 0)
            {
                return new List<TodoItem>();
            }

            byte[] decryptedData = DecryptData(encryptedData);

            string json = Encoding.UTF8.GetString(decryptedData);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<List<TodoItem>>(json, options) ?? new List<TodoItem>();
        }

        public async Task<bool> CheckServerAvailabilityAsync()
        {
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                var response = await _httpClient.GetAsync("/profiles", cts.Token);
                return response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.NotFound;
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _httpClient?.Dispose();
                _disposed = true;
            }
        }
    }
}