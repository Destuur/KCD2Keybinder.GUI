using System.Xml.Serialization;

namespace KDC2Keybinder.Core.Models
{
	public class Action
	{
		[XmlAttribute("name")]
		public string Name { get; set; } = string.Empty;
		[XmlAttribute("map")]
		public string Map { get; set; } = string.Empty;
		[XmlAttribute("onPress")]
		public string OnPress { get; set; } = string.Empty;
		[XmlAttribute("onRelease")]
		public string OnRelease { get; set; } = string.Empty;
		[XmlAttribute("onHold")]
		public string OnHold { get; set; } = string.Empty;
		[XmlAttribute("retriggerable")]
		public string Retriggerable { get; set; } = string.Empty;
		[XmlAttribute("holdTriggerDelay")]
		public string HoldTriggerDelay { get; set; } = string.Empty;
		[XmlAttribute("holdRepeatDelay")]
		public string HoldRepeatDelay { get; set; } = string.Empty;
	}
}
