using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDC2Keybinder.Core.Models
{
	public class IgnoredObject
	{
		public string ModId { get; set; } = string.Empty;
		public string ObjectType { get; set; } = string.Empty;
		public string ObjectName { get; set; } = string.Empty;
	}

	public class IgnoreConfig
	{
		public List<IgnoredObject> IgnoredObjects { get; set; } = new();
	}
}
