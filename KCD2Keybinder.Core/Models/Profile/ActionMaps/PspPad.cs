using System.Xml.Serialization;

namespace KDC2Keybinder.Core.Models.Profile.ActionMaps
{
	public class PspPad
	{
		[XmlElement("inputdata")]
		public List<InputData> InputData { get; set; } = [];
	}
}
