using KDC2Keybinder.Core;
using KDC2Keybinder.Core.Services;
using KDC2Keybinder.Core.Utils;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
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
			if (e.ChangedButton == MouseButton.Left)
			{
				if (e.ClickCount == 2)
				{
					// Doppelklick: Maximieren oder wiederherstellen
					this.WindowState = this.WindowState == WindowState.Maximized
						? WindowState.Normal
						: WindowState.Maximized;
				}
				else
				{
					// Einfaches DragMove für Fenster verschieben
					this.DragMove();
				}
			}
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