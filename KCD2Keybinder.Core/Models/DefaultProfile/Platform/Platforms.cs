using System.Xml.Serialization;

namespace KDC2Keybinder.Core.Models.DefaultProfile.Platform
{
	[XmlRoot("platforms")]
	public class Platforms
	{
		[XmlElement("PC", typeof(PC))]
		[XmlElement("Xbox", typeof(Xbox))]
		[XmlElement("PS4", typeof(PS4))]
		public List<PlatformBase> PlatformList { get; set; } = [];
	}
}
