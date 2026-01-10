using System.Xml.Serialization;

namespace KDC2Keybinder.Core.Models.Profile.Platforms
{
	[XmlRoot("platforms")]
	public class Platforms
	{
		public List<IPlatform> PlatformList { get; set; } = [];
	}
}
