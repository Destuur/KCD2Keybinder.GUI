using KDC2Keybinder.Core.Models;

namespace KDC2Keybinder.Core.Services
{
	public interface IUserSettingsService
	{
		UserSettings Current { get; }

		event Action<UserSettings>? SettingsChanged;

		void Update(Action<UserSettings> update);
	}

}
