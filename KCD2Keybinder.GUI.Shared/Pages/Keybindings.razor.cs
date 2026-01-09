
using KCD2Keybinder.GUI.Shared.Components.Keyboard;
using KCD2Keybinder.GUI.Shared.Models;

namespace KCD2Keybinder.GUI.Shared.Pages
{
	public partial class Keybindings
	{
		private string? activeKey;
		private KeyboardLayout keyboardLayout = KeyboardLayout.QWERTY;

		private void OnActiveKeyChanged(string? key)
		{
			activeKey = key;
		}
	}
}