using System.Xml.Serialization;

namespace KDC2Keybinder.Core.Models.Profile.Platforms
{
	public interface IPlatform
	{
		[XmlAttribute("keyboard")]
		string Keyboard { get; set; }
		[XmlAttribute("xboxpad")]
		string Xboxpad { get; set; }
		[XmlAttribute("pspad")]
		string Pspad { get; set; }
	}
}
