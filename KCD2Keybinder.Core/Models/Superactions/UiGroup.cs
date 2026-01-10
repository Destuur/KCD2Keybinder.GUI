using System.Xml.Serialization;

namespace KDC2Keybinder.Core.Models.Superactions
{
	[XmlRoot("ui_group")]
	public class UiGroup
	{
		[XmlAttribute("name")]
		public string Name { get; set; } = string.Empty;
		[XmlAttribute("ui_label")]
		public string UiLabel { get; set; } = string.Empty;
	}
}
