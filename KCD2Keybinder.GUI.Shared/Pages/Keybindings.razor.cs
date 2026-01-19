using KCD2Keybinder.GUI.Shared.Models;
using KDC2Keybinder.Core;
using KDC2Keybinder.Core.Models;
using KDC2Keybinder.Core.Models.Superactions;
using KDC2Keybinder.Core.Services;
using KDC2Keybinder.Core.Utils;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KCD2Keybinder.GUI.Shared.Pages
{
	public partial class Keybindings
	{
		private string? activeKey;
		private Superaction? selectedSuperaction;
		private KeyboardLayout keyboardLayout = KeyboardLayout.QWERTY;
		private MergeStore mergeStore = new();
		private List<ModDeltaViewModel> selectedMods = [];
		private bool canCreate;

		[Inject]
		public IFolderPickerService FolderPicker { get; set; } = null!;
		[Inject]
		public IUserSettingsService UserSettings { get; set; } = null!;
		[Inject]
		public ModKeybindManager KeybindManager { get; set; } = null!;
		[Inject]
		public ISnackbar Snackbar { get; set; } = null!;

		private async Task PickGameFolder()
		{
			var path = await FolderPicker.PickFolderAsync();
			if (string.IsNullOrWhiteSpace(path))
			{
				return;
			}
			if (path.Contains("KingdomComeDeliverance2") == false)
			{
				Snackbar.Add("Pick the KCD2 folder, you fool!", Severity.Warning);
				path = await FolderPicker.PickFolderAsync();
			}
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
			//if (path.Contains("KingdomComeDeliverance2") == false)
			//{
			//	Snackbar.Add("Pick the KCD2 folder, you fool!", Severity.Warning);
			//	path = await FolderPicker.PickFolderAsync();
			//}
			//if (string.IsNullOrWhiteSpace(path))
			//{
			//	return;
			//}

			UserSettings.Update(s => s.ModPath = path);
		}

		public void ApplyModChanges()
		{
			if (KeybindManager.MergedKeybindStore is null)
			{
				Snackbar.Add("Load Keybind Data first", Severity.Info);
				return;
			}

			foreach (var mod in selectedMods)
			{
				MergeMod(mod);
			}
			selectedMods.Clear();
			KeybindManager.ApplyAllToMergedStore(mergeStore);
			Snackbar.Add("Merging successful", Severity.Success);
			canCreate = true;
		}



		private void ToggleMod(ModDeltaViewModel mod)
		{
			var foundMod = selectedMods.FirstOrDefault(x => x.ModId == mod.ModId);

			if (foundMod is null)
			{
				selectedMods.Add(mod);
			}
			else
			{
				selectedMods.Remove(mod);
			}
		}

		public void BuildMod()
		{
			if (KeybindManager.MergedKeybindStore is null)
			{
				Snackbar.Add("Load Keybind Data first", Severity.Info);
				return;
			}

			KeybindManager.BuildMod();
			Snackbar.Add("Creating Mod successful", Severity.Success);
			canCreate = false;
		}

		public void MergeMod(ModDeltaViewModel delta)
		{
			foreach (var sa in delta.ChangedSuperactions)
			{
				mergeStore.AddSuperaction(sa.Name, delta.ModId, sa);
			}

			foreach (var am in delta.ChangedActionMaps)
			{
				mergeStore.AddActionMap(am.Name, delta.ModId, am);
			}

			foreach (var conflict in delta.ChangedConflicts)
			{
				mergeStore.AddConflict(conflict.Id, delta.ModId, conflict);
			}
		}

		private List<Superaction> GetKeybindSuperactions()
		{
			if (activeKey is null ||
				KeybindManager.MergedKeybindStore is null)
			{
				return [];
			}

			if (KeybindManager.MergedKeybindStore.Superactions is null)
			{
				return [];
			}

			var superactions = KeybindManager.MergedKeybindStore.Superactions;

			if (superactions is null)
			{
				return [];
			}

			return superactions.Where(superactions => superactions.Controls.Any(control => control.Input.ToLower() == activeKey.ToLower())).Select(x => x).ToList();
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
			if (string.IsNullOrEmpty(UserSettings.Current.GamePath) ||
				string.IsNullOrEmpty(UserSettings.Current.ModPath))
			{
				Snackbar.Add("Pick folders!", Severity.Info);
				return;
			}

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