namespace KDC2Keybinder.Core
{
	public class ResolvableModChange<T>
	{
		public ModChange<T> Change { get; set; } = null!;
		public T SelectedValue { get; set; } = default!;
	}

}
