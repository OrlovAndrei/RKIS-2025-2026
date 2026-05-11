using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace TodoApp.Tests;

public class FileManagerTests
{
	private readonly byte[] _testKey;
	private readonly byte[] _testIV;
	private readonly string _testProfilesPath;
	private readonly string _testTodosDir;

	public FileManagerTests()
	{
		// Генерируем тестовый ключ и IV (по 16 байт для AES-128)
		_testKey = Encoding.UTF8.GetBytes("1234567890123456");
		_testIV = Encoding.UTF8.GetBytes("1234567890123456");

		// Создаём временные пути для тестов
		_testProfilesPath = Path.Combine(Path.GetTempPath(), $"test_profiles_{Guid.NewGuid()}.json");
		_testTodosDir = Path.Combine(Path.GetTempPath(), $"test_todos_{Guid.NewGuid()}");
	}

	[Fact]
	public void Constructor_WithValidParameters_CreatesDirectory()
	{
		// Arrange & Act
		var fileManager = new FileManager(_testProfilesPath, _testTodosDir, _testKey, _testIV);

		// Assert
		Assert.True(Directory.Exists(_testTodosDir));

		// Cleanup
		Directory.Delete(_testTodosDir, true);
	}

	[Fact]
	public void Constructor_WithNullProfilesPath_ThrowsArgumentNullException()
	{
		// Arrange, Act & Assert
		Assert.Throws<ArgumentNullException>(() =>
			new FileManager(null, _testTodosDir, _testKey, _testIV));
	}

	[Fact]
	public void Constructor_WithNullTodosDirectory_ThrowsArgumentNullException()
	{
		// Arrange, Act & Assert
		Assert.Throws<ArgumentNullException>(() =>
			new FileManager(_testProfilesPath, null, _testKey, _testIV));
	}

	[Fact]
	public void SaveAndLoadProfiles_RoundTrip_PreservesData()
	{
		// Arrange
		var fileManager = new FileManager(_testProfilesPath, _testTodosDir, _testKey, _testIV);
		var profile1 = new Profile("user1", "pass1", "Иван", "Иванов", 1990);
		var profile2 = new Profile("user2", "pass2", "Петр", "Петров", 1995);
		var profiles = new List<Profile> { profile1, profile2 };

		try
		{
			// Act
			fileManager.SaveProfiles(profiles);
			var loadedProfiles = fileManager.LoadProfiles().ToList();

			// Assert
			Assert.Equal(2, loadedProfiles.Count);

			// Проверяем первый профиль
			Assert.Equal(profile1.Id, loadedProfiles[0].Id);
			Assert.Equal(profile1.Login, loadedProfiles[0].Login);
			Assert.Equal(profile1.Password, loadedProfiles[0].Password);
			Assert.Equal(profile1.FirstName, loadedProfiles[0].FirstName);
			Assert.Equal(profile1.LastName, loadedProfiles[0].LastName);
			Assert.Equal(profile1.BirthYear, loadedProfiles[0].BirthYear);

			// Проверяем второй профиль
			Assert.Equal(profile2.Id, loadedProfiles[1].Id);
			Assert.Equal(profile2.Login, loadedProfiles[1].Login);
			Assert.Equal(profile2.Password, loadedProfiles[1].Password);
			Assert.Equal(profile2.FirstName, loadedProfiles[1].FirstName);
			Assert.Equal(profile2.LastName, loadedProfiles[1].LastName);
			Assert.Equal(profile2.BirthYear, loadedProfiles[1].BirthYear);
		}
		finally
		{
			// Cleanup
			if (File.Exists(_testProfilesPath))
				File.Delete(_testProfilesPath);
			if (Directory.Exists(_testTodosDir))
				Directory.Delete(_testTodosDir, true);
		}
	}

	[Fact]
	public void LoadProfiles_WhenFileDoesNotExist_ReturnsEmptyList()
	{
		// Arrange
		var fileManager = new FileManager(_testProfilesPath, _testTodosDir, _testKey, _testIV);

		try
		{
			// Act
			var profiles = fileManager.LoadProfiles();

			// Assert
			Assert.Empty(profiles);
		}
		finally
		{
			// Cleanup
			if (Directory.Exists(_testTodosDir))
				Directory.Delete(_testTodosDir, true);
		}
	}

	[Fact]
	public void SaveProfiles_WithNullParameter_DoesNotThrowException()
	{
		// Arrange
		var fileManager = new FileManager(_testProfilesPath, _testTodosDir, _testKey, _testIV);

		try
		{
			// Act
			var exception = Record.Exception(() => fileManager.SaveProfiles(null));

			// Assert
			Assert.Null(exception);
			Assert.False(File.Exists(_testProfilesPath));
		}
		finally
		{
			// Cleanup
			if (Directory.Exists(_testTodosDir))
				Directory.Delete(_testTodosDir, true);
		}
	}

	[Fact]
	public void SaveProfiles_WithEmptyList_SavesEmptyFile()
	{
		// Arrange
		var fileManager = new FileManager(_testProfilesPath, _testTodosDir, _testKey, _testIV);
		var emptyProfiles = new List<Profile>();

		try
		{
			// Act
			fileManager.SaveProfiles(emptyProfiles);
			var loadedProfiles = fileManager.LoadProfiles();

			// Assert
			Assert.Empty(loadedProfiles);
		}
		finally
		{
			// Cleanup
			if (File.Exists(_testProfilesPath))
				File.Delete(_testProfilesPath);
			if (Directory.Exists(_testTodosDir))
				Directory.Delete(_testTodosDir, true);
		}
	}

	[Fact]
	public void SaveAndLoadTodos_RoundTrip_PreservesData()
	{
		// Arrange
		var fixedTime = new DateTime(2026, 4, 9, 12, 0, 0);
		var clockMock = new Mock<IClock>();
		clockMock.Setup(c => c.Now).Returns(fixedTime);

		var fileManager = new FileManager(_testProfilesPath, _testTodosDir, _testKey, _testIV);
		var userId = Guid.NewGuid();
		var todo1 = new TodoItem("Помыть посуду", clockMock.Object);
		var todo2 = new TodoItem("Сделать уроки", clockMock.Object);
		todo1.MarkDone();

		var todos = new List<TodoItem> { todo1, todo2 };

		try
		{
			// Act
			fileManager.SaveTodos(userId, todos);
			var loadedTodos = fileManager.LoadTodos(userId).ToList();

			// Assert
			Assert.Equal(2, loadedTodos.Count);

			Assert.Equal("Помыть посуду", loadedTodos[0].GetText());
			Assert.True(loadedTodos[0].GetIsDone());
			Assert.Equal(TodoStatus.Completed, loadedTodos[0].GetStatus());

			Assert.Equal("Сделать уроки", loadedTodos[1].GetText());
			Assert.False(loadedTodos[1].GetIsDone());
			Assert.Equal(TodoStatus.NotStarted, loadedTodos[1].GetStatus());
		}
		finally
		{
			// Cleanup
			if (File.Exists(_testProfilesPath))
				File.Delete(_testProfilesPath);
			if (Directory.Exists(_testTodosDir))
				Directory.Delete(_testTodosDir, true);
		}
	}

	[Fact]
	public void LoadTodos_WhenFileDoesNotExist_ReturnsEmptyList()
	{
		// Arrange
		var fileManager = new FileManager(_testProfilesPath, _testTodosDir, _testKey, _testIV);
		var userId = Guid.NewGuid();

		try
		{
			// Act
			var todos = fileManager.LoadTodos(userId);

			// Assert
			Assert.Empty(todos);
		}
		finally
		{
			// Cleanup
			if (Directory.Exists(_testTodosDir))
				Directory.Delete(_testTodosDir, true);
		}
	}

	[Fact]
	public void SaveTodos_WithNullParameter_DoesNotThrowException()
	{
		// Arrange
		var fileManager = new FileManager(_testProfilesPath, _testTodosDir, _testKey, _testIV);
		var userId = Guid.NewGuid();

		try
		{
			// Act
			var exception = Record.Exception(() => fileManager.SaveTodos(userId, null));

			// Assert
			Assert.Null(exception);
		}
		finally
		{
			// Cleanup
			if (Directory.Exists(_testTodosDir))
				Directory.Delete(_testTodosDir, true);
		}
	}

	[Fact]
	public void SaveTodos_ForDifferentUsers_CreatesSeparateFiles()
	{
		// Arrange
		var fixedTime = new DateTime(2026, 4, 9, 12, 0, 0);
		var clockMock = new Mock<IClock>();
		clockMock.Setup(c => c.Now).Returns(fixedTime);

		var fileManager = new FileManager(_testProfilesPath, _testTodosDir, _testKey, _testIV);
		var user1Id = Guid.NewGuid();
		var user2Id = Guid.NewGuid();

		var user1Todos = new List<TodoItem> { new TodoItem("Задача пользователя 1", clockMock.Object) };
		var user2Todos = new List<TodoItem> { new TodoItem("Задача пользователя 2", clockMock.Object) };

		try
		{
			// Act
			fileManager.SaveTodos(user1Id, user1Todos);
			fileManager.SaveTodos(user2Id, user2Todos);

			var loadedUser1Todos = fileManager.LoadTodos(user1Id).ToList();
			var loadedUser2Todos = fileManager.LoadTodos(user2Id).ToList();

			// Assert
			Assert.Single(loadedUser1Todos);
			Assert.Equal("Задача пользователя 1", loadedUser1Todos[0].GetText());

			Assert.Single(loadedUser2Todos);
			Assert.Equal("Задача пользователя 2", loadedUser2Todos[0].GetText());
		}
		finally
		{
			// Cleanup
			if (File.Exists(_testProfilesPath))
				File.Delete(_testProfilesPath);
			if (Directory.Exists(_testTodosDir))
				Directory.Delete(_testTodosDir, true);
		}
	}
}