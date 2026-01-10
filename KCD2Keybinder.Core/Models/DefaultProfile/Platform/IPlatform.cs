using System.Xml.Serialization;

namespace KDC2Keybinder.Core.Models.DefaultProfile.Platform
{
	[XmlInclude(typeof(PC))]
	[XmlInclude(typeof(Xbox))]
	[XmlInclude(typeof(PS4))]
	public abstract class PlatformBase
	{
		[XmlAttribute("keyboard")] public string Keyboard { get; set; } = "";
		[XmlAttribute("xboxpad")] public string Xboxpad { get; set; } = "";
		[XmlAttribute("pspad")] public string Pspad { get; set; } = "";
	}
}
