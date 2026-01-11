using KDC2Keybinder.Core.Models.DefaultProfile;
using KDC2Keybinder.Core.Models.Superactions;

namespace KDC2Keybinder.Core
{
	public class ModData
	{
		public string ModId { get; set; } = string.Empty;
		public string ModPath { get; set; } = string.Empty;
		public Profile? Profile { get; set; }
		public Keybinds? Keybinds { get; set; }
	}
}
