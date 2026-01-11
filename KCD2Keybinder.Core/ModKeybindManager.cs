using KDC2Keybinder.Core.Models.DefaultProfile;
using KDC2Keybinder.Core.Models.Superactions;
using KDC2Keybinder.Core.Services;
using KDC2Keybinder.Core.Utils;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace KDC2Keybinder.Core
{
	public class ModKeybindManager
	{
		private readonly ILogger<ModKeybindManager> logger;
		private readonly IPathProvider pathProvider;

		public Profile? BaseProfile { get; private set; }
		public Keybinds? BaseKeybinds { get; private set; }

		public List<ModData> Mods { get; } = new();

		public ModKeybindManager(ILogger<ModKeybindManager> logger, IPathProvider pathProvider)
		{
			this.logger = logger;
			this.pathProvider = pathProvider;
		}

		#region --- Load Base Game ---

		public void LoadBaseGame(string pakFileName)
		{
			var pakPath = Path.Combine(pathProvider.GamePath, "Data", pakFileName);
			if (!File.Exists(pakPath))
			{
				logger.LogError("Base game .pak file not found at path: {PakPath}", pakPath);
				throw new FileNotFoundException("Base game .pak file not found.", pakPath);
			}

			using var reader = new PakReader(pakPath);

			// --- Profile ---
			var profileXml = reader.ReadFile(Resources.DefaultProfileXML);
			if (!string.IsNullOrEmpty(profileXml))
			{
				var serializer = new XmlSerializer(typeof(Profile));
				using var sr = new StringReader(profileXml);
				BaseProfile = (Profile)serializer.Deserialize(sr)!;
			}

			// --- Keybinds ---
			var keybindXml = reader.ReadFile(Resources.KeybindSuperactions);
			if (!string.IsNullOrEmpty(keybindXml))
			{
				var doc = XDocument.Parse(keybindXml);
				BaseKeybinds = KeybindsParser.Parse(doc);
			}
			logger.LogInformation("Base game data loaded successfully from {PakPath}", pakPath);
		}

		#endregion

		#region --- Load Mods ---

		public void LoadMods(string modsRoot)
		{
			if (!Directory.Exists(modsRoot))
			{
				logger.LogWarning("Mods directory does not exist at path: {ModsRoot}", modsRoot);
				return;
			}

			Mods.Clear();

			foreach (var modDir in Directory.GetDirectories(modsRoot))
			{
				var modName = Path.GetFileName(modDir);
				if (modName.StartsWith("zz", StringComparison.OrdinalIgnoreCase))
					continue;

				var manifest = Path.Combine(modDir, "mod.manifest");
				if (!File.Exists(manifest)) continue;

				var modId = GetModIdFromManifest(manifest);

				var dataPath = Path.Combine(modDir, "Data");
				if (!Directory.Exists(dataPath)) continue;

				var pakFile = Directory.GetFiles(dataPath, "*.pak").FirstOrDefault();
				if (pakFile == null) continue;

				using var pakReader = new PakReader(pakFile);

				var modData = new ModData
				{
					ModId = modId,
					ModPath = modDir
				};

				// --- Keybinds ---
				var keybindXml = pakReader.ReadFile(Resources.KeybindSuperactions);
				if (!string.IsNullOrEmpty(keybindXml))
				{
					var doc = XDocument.Parse(keybindXml);
					modData.Keybinds = KeybindsParser.Parse(doc);
				}

				// --- Profile ---
				var profileXml = pakReader.ReadFile(Resources.DefaultProfileXML);
				if (!string.IsNullOrEmpty(profileXml))
				{
					var serializer = new XmlSerializer(typeof(Profile));
					using var sr = new StringReader(profileXml);
					modData.Profile = (Profile)serializer.Deserialize(sr)!;
				}

				logger.LogInformation("Loaded mod: {ModId} from {ModPath}", modId, modDir);
				Mods.Add(modData);
			}
		}

		private string GetModIdFromManifest(string manifestPath)
		{
			var doc = XDocument.Load(manifestPath);
			return doc.Root?.Element("info")?.Element("modid")?.Value ?? Path.GetFileNameWithoutExtension(manifestPath);
		}

		#endregion
	}
}
