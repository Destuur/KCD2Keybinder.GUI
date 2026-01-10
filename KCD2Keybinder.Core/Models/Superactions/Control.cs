using System.Xml.Serialization;

namespace KDC2Keybinder.Core.Models.Superactions
{
	public class Control
	{
		[XmlAttribute("input")]
		public string Input { get; set; } = string.Empty;
		[XmlAttribute("controller")]
		public string Controller { get; set; } = string.Empty;
	}
}
