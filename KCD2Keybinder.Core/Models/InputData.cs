using System.Xml.Serialization;

namespace KDC2Keybinder.Core.Models
{
	public class InputData
	{
		[XmlAttribute("input")]
		public string Input { get; set; } = string.Empty;
		[XmlAttribute("useAnalogCompare")]
		public string useAnalogCompare { get; set; } = string.Empty;
		[XmlAttribute("analogCompareVal")]
		public string analogCompareVal { get; set; } = string.Empty;
		[XmlAttribute("analogCompareOp")]
		public string analogCompareOp { get; set; } = string.Empty;
	}
}
