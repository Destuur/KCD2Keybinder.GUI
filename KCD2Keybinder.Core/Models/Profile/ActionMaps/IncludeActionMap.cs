using System.Xml.Serialization;

namespace KDC2Keybinder.Core.Models
{
	public class IncludeActionMap
	{
		[XmlAttribute("actionmap")]
		public string ActionMap { get; set; } = string.Empty;
	}
}
