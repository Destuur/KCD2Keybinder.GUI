using System.Xml.Serialization;

namespace KDC2Keybinder.Core.Models
{
	public class ActionMap
	{
		[XmlAttribute("name")]
		public string Name { get; set; } = string.Empty;
		[XmlAttribute("priority")]
		public string Priority { get; set; } = "default";
		[XmlAttribute("exclusivity")]
		public string Exclusivity { get; set; } = "0";
		[XmlElement("action")]
		public List<Action> Actions { get; set; } = new();
		[XmlElement("include")]
		public List<string> Includes { get; set; } = new();
	}
}
