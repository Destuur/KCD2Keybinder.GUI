using KDC2Keybinder.Core.Services;

namespace KDC2Keybinder.Core.Utils
{
	public class UserPathProvider : IPathProvider
	{
		private readonly IUserSettingsService settings;

		public UserPathProvider(IUserSettingsService settings)
		{
			this.settings = settings;
		}

		public string GamePath => settings.Current.GamePath;
		public string ModPath => settings.Current.ModPath;
	}
}
