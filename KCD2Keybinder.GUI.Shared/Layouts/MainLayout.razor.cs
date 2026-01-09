namespace KCD2Keybinder.GUI.Shared.Layouts
{
	public partial class MainLayout
	{
		bool isLoaded = false;

		protected override async Task OnInitializedAsync()
		{
			await Task.Delay(50);
			isLoaded = true;
		}
	}
}