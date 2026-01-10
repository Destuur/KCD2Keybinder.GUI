using KDC2Keybinder.Core.Models.DefaultProfile.ActionMaps;
using System.Xml.Serialization;

namespace KDC2Keybinder.Core.Models.Superactions
{
	[XmlRoot("superaction")]
	public class Superaction
	{
		[XmlAttribute("name")]
		public string Name { get; set; } = string.Empty;
		[XmlAttribute("ui_group")]
		public string UiGroup { get; set; } = string.Empty;
		[XmlAttribute("ui_name")]
		public string UiName { get; set; } = string.Empty;
		[XmlAttribute("ui_tooltip")]
		public string UiTooltip { get; set; } = string.Empty;
		[XmlAttribute("keyboard")]
		public string Keyboard { get; set; } = string.Empty;
		[XmlElement("action")]
		public List<SuperactionAction> Actions { get; set; } = new List<SuperactionAction>();
		[XmlElement("control")]
		public List<Control> Controls { get; set; } = new List<Control>();
	}
}
