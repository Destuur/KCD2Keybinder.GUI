using KDC2Keybinder.Core;
using KDC2Keybinder.Core.Services;
using KDC2Keybinder.Core.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace KCD2Keybinder.GUI
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			var services = new ServiceCollection();
			services.AddWpfBlazorWebView();
			services.AddMudServices();

			services.AddSingleton<IFolderPickerService, FolderPickerService>();
			services.AddSingleton<IUserSettingsService, UserSettingsService>();
			services.AddSingleton<IPathProvider, UserPathProvider>();
			services.AddSingleton<ModKeybindManager>();
			services.AddSingleton<ModService>();

			var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			var logPath = Path.Combine(appData, "KCD2Keybinder", "KeybindSuperactions.log");

			services.AddLogging(builder =>
			{
				builder.ClearProviders();
				builder.AddProvider(new FileLoggerProvider(logPath));
				builder.SetMinimumLevel(LogLevel.Information);
			});
#if DEBUG
			services.AddBlazorWebViewDeveloperTools();
#endif

			var serviceProvider = services.BuildServiceProvider();
			Resources.Add("services", serviceProvider);

			InitializeComponent();
		}

		#region WPF Window Methods
		private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton != MouseButton.Left)
				return;

			if (WindowState == WindowState.Maximized)
			{
				Point mouseScreen = PointToScreen(e.GetPosition(this));
				Rect restoreBounds = RestoreBounds;

				WindowState = WindowState.Normal;

				Left = mouseScreen.X - restoreBounds.Width / 2;
				Top = mouseScreen.Y - 10;
			}

			DragMove();
			WindowState = WindowState.Maximized;
		}

		private void Minimize_Click(object sender, RoutedEventArgs e)
		{
			this.WindowState = WindowState.Minimized;
		}

		private void MaximizeRestore_Click(object sender, RoutedEventArgs e)
		{
			this.WindowState = this.WindowState == WindowState.Maximized
				? WindowState.Normal
				: WindowState.Maximized;
		}

		private void Close_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		#endregion
	}
}