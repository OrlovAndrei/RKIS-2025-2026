using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TodoList.Interfaces;
using TodoList.Security;
using TodoList.Exceptions;
using TodoList.Models;
using System.IO;

namespace TodoList.Data
{
    public class ApiDataStorage : IDataStorage
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://localhost:5000";

        public ApiDataStorage()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _httpClient.Timeout = TimeSpan.FromSeconds(10);
        }

        public void SaveProfiles(IEnumerable<Profile> profiles)
        {
            try
            {
                // Сериализуем в JSON
                string json = JsonSerializer.Serialize(profiles, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                // Шифруем
                byte[] encryptedData = EncryptData(json);

                // Отправляем на сервер
                var content = new ByteArrayContent(encryptedData);
                content.Headers.Add("Content-Type", "application/octet-stream");

                var response = _httpClient.PostAsync("/profiles", content).Result;
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new InvalidOperationException("Сервер недоступен или вернул ошибку", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка при сохранении профилей: {ex.Message}", ex);
            }
        }

        public IEnumerable<Profile> LoadProfiles()
        {
            try
            {
                // Получаем зашифрованные данные с сервера
                var response = _httpClient.GetAsync("/profiles").Result;
                
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new List<Profile>();
                }
                
                response.EnsureSuccessStatusCode();

                byte[] encryptedData = response.Content.ReadAsByteArrayAsync().Result;

                // Расшифровываем
                string json = DecryptData(encryptedData);

                // Десериализуем
                var profiles = JsonSerializer.Deserialize<List<Profile>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return profiles ?? new List<Profile>();
            }
            catch (HttpRequestException ex)
            {
                throw new InvalidOperationException("Сервер недоступен или вернул ошибку", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка при загрузке профилей: {ex.Message}", ex);
            }
        }

        public void SaveTodos(Guid userId, IEnumerable<TodoItem> todos)
        {
            try
            {
                // Сериализуем в JSON
                string json = JsonSerializer.Serialize(todos, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                // Шифруем
                byte[] encryptedData = EncryptData(json);

                // Отправляем на сервер
                var content = new ByteArrayContent(encryptedData);
                content.Headers.Add("Content-Type", "application/octet-stream");

                var response = _httpClient.PostAsync($"/todos/{userId}", content).Result;
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new InvalidOperationException("Сервер недоступен или вернул ошибку", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка при сохранении задач: {ex.Message}", ex);
            }
        }

        public IEnumerable<TodoItem> LoadTodos(Guid userId)
        {
            try
            {
                // Получаем зашифрованные данные с сервера
                var response = _httpClient.GetAsync($"/todos/{userId}").Result;
                
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new List<TodoItem>();
                }
                
                response.EnsureSuccessStatusCode();

                byte[] encryptedData = response.Content.ReadAsByteArrayAsync().Result;

                // Расшифровываем
                string json = DecryptData(encryptedData);

                // Десериализуем
                var todos = JsonSerializer.Deserialize<List<TodoItem>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return todos ?? new List<TodoItem>();
            }
            catch (HttpRequestException ex)
            {
                throw new InvalidOperationException("Сервер недоступен или вернул ошибку", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка при загрузке задач: {ex.Message}", ex);
            }
        }

        private byte[] EncryptData(string data)
        {
            using var memoryStream = new MemoryStream();
            using (var cryptoStream = EncryptionHelper.CreateEncryptStream(memoryStream))
            using (var writer = new StreamWriter(cryptoStream, Encoding.UTF8))
            {
                writer.Write(data);
                writer.Flush();
                cryptoStream.Flush();
            }
            return memoryStream.ToArray();
        }

        private string DecryptData(byte[] encryptedData)
        {
            using var memoryStream = new MemoryStream(encryptedData);
            using var cryptoStream = EncryptionHelper.CreateDecryptStream(memoryStream);
            using var reader = new StreamReader(cryptoStream, Encoding.UTF8);
            return reader.ReadToEnd();
        }
    }
}