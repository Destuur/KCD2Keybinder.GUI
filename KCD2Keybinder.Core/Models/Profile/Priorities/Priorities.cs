using System.Xml.Serialization;

namespace KDC2Keybinder.Core.Models.Profile.Priorities
{
	[XmlRoot("priorities")]
	public class Priorities
	{
		[XmlElement("priority")]
		public List<Priority> PriorityList { get; set; } = [];
	}
}
