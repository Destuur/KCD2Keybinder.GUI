using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace KCD2Keybinder.GUI.Shared.Components
{
	public partial class StartupLoader
	{
		private bool hidden = false;

		[Inject]
		private IJSRuntime JS { get; set; } = null!;

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				hidden = true;
				await JS.InvokeVoidAsync("removeInitialLoader");
				await InvokeAsync(StateHasChanged);
			}
		}
	}
}