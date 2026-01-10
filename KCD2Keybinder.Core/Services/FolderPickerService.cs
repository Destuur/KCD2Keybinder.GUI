using Microsoft.WindowsAPICodePack.Dialogs;

namespace KDC2Keybinder.Core.Services
{
	public class FolderPickerService : IFolderPickerService
	{
		public Task<string?> PickFolderAsync()
		{
			var dialog = new CommonOpenFileDialog
			{
				IsFolderPicker = true
			};

			return Task.FromResult(dialog.ShowDialog() == CommonFileDialogResult.Ok ? dialog.FileName : null);
		}
	}
}
