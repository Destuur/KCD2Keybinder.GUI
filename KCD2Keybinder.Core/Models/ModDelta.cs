using KDC2Keybinder.Core.Models.DefaultProfile.ActionMaps;
using KDC2Keybinder.Core.Models.Superactions;

namespace KDC2Keybinder.Core
{
	public sealed class ModDelta
	{
		public string ModId { get; init; } = string.Empty;

		public Dictionary<string, Superaction> Superactions { get; init; } = [];
		public Dictionary<string, ActionMap> ActionMaps { get; init; } = [];
	}
}
