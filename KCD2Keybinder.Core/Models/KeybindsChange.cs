using KDC2Keybinder.Core.Models.DefaultProfile.ActionMaps;
using KDC2Keybinder.Core.Models.Superactions;

namespace KDC2Keybinder.Core
{
	public class KeybindsChange
	{
		public List<ModChange<Superaction>> Superactions { get; } = new();
		public List<ModChange<ActionMap>> ActionMaps { get; } = new();
	}

}
