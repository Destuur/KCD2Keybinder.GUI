using System.Xml.Serialization;

namespace KDC2Keybinder.Core.Models.DefaultProfile.ActionMaps
{
	public class InputData
	{
		[XmlAttribute("input")]
		public string Input { get; set; } = string.Empty;
		[XmlAttribute("useAnalogCompare")]
		public string? UseAnalogCompare { get; set; } = string.Empty;
		[XmlAttribute("analogCompareVal")]
		public string? AnalogCompareVal { get; set; } = string.Empty;
		[XmlAttribute("analogCompareOp")]
		public string? AnalogCompareOp { get; set; } = string.Empty;
	}
}
