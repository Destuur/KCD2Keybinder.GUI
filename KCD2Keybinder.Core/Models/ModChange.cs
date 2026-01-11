namespace KDC2Keybinder.Core
{
	public class ModChange<T>
	{
		public string ModId { get; set; } = string.Empty;
		public T Original { get; set; } // Wert aus BaseGame
		public T Modified { get; set; } // Wert aus Mod
	}

}
