using System.Xml.Serialization;

namespace KDC2Keybinder.Core.Models.DefaultProfile.Filter
{
	public class FilterAction
	{
		[XmlAttribute("name")]
		public string Name { get; set; } = string.Empty;
	}
}
