using System.Windows;
using TodoApp.Desktop.ViewModels;

namespace TodoApp.Desktop;

public partial class App : Application
{
	protected override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);

		var mainWindow = new MainWindow
		{
			DataContext = new MainViewModel()
		};

		mainWindow.Show();
	}
}
