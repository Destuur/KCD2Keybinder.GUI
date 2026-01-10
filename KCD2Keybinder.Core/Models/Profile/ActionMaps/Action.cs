using System.Xml.Serialization;

namespace KDC2Keybinder.Core.Models.Profile.ActionMaps
{
	public class Action
	{
		[XmlAttribute("name")]
		public string Name { get; set; } = string.Empty;
		[XmlAttribute("onPress")]
		public string OnPress { get; set; } = string.Empty;
		[XmlAttribute("onRelease")]
		public string OnRelease { get; set; } = string.Empty;
		[XmlAttribute("onHold")]
		public string OnHold { get; set; } = string.Empty;
		[XmlAttribute("noModifiers")]
		public string? NoModifiers { get; set; }
		[XmlAttribute("keyboard")]
		public string? Keyboard { get; set; }
		[XmlAttribute("retriggerable")]
		public string Retriggerable { get; set; } = string.Empty;
		[XmlAttribute("holdTriggerDelay")]
		public string HoldTriggerDelay { get; set; } = string.Empty;
		[XmlAttribute("holdRepeatDelay")]
		public string HoldRepeatDelay { get; set; } = string.Empty;
		[XmlAttribute("xboxpad")]
		public string? Xboxpad { get; set; }
		[XmlAttribute("pspad")]
		public string? Pspad { get; set; }
		[XmlElement("xboxpad")]
		public XBoxPad? XBoxPad { get; set; }
		[XmlElement("pspad")]
		public PspPad? PspPad { get; set; }
	}
}
