using KDC2Keybinder.Core.Models.DefaultProfile;
using KDC2Keybinder.Core.Models.Superactions;

namespace KDC2Keybinder.Core
{
	public class ModMergeContext
	{
		public Profile BaseProfile { get; set; } = null!;
		public Keybinds BaseKeybinds { get; set; } = null!;

		public List<ModData> Mods { get; set; } = new();

		public Dictionary<string, KeybindsChange> ChangesPerMod { get; } = new();
	}

}
