using System.Xml.Serialization;

namespace KDC2Keybinder.Core.Models.Superactions
{
	/// <summary>
	/// The root element for keybinds configuration, containing UI groups, superactions, and conflicts.
	/// </summary>
	[XmlRoot("keybinds")]
	public class Keybinds
	{
		public List<UiGroup> UIGroups { get; set; } = [];
		public List<Superaction> Superactions { get; set; } = [];
		public List<Conflict> Conflicts { get; set; } = [];
	}
}
