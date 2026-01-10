using System.Xml.Serialization;

namespace KDC2Keybinder.Core.Models.Profile.ActionFilter
{
	[XmlRoot("actionfilter")]
	public class ActionFilter
	{
		[XmlAttribute("name")]
		public string Name { get; set; } = string.Empty;
		[XmlElement("action")]
		public List<FilterAction> Actions { get; set; } = [];
	}
}
