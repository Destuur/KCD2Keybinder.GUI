using System.Xml.Serialization;

namespace KDC2Keybinder.Core.Models
{
	public class XBoxPad
	{
		[XmlElement("inputdata")]
		public List<InputData> InputData { get; set; } = [];
	}
}
