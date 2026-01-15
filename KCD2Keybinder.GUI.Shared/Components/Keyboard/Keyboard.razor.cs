using KCD2Keybinder.GUI.Shared.Models;
using KDC2Keybinder.Core.Models;
using Microsoft.AspNetCore.Components;

namespace KCD2Keybinder.GUI.Shared.Components.Keyboard
{
	public partial class Keyboard
	{
		private string? activeKey;

		[Parameter]
		public KeyboardLayout Layout { get; set; } = KeyboardLayout.QWERTY;
		[Parameter]
		public MergeStore MergeStore { get; set; } = null!;
		[Parameter]
		public EventCallback<string?> ActiveKeyChanged { get; set; }

		private async Task OnKeyPressed(string key)
		{
			if (activeKey == key)
			{
				activeKey = null;
			}
			else
			{
				activeKey = key;
			}

			await ActiveKeyChanged.InvokeAsync(activeKey);
		}
	}
}