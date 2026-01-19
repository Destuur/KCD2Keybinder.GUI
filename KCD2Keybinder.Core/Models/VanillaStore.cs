using KDC2Keybinder.Core.Models.DefaultProfile;
using KDC2Keybinder.Core.Models.DefaultProfile.ActionMaps;
using KDC2Keybinder.Core.Models.Superactions;

namespace KDC2Keybinder.Core.Models
{
	public sealed class VanillaStore
	{
		public Profile Profile { get; set; } = null!;
		public Keybinds Keybinds { get; set; } = null!;

		public Dictionary<string, Superaction> SuperactionsByName { get; init; } = [];
		public Dictionary<string, ActionMap> ActionMapsByName { get; init; } = [];
		public Dictionary<string, Conflict> ConflictsByName { get; init; } = [];
	}
}
