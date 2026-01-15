using KDC2Keybinder.Core.Models.DefaultProfile;
using KDC2Keybinder.Core.Models.Superactions;

namespace KDC2Keybinder.Core
{
	public class ModData
	{
		public string Id { get; set; } = string.Empty;
		public string Path { get; set; } = string.Empty;

		public Profile? Profile { get; set; }
		public Keybinds? Keybinds { get; set; }
		public ModDelta? Delta { get; set; }
	}
}
