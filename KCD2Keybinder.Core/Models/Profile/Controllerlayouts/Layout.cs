using System.Xml.Serialization;

namespace KDC2Keybinder.Core.Models.Profile.Controllerlayouts
{
	public class Layout
	{
		[XmlAttribute("name")]
		public string Name { get; set; } = string.Empty;
		[XmlElement("button")]
		public string File { get; set; } = string.Empty;
	}
}
