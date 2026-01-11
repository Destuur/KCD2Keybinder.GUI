namespace KDC2Keybinder.Core
{
	public class ModDiffViewModel
	{
		public string ModId { get; set; } = "";
		public KeybindsChange Changes { get; set; } = new();
	}

}
