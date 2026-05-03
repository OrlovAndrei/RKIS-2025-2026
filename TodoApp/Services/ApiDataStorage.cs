using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TodoApp.Exceptions;
using TodoApp.Models;

namespace TodoApp.Services
{
    public class ApiDataStorage : IDataStorage
    {
        private static readonly byte[] Key =
        {
            0x25, 0x7A, 0x2D, 0xA8, 0x31, 0x6C, 0x99, 0xE0,
            0x4B, 0x12, 0xF4, 0x70, 0xC8, 0x55, 0x91, 0x3D,
            0xA1, 0x0E, 0xB7, 0x62, 0x19, 0xDC, 0x43, 0x8A,
            0xF0, 0x36, 0x5E, 0xC1, 0x77, 0x2B, 0x94, 0x09
        };

        private static readonly byte[] IV =
        {
            0xA4, 0x19, 0xC3, 0x5D, 0x6E, 0x80, 0x2F, 0xB1,
            0x47, 0xD9, 0x03, 0xEA, 0x7C, 0x58, 0x92, 0x10
        };

        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public ApiDataStorage(string baseUrl = "http://localhost:5000/")
        {
            _baseUrl = baseUrl.EndsWith("/") ? baseUrl : baseUrl + "/";
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_baseUrl),
                Timeout = TimeSpan.FromSeconds(5)
            };
        }

        public bool IsServerAvailable()
        {
            try
            {
                var response = _httpClient.GetAsync("profiles").GetAwaiter().GetResult();
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                return false;
            }
            catch (TaskCanceledException)
            {
                return false;
            }
        }

        public void SaveProfiles(IEnumerable<Profile> profiles)
        {
            byte[] encrypted = EncryptJson(profiles.Select(ProfileDto.FromProfile).ToList());
            PostBytes("profiles", encrypted, "Не удалось отправить профили на сервер.");
        }

        public IEnumerable<Profile> LoadProfiles()
        {
            byte[] encrypted = GetBytes("profiles", "Не удалось получить профили с сервера.");
            if (encrypted.Length == 0)
            {
                return new List<Profile>();
            }

            return DecryptJson<List<ProfileDto>>(encrypted)
                .Select(dto => dto.ToProfile())
                .ToList();
        }

        public void SaveTodos(Guid userId, IEnumerable<TodoItem> todos)
        {
            byte[] encrypted = EncryptJson(todos.Select(TodoItemDto.FromTodoItem).ToList());
            PostBytes($"todos/{userId}", encrypted, "Не удалось отправить задачи на сервер.");
        }

        public IEnumerable<TodoItem> LoadTodos(Guid userId)
        {
            byte[] encrypted = GetBytes($"todos/{userId}", "Не удалось получить задачи с сервера.");
            if (encrypted.Length == 0)
            {
                return new List<TodoItem>();
            }

            return DecryptJson<List<TodoItemDto>>(encrypted)
                .Select(dto => dto.ToTodoItem())
                .ToList();
        }

        private void PostBytes(string relativeUrl, byte[] bytes, string errorMessage)
        {
            try
            {
                using var content = new ByteArrayContent(bytes);
                var response = _httpClient.PostAsync(relativeUrl, content).GetAwaiter().GetResult();
                if (!response.IsSuccessStatusCode)
                {
                    throw new DataStorageException($"{errorMessage} Код ответа: {(int)response.StatusCode}.");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new DataStorageException("Сервер недоступен.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new DataStorageException("Сервер не ответил вовремя.", ex);
            }
        }

        private byte[] GetBytes(string relativeUrl, string errorMessage)
        {
            try
            {
                var response = _httpClient.GetAsync(relativeUrl).GetAwaiter().GetResult();
                if (!response.IsSuccessStatusCode)
                {
                    throw new DataStorageException($"{errorMessage} Код ответа: {(int)response.StatusCode}.");
                }

                return response.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
            }
            catch (HttpRequestException ex)
            {
                throw new DataStorageException("Сервер недоступен.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new DataStorageException("Сервер не ответил вовремя.", ex);
            }
        }

        private byte[] EncryptJson<T>(T value)
        {
            try
            {
                string json = JsonSerializer.Serialize(value);
                byte[] jsonBytes = Encoding.UTF8.GetBytes(json);

                using var output = new MemoryStream();
                using (var aes = CreateAes())
                using (var cryptoStream = new CryptoStream(output, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(jsonBytes, 0, jsonBytes.Length);
                }

                return output.ToArray();
            }
            catch (CryptographicException ex)
            {
                throw new DataStorageException("Ошибка шифрования данных для сервера.", ex);
            }
        }

        private T DecryptJson<T>(byte[] encrypted)
        {
            try
            {
                using var input = new MemoryStream(encrypted);
                using var aes = CreateAes();
                using var cryptoStream = new CryptoStream(input, aes.CreateDecryptor(), CryptoStreamMode.Read);
                using var reader = new StreamReader(cryptoStream, Encoding.UTF8);

                string json = reader.ReadToEnd();
                return JsonSerializer.Deserialize<T>(json)
                    ?? throw new DataStorageException("Сервер вернул пустые или повреждённые данные.");
            }
            catch (JsonException ex)
            {
                throw new DataStorageException("Не удалось разобрать JSON с сервера.", ex);
            }
            catch (CryptographicException ex)
            {
                throw new DataStorageException("Не удалось расшифровать данные с сервера.", ex);
            }
        }

        private Aes CreateAes()
        {
            var aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;
            return aes;
        }

        private class ProfileDto
        {
            public Guid Id { get; set; }
            public string Login { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public int BirthYear { get; set; }

            public static ProfileDto FromProfile(Profile profile)
            {
                return new ProfileDto
                {
                    Id = profile.Id,
                    Login = profile.Login,
                    Password = profile.Password,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    BirthYear = profile.BirthYear
                };
            }

            public Profile ToProfile()
            {
                return new Profile
                {
                    Id = Id,
                    Login = Login,
                    Password = Password,
                    FirstName = FirstName,
                    LastName = LastName,
                    BirthYear = BirthYear
                };
            }
        }

        private class TodoItemDto
        {
            public string Text { get; set; } = string.Empty;
            public TodoStatus Status { get; set; }
            public DateTime LastUpdate { get; set; }

            public static TodoItemDto FromTodoItem(TodoItem item)
            {
                return new TodoItemDto
                {
                    Text = item.Text,
                    Status = item.Status,
                    LastUpdate = item.LastUpdate
                };
            }

            public TodoItem ToTodoItem()
            {
                return new TodoItem(Text)
                {
                    Status = Status,
                    LastUpdate = LastUpdate
                };
            }
        }
    }
}
