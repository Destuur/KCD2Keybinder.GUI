using KDC2Keybinder.Core.Models.DefaultProfile.ActionMaps;
using KDC2Keybinder.Core.Models.Superactions;

namespace KDC2Keybinder.Core.Services
{
	public class ModMergeService
	{
		private readonly ModMergeState state;

		public ModMergeService(ModMergeState state)
		{
			this.state = state;
		}

		public void Initialize(ModMergeContext context)
		{
			state.Context.BaseProfile = context.BaseProfile;
			state.Context.BaseKeybinds = context.BaseKeybinds;
			state.Context.Mods = context.Mods;

			// 1️⃣ Core-Vergleich
			ModComparer.CompareModToBase(context);

			// 2️⃣ UI-State aufbauen (PUNKT 5)
			BuildUiState(context);
		}

		private void BuildUiState(ModMergeContext context)
		{
			state.ModSelections.Clear();
			state.SuperactionChanges.Clear();
			state.ActionMapChanges.Clear();

			foreach (var (modId, changes) in context.ChangesPerMod)
			{
				state.ModSelections.Add(new ModSelection
				{
					ModId = modId,
					IsEnabled = true
				});

				state.SuperactionChanges[modId] =
					changes.Superactions.Select(c => new ChangeViewModel<Superaction>
					{
						ModId = modId,
						Name = c.Modified.Name,
						BaseValue = c.Original,
						ModValue = c.Modified,
						IsSelected = true
					}).ToList();

				state.ActionMapChanges[modId] =
					changes.ActionMaps.Select(c => new ChangeViewModel<ActionMap>
					{
						ModId = modId,
						Name = c.Modified.Name,
						BaseValue = c.Original,
						ModValue = c.Modified,
						IsSelected = true
					}).ToList();
			}
		}
	}
}
