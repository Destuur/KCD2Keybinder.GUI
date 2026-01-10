using System.Xml.Serialization;

namespace KDC2Keybinder.Core.Models.DefaultProfile.ActionMaps
{
	public class IncludeActionMap
	{
		[XmlAttribute("actionmap")]
		public string ActionMap { get; set; } = string.Empty;
	}
}
