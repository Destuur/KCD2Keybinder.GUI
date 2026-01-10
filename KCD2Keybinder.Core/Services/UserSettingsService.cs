using KDC2Keybinder.Core.Models;
using System.Text.Json;

namespace KDC2Keybinder.Core.Services
{
	public class UserSettingsService : IUserSettingsService
	{
		private readonly string filePath;
		private UserSettings current;
		private JsonSerializerOptions options = new JsonSerializerOptions
		{
			WriteIndented = true
		};

		public event Action<UserSettings>? SettingsChanged;

		public UserSettings Current => current;

		public UserSettingsService()
		{
			filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "KCD2Keybinder", "settings.json");

			Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
			current = Load();
		}

		public void Update(Action<UserSettings> update)
		{
			update(current);
			Save();
			SettingsChanged?.Invoke(current);
		}

		private UserSettings Load()
		{
			if (!File.Exists(filePath))
			{
				return new UserSettings();
			}

			return JsonSerializer.Deserialize<UserSettings>(File.ReadAllText(filePath)) ?? new UserSettings();
		}

		private void Save()
		{
			File.WriteAllText(filePath, JsonSerializer.Serialize(current, options));
		}
	}

}
