using KDC2Keybinder.Core.Models.DefaultProfile.ActionMaps;
using KDC2Keybinder.Core.Models.DefaultProfile.Controller;
using KDC2Keybinder.Core.Models.DefaultProfile.Filter;
using KDC2Keybinder.Core.Models.DefaultProfile.Platform;
using KDC2Keybinder.Core.Models.DefaultProfile.Prios;
using System.Xml.Serialization;

namespace KDC2Keybinder.Core.Models.DefaultProfile
{
	/// <summary>
	/// The root element for the default profile configuration.
	/// </summary>
	[XmlRoot("profile")]
	public class Profile
	{
		[XmlElement("priorities")]
		public Priorities? Priorities { get; set; }
		[XmlElement("platforms")]
		public Platforms? Platforms { get; set; }
		[XmlElement("actionmap")]
		public List<ActionMap> ActionMaps { get; set; } = [];
		[XmlElement("actionfilter")]
		public List<ActionFilter> ActionFilter { get; set; } = [];
		[XmlElement("controllerlayouts")]
		public Controllerlayouts? Controllerlayouts { get; set; }
	}
}
