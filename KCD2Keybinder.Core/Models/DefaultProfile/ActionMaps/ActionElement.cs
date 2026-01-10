using System.Xml.Serialization;

namespace KDC2Keybinder.Core.Models.DefaultProfile.ActionMaps
{
	public class ActionElement
	{
		[XmlAttribute("name")]
		public string Name { get; set; } = string.Empty;
		[XmlAttribute("onPress")]
		public string? OnPress { get; set; }
		[XmlAttribute("onRelease")]
		public string? OnRelease { get; set; }
		[XmlAttribute("onHold")]
		public string? OnHold { get; set; }
		[XmlAttribute("noModifiers")]
		public string? NoModifiers { get; set; }
		[XmlAttribute("keyboard")]
		public string? Keyboard { get; set; }
		[XmlAttribute("retriggerable")]
		public string? Retriggerable { get; set; }
		[XmlAttribute("holdTriggerDelay")]
		public string? HoldTriggerDelay { get; set; }
		[XmlAttribute("holdRepeatDelay")]
		public string? HoldRepeatDelay { get; set; }
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
