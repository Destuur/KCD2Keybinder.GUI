using System.Xml.Serialization;

namespace KDC2Keybinder.Core.Models.DefaultProfile.Controller
{
	public class Layout
	{
		[XmlAttribute("name")]
		public string Name { get; set; } = string.Empty;
		[XmlAttribute("button")]
		public string File { get; set; } = string.Empty;
	}
}
