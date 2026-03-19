using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Moq;
using TodoList;
using TodoList.Data;
using TodoList.Interfaces;
using TodoList.Security;

namespace TodoList.Tests
{
	public class FileStorageTests
	{
		private readonly Mock<IFileSystem> _fileSystemMock;
		private readonly string _baseDirectory = "test_data";
		private readonly byte[] _encryptedData;
		private readonly List<Profile> _testProfiles;
		private readonly List<TodoItem> _testTodos;
		private readonly Guid _testUserId;

		public FileStorageTests()
		{
			_fileSystemMock = new Mock<IFileSystem>();
			_testUserId = Guid.NewGuid();

			// Подготовка тестовых данных
			_testProfiles = new List<Profile>
			{
				new Profile(Guid.NewGuid(), "user1", "pass1", "Иван", "Иванов", 2000),
				new Profile(Guid.NewGuid(), "user2", "pass2", "Петр", "Петров", 1995)
			};

			var clockMock = new Mock<IClock>();
			clockMock.Setup(c => c.Now).Returns(new DateTime(2025, 3, 19));

			_testTodos = new List<TodoItem>
			{
				new TodoItem("Задача 1", clockMock.Object),
				new TodoItem("Задача 2", clockMock.Object)
			};

			// Подготовка зашифрованных данных для мока
			_encryptedData = CreateEncryptedData("test data");
		}

		private byte[] CreateEncryptedData(string content)
		{
			using var ms = new MemoryStream();
			using (var cryptoStream = EncryptionHelper.CreateEncryptStream(ms))
			using (var writer = new StreamWriter(cryptoStream, Encoding.UTF8))
			{
				writer.Write(content);
				writer.Flush();
				cryptoStream.Flush();
			}
			return ms.ToArray();
		}

		[Fact]
		public void Constructor_CreatesDirectory()
		{
			// Act
			var storage = new FileStorage(_baseDirectory, _fileSystemMock.Object);

			// Assert
			_fileSystemMock.Verify(fs => fs.CreateDirectory(_baseDirectory), Times.Once);
		}

		[Fact]
		public void SaveProfiles_ValidData_WritesEncryptedFile()
		{
			// Arrange
			var storage = new FileStorage(_baseDirectory, _fileSystemMock.Object);

			// Act
			storage.SaveProfiles(_testProfiles);

			// Assert
			_fileSystemMock.Verify(
				fs => fs.WriteAllBytes(
					It.Is<string>(path => path.EndsWith("profiles.dat")),
					It.IsAny<byte[]>()
				),
				Times.Once
			);
		}

		[Fact]
		public void LoadProfiles_FileExists_ReturnsProfiles()
		{
			// Arrange
			var storage = new FileStorage(_baseDirectory, _fileSystemMock.Object);

			// Сохраняем данные через реальный storage, чтобы получить корректный encryptedData
			// Но в тесте мы будем мокать чтение
			using var ms = new MemoryStream();
			using (var cryptoStream = EncryptionHelper.CreateEncryptStream(ms))
			using (var writer = new StreamWriter(cryptoStream, Encoding.UTF8))
			{
				writer.WriteLine("Id;Login;Password;FirstName;LastName;BirthYear");
				foreach (var p in _testProfiles)
				{
					writer.WriteLine($"{p.Id};{p.Login};{p.Password};{p.FirstName};{p.LastName};{p.BirthYear}");
				}
				writer.Flush();
				cryptoStream.Flush();
			}
			byte[] data = ms.ToArray();

			_fileSystemMock.Setup(fs => fs.FileExists(It.IsAny<string>())).Returns(true);
			_fileSystemMock.Setup(fs => fs.ReadAllBytes(It.IsAny<string>())).Returns(data);

			// Act
			var result = storage.LoadProfiles().ToList();

			// Assert
			Assert.Equal(2, result.Count);
			Assert.Equal(_testProfiles[0].Login, result[0].Login);
			Assert.Equal(_testProfiles[1].Login, result[1].Login);
		}

		[Fact]
		public void LoadProfiles_FileDoesNotExist_ReturnsEmptyList()
		{
			// Arrange
			var storage = new FileStorage(_baseDirectory, _fileSystemMock.Object);
			_fileSystemMock.Setup(fs => fs.FileExists(It.IsAny<string>())).Returns(false);

			// Act
			var result = storage.LoadProfiles();

			// Assert
			Assert.Empty(result);
		}

		[Fact]
		public void SaveTodos_ValidData_WritesEncryptedFile()
		{
			// Arrange
			var storage = new FileStorage(_baseDirectory, _fileSystemMock.Object);

			// Act
			storage.SaveTodos(_testUserId, _testTodos);

			// Assert
			_fileSystemMock.Verify(
				fs => fs.WriteAllBytes(
					It.Is<string>(path => path.Contains(_testUserId.ToString())),
					It.IsAny<byte[]>()
				),
				Times.Once
			);
		}

		[Fact]
		public void LoadTodos_FileExists_ReturnsTodos()
		{
			// Arrange
			var storage = new FileStorage(_baseDirectory, _fileSystemMock.Object);

			// Подготовка зашифрованных данных с задачами
			using var ms = new MemoryStream();
			using (var cryptoStream = EncryptionHelper.CreateEncryptStream(ms))
			using (var writer = new StreamWriter(cryptoStream, Encoding.UTF8))
			{
				writer.WriteLine("Index;Text;Status;LastUpdate");
				int index = 0;
				foreach (var todo in _testTodos)
				{
					string textEscaped = todo.Text.Replace("\n", "\\n").Replace("\"", "\"\"");
					writer.WriteLine($"{index};\"{textEscaped}\";{todo.Status};{todo.LastUpdate:O}");
					index++;
				}
				writer.Flush();
				cryptoStream.Flush();
			}
			byte[] data = ms.ToArray();

			_fileSystemMock.Setup(fs => fs.FileExists(It.IsAny<string>())).Returns(true);
			_fileSystemMock.Setup(fs => fs.ReadAllBytes(It.IsAny<string>())).Returns(data);

			// Act
			var result = storage.LoadTodos(_testUserId).ToList();

			// Assert
			Assert.Equal(2, result.Count);
			Assert.Equal(_testTodos[0].Text, result[0].Text);
			Assert.Equal(_testTodos[1].Text, result[1].Text);
		}

		[Fact]
		public void LoadTodos_FileDoesNotExist_ReturnsEmptyList()
		{
			// Arrange
			var storage = new FileStorage(_baseDirectory, _fileSystemMock.Object);
			_fileSystemMock.Setup(fs => fs.FileExists(It.IsAny<string>())).Returns(false);

			// Act
			var result = storage.LoadTodos(_testUserId);

			// Assert
			Assert.Empty(result);
		}

		[Fact]
		public void LoadTodos_CorruptedFile_ThrowsInvalidDataException()
		{
			// Arrange
			var storage = new FileStorage(_baseDirectory, _fileSystemMock.Object);

			// Поврежденные данные (некорректный шифрованный формат)
			byte[] corruptedData = new byte[] { 0x00, 0x01, 0x02, 0x03 };

			_fileSystemMock.Setup(fs => fs.FileExists(It.IsAny<string>())).Returns(true);
			_fileSystemMock.Setup(fs => fs.ReadAllBytes(It.IsAny<string>())).Returns(corruptedData);

			// Act & Assert
			Assert.Throws<InvalidDataException>(() => storage.LoadTodos(_testUserId));
		}
	}
}