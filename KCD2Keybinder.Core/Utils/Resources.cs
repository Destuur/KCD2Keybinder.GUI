using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDC2Keybinder.Core.Utils
{
	public static class Resources
	{
		public static string GameDirectory => "";
		public static string DataDirectory => Path.Combine(GameDirectory, "Data");
		public static string ModsDirectory => Path.Combine(GameDirectory, "Mods");
		public static string IPLGameData => "IPL_GameData.pak";
		public static string DefaultProfileXML => "Libs/Config/defaultProfile.xml";
		public static string KeybindSuperactions => "Libs/Config/keybindSuperactions.xml";

	}
}
