namespace KDC2Keybinder.Core.Services
{
	public interface IFolderPickerService
	{
		Task<string?> PickFolderAsync();
	}
}
