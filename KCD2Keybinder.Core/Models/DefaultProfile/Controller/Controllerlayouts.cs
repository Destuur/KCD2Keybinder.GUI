using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace KDC2Keybinder.Core.Models.DefaultProfile.Controller
{
	[XmlRoot("controllerlayouts")]
	public class Controllerlayouts
	{
		[XmlElement("layout")]
		public List<Layout> Layout { get; set; } = [];
	}
}
