using KDC2Keybinder.Core.Models.DefaultProfile.ActionMaps;
using KDC2Keybinder.Core.Models.Superactions;

namespace KDC2Keybinder.Core
{
	public class ModMergeState
	{
		public ModMergeContext Context { get; } = new();

		public List<ModSelection> ModSelections { get; } = new();

		public Dictionary<string, List<ChangeViewModel<Superaction>>> SuperactionChanges { get; } = [];

		public Dictionary<string, List<ChangeViewModel<ActionMap>>> ActionMapChanges { get; } = [];
	}

}
