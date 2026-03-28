using TodoApp.Exceptions;
public class SyncCommand : ICommand
{
	public void Execute()
	{
		try
		{
			Console.WriteLine("Синхронизация с сервером...");
			AppInfo.SaveData();

			Console.WriteLine("Синхронизация завершена!");
		}
		catch (StorageException ex)
		{
			Console.WriteLine($"Ошибка синхронизации: {ex.Message}");
		}
	}
}