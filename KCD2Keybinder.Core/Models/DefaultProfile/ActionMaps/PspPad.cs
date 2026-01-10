using System.Xml.Serialization;

namespace KDC2Keybinder.Core.Models.DefaultProfile.ActionMaps
{
	public class PspPad
	{
		[XmlElement("inputdata")]
		public List<InputData> InputData { get; set; } = [];
	}
}
