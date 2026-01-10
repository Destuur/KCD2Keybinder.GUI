using System.Xml.Serialization;

namespace KDC2Keybinder.Core.Models.Profile.Platforms
{
	public class PS4 : IPlatform
	{
		[XmlAttribute("keyboard")]
		public string Keyboard { get; set; } = string.Empty;
		[XmlAttribute("xboxpad")]
		public string Xboxpad { get; set; } = string.Empty;
		[XmlAttribute("pspad")]
		public string Pspad { get; set; } = string.Empty;
	}
}
