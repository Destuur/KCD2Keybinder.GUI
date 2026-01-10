using KCD2Keybinder.GUI.Shared.Models;
using KDC2Keybinder.Core.Models;
using KDC2Keybinder.Core.Models.Profile.ActionMaps;
using KDC2Keybinder.Core.Services;
using KDC2Keybinder.Core.Utils;

namespace KCD2Keybinder.GUI.Shared.Pages
{
	public partial class Keybindings
	{
		private string? activeKey;
		private KeyboardLayout keyboardLayout = KeyboardLayout.QWERTY;

		private Dictionary<string, ActionMap>? actionMaps;
		private Dictionary<string, Superaction>? superactions;
		private ActionMap? selectedActionMap;
		private Superaction? selectedSuperaction;

		protected override void OnInitialized()
		{
			var keybindService = new KeybindService(Resources.DataDirectory, Resources.ModsDirectory);
			keybindService.LoadVanillaPak(Resources.IPLGameData);
			actionMaps = keybindService.GetVanillaActionMaps();
			superactions = keybindService.GetVanillaSuperactions();
		}

		private List<Superaction> GetKeybindSuperactions()
		{
			if (activeKey is null)
			{
				return [];
			}

			if (superactions is null)
			{
				return [];
			}

			return superactions.Where(superactions => superactions.Value.Controls.Any(control => control.Input.ToLower() == activeKey.ToLower())).Select(x => x.Value).ToList();
		}

		private void SelectSuperaction(Superaction? superaction)
		{
			if (superaction is null)
			{
				return;
			}

			selectedSuperaction = superaction;
			StateHasChanged();
		}

		private void OnActiveKeyChanged(string? key)
		{
			activeKey = key;
			selectedActionMap = null;
			selectedSuperaction = null;
		}
	}
}