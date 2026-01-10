using System.Xml.Serialization;

namespace KDC2Keybinder.Core.Models.Superactions
{
	public class SuperactionAction
	{
		[XmlAttribute("name")]
		public string Name { get; set; } = string.Empty;
		[XmlAttribute("map")]
		public string Map { get; set; } = string.Empty;
	}
}
