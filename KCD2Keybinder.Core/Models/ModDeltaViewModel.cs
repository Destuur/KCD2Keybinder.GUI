using KDC2Keybinder.Core.Models.DefaultProfile.ActionMaps;
using KDC2Keybinder.Core.Models.Superactions;

namespace KDC2Keybinder.Core
{
	public class ModDeltaViewModel
	{
		public string ModId { get; set; } = "";
		public List<Superaction> ChangedSuperactions { get; set; } = [];
		public List<ActionMap> ChangedActionMaps { get; set; } = [];
	}
}
