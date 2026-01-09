using KCD2Keybinder.GUI.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace KCD2Keybinder.GUI.Shared.Components.Keyboard
{
	public partial class Key
	{
		[Parameter, EditorRequired]
		public string Label { get; set; } = string.Empty;
		[Parameter]
		public int Size { get; set; } = 64;
		[Parameter]
		public KeySize KeySize { get; set; } = KeySize.Small;
		[Parameter]
		public bool IsActivated { get; set; }
		[Parameter]
		public EventCallback<string> KeyPressed { get; set; }

		private async Task ClickButton()
		{
			await KeyPressed.InvokeAsync(Label);
		}

		private int GetWidth()
		{
			return KeySize switch
			{
				KeySize.Small => Size,
				KeySize.Medium => 128,
				KeySize.Large => 128,
				_ => Size,
			};
		}

		private string GetKey()
		{
			return KeySize switch
			{
				KeySize.Small => "_content/KCD2Keybinder.GUI.Shared/images/key.png",
				KeySize.Medium => "_content/KCD2Keybinder.GUI.Shared/images/key_middle.png",
				KeySize.Large => "_content/KCD2Keybinder.GUI.Shared/images/key_long.png",
				_ => "_content/KCD2Keybinder.GUI.Shared/images/key.png",
			};
		}
	}
}