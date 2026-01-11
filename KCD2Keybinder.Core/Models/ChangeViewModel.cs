namespace KDC2Keybinder.Core
{
	public class ChangeViewModel<T>
	{
		public string ModId { get; set; } = "";
		public string Name { get; set; } = "";

		public T BaseValue { get; set; } = default!;
		public T ModValue { get; set; } = default!;

		public bool IsSelected { get; set; } = true;
	}

}
