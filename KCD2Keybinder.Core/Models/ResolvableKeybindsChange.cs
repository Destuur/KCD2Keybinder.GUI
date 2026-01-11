using KDC2Keybinder.Core.Models.DefaultProfile.ActionMaps;
using KDC2Keybinder.Core.Models.Superactions;

namespace KDC2Keybinder.Core
{
	public class ResolvableKeybindsChange
	{
		public List<ResolvableModChange<Superaction>> Superactions { get; set; } = new();
		public List<ResolvableModChange<ActionMap>> ActionMaps { get; set; } = new();
	}

}
