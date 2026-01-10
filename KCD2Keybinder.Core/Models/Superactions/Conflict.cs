using System.Xml.Serialization;

namespace KDC2Keybinder.Core.Models.Superactions
{
	[XmlRoot("conflict")]
	public class Conflict
	{
		[XmlAttribute("name")]
		public string Name { get; set; } = string.Empty;
		[XmlElement("superaction")]
		public List<Superaction> Superactions { get; set; } = new List<Superaction>();
		[XmlElement("include")]
		public List<IncludeConflict> Includes { get; set; } = new List<IncludeConflict>();
	}
}
