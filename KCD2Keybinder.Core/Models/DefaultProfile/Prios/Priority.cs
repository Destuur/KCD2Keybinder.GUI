using System.Xml.Serialization;

namespace KDC2Keybinder.Core.Models.DefaultProfile.Prios
{
	public class Priority
	{
		[XmlAttribute("name")]
		public string Name { get; set; } = string.Empty;
		[XmlAttribute("value")]
		public string Value { get; set; } = string.Empty;
	}
}
