namespace KDC2Keybinder.Core.Models
{
	public class KeybindsDescription
	{
		public List<UiGroup> UIGroups { get; set; } = [];
		public List<Superaction> Superactions { get; set; } = [];
		public List<Conflict> Conflicts { get; set; } = [];
	}
}
