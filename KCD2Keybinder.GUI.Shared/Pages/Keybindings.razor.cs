using KCD2Keybinder.GUI.Shared.Models;
using KDC2Keybinder.Core;
using KDC2Keybinder.Core.Models.DefaultProfile.ActionMaps;
using KDC2Keybinder.Core.Models.Superactions;
using KDC2Keybinder.Core.Services;
using KDC2Keybinder.Core.Utils;
using Microsoft.AspNetCore.Components;

namespace KCD2Keybinder.GUI.Shared.Pages
{
	public partial class Keybindings
	{
		private string? activeKey;
		private Superaction? selectedSuperaction;
		private KeyboardLayout keyboardLayout = KeyboardLayout.QWERTY;

		[Inject]
		public IFolderPickerService FolderPicker { get; set; } = null!;
		[Inject]
		public IUserSettingsService UserSettings { get; set; } = null!;
		[Inject]
		public ModKeybindManager KeybindManager { get; set; } = null!;

		private async Task PickGameFolder()
		{
			var path = await FolderPicker.PickFolderAsync();
			if (string.IsNullOrWhiteSpace(path))
			{
				return;
			}

			UserSettings.Update(s => s.GamePath = path);
		}

		private async Task PickModFolder()
		{
			var path = await FolderPicker.PickFolderAsync();
			if (string.IsNullOrWhiteSpace(path))
			{
				return;
			}

			UserSettings.Update(s => s.ModPath = path);
		}

		private List<Superaction> GetKeybindSuperactions()
		{
			if (activeKey is null)
			{
				return [];
			}

			if (KeybindManager.BaseKeybinds is null)
			{
				return [];
			}

			var superactions = KeybindManager.BaseKeybinds.Superactions;

			if (superactions is null)
			{
				return [];
			}

			return superactions.Where(superactions => superactions.Controls.Any(control => control.Input.ToLower() == activeKey.ToLower())).Select(x => x).ToList();
		}

		private List<ModData> GetMods()
		{
			if (KeybindManager is null)
			{
				return [];
			}

			return KeybindManager.Mods;
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

		private void LoadData()
		{
			KeybindManager.LoadBaseGame(Resources.IPLGameData);
			if (!string.IsNullOrEmpty(UserSettings.Current.ModPath))
			{
				KeybindManager.LoadMods(UserSettings.Current.ModPath);
			}
		}

		private void OnActiveKeyChanged(string? key)
		{
			activeKey = key;
		}
	}
}