using KCD2Keybinder.GUI.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace KCD2Keybinder.GUI.Shared.Components.Keyboard
{
	public partial class Key
	{
		[Parameter, EditorRequired]
		public string Label { get; set; } = string.Empty;
		[Parameter, EditorRequired]
		public string Value { get; set; } = string.Empty;
		[Parameter]
		public bool HasConflict { get; set; }
		[Parameter]
		public bool IsMouse { get; set; }
		[Parameter]
		public int Size { get; set; } = 64;
		[Parameter]
		public KeySize KeySize { get; set; } = KeySize.Small;
		[Parameter]
		public Mouse Mouse { get; set; }
		[Parameter]
		public bool IsActivated { get; set; }
		[Parameter]
		public EventCallback<string> KeyPressed { get; set; }

		private async Task ClickButton()
		{
			await KeyPressed.InvokeAsync(Value);
		}

		private int GetWidth()
		{
			return KeySize switch
			{
				KeySize.Small => Size,
				KeySize.Medium => 102,
				KeySize.Large => 128,
				KeySize.Space => 384,
				_ => Size,
			};
		}

		private string GetKey()
		{
			if (IsMouse)
			{
				return Mouse switch
				{
					Mouse.Left => "_content/KCD2Keybinder.GUI.Shared/images/mouse_1.png",
					Mouse.Middle => "_content/KCD2Keybinder.GUI.Shared/images/mouse_3.png",
					Mouse.Right => "_content/KCD2Keybinder.GUI.Shared/images/mouse_2.png",
					_ => "_content/KCD2Keybinder.GUI.Shared/images/mouse_1.png",
				};
			}

			return KeySize switch
			{
				KeySize.Small => "_content/KCD2Keybinder.GUI.Shared/images/key.png",
				KeySize.Medium => "_content/KCD2Keybinder.GUI.Shared/images/key_true_middle.png",
				KeySize.Large => "_content/KCD2Keybinder.GUI.Shared/images/key_long.png",
				KeySize.Space => "_content/KCD2Keybinder.GUI.Shared/images/key_space.png",
				_ => "_content/KCD2Keybinder.GUI.Shared/images/key.png",
			};
		}
	}
}