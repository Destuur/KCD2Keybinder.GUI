using KDC2Keybinder.Core.Models;
using KDC2Keybinder.Core.Models.DefaultProfile;
using KDC2Keybinder.Core.Models.DefaultProfile.ActionMaps;
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
		private readonly ModService modService;
		private readonly IPathProvider pathProvider;
		private Profile? profile;
		private Keybinds? keybinds;

		public VanillaStore? VanillaStore { get; set; }
		public MergedKeybindStore? MergedKeybindStore { get; set; }
		public List<ModData> Mods { get; } = new();
		public List<ModDeltaViewModel> DeltaViewModels { get; } = new();

		public ModKeybindManager(ILogger<ModKeybindManager> logger, ModService modService, IPathProvider pathProvider)
		{
			this.logger = logger;
			this.modService = modService;
			this.pathProvider = pathProvider;
		}

		public void BuildMod()
		{
			if (MergedKeybindStore is null) return;

			var modDesc = modService.CreateNewMod();

			var modRoot = Path.Combine(Resources.ModsDirectory, modDesc.Id);
			modService.WriteModManifest(modDesc);

			WriteMergedModFiles(MergedKeybindStore, modRoot);
			var pakFile = Path.Combine(modRoot, "Data", modDesc.Id + ".pak");
			modService.CreateModPak(Path.Combine(modRoot, "Data"), pakFile);
		}

		public bool WriteMergedModFiles(MergedKeybindStore mergedStore, string modRootPath)
		{
			try
			{
				var dataPath = Path.Combine(modRootPath, "Data", "zz_keybinder", "Libs", "Config");
				Directory.CreateDirectory(dataPath);

				var keybindXml = KeybindsParser.BuildKeybindSuperactionsXml(mergedStore);
				var keybindPath = Path.Combine(dataPath, "keybindSuperactions.xml");
				keybindXml.Save(keybindPath);

				var profileXml = ProfileParser.BuildDefaultProfileXml(mergedStore);
				var profilePath = Path.Combine(dataPath, "defaultProfile.xml");
				profileXml.Save(profilePath);

				return true;
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Failed to write merged mod files to {ModRootPath}", modRootPath);
				return false;
			}
		}


		public void ApplyAllToMergedStore(MergeStore mergeStore)
		{
			if (MergedKeybindStore is null)
			{
				logger.LogError("MergedKeybindStore is not initialized.");
				throw new InvalidOperationException("MergedKeybindStore is not initialized.");
			}
			foreach (var kvp in mergeStore.Changes)
			{
				var entries = kvp.Value;

				var selected = entries.First();

				switch (selected.Delta)
				{
					case Superaction sa:
						MergedKeybindStore.ApplyModDelta(new ModDelta
						{
							Superactions = { [sa.Name] = sa }
						});
						break;

					case ActionMap am:
						MergedKeybindStore.ApplyModDelta(new ModDelta
						{
							ActionMaps = { [am.Name] = am }
						});
						break;

					case Conflict conflict:
						MergedKeybindStore.ApplyModDelta(new ModDelta
						{
							Conflicts = { [conflict.Id] = conflict }
						});
						break;
				}
			}
			DeltaViewModels.Clear();
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

			var profileXml = reader.ReadFile(Resources.DefaultProfileXML);
			if (!string.IsNullOrEmpty(profileXml))
			{
				var serializer = new XmlSerializer(typeof(Profile));
				using var sr = new StringReader(profileXml);
				profile = (Profile)serializer.Deserialize(sr)!;
			}

			var keybindXml = reader.ReadFile(Resources.KeybindSuperactions);
			if (!string.IsNullOrEmpty(keybindXml))
			{
				var doc = XDocument.Parse(keybindXml, LoadOptions.SetLineInfo | LoadOptions.PreserveWhitespace);
				keybinds = KeybindsParser.Parse(doc);
			}

			if (profile == null || keybinds == null)
			{
				logger.LogError("Failed to load base game data from {PakPath}", pakPath);
				throw new InvalidOperationException("Failed to load base game data.");
			}

			VanillaStore = new VanillaStore
			{
				Profile = profile,
				Keybinds = keybinds,
				SuperactionsByName = keybinds.Superactions.ToDictionary(sa => sa.Name),
				ActionMapsByName = profile.ActionMaps.ToDictionary(am => am.Name)
			};

			MergedKeybindStore = new MergedKeybindStore(keybinds, profile);
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
					Id = modId,
					Path = modDir
				};

				var keybindXml = pakReader.ReadFile(Resources.KeybindSuperactions);
				if (!string.IsNullOrEmpty(keybindXml))
				{
					var doc = XDocument.Parse(keybindXml);
					modData.Keybinds = KeybindsParser.Parse(doc);
				}

				var profileXml = pakReader.ReadFile(Resources.DefaultProfileXML);
				if (!string.IsNullOrEmpty(profileXml))
				{
					var serializer = new XmlSerializer(typeof(Profile));
					using var sr = new StringReader(profileXml);
					modData.Profile = (Profile)serializer.Deserialize(sr)!;
				}

				if (VanillaStore != null)
				{
					modData.Delta = ExtractModDelta(modData, VanillaStore, logger);
				}

				logger.LogInformation("Loaded mod: {ModId} from {ModPath}", modId, modDir);
				Mods.Add(modData);
			}

			BuildDeltaViewModels();
		}

		public static ModDelta ExtractModDelta(ModData mod, VanillaStore vanilla, ILogger logger)
		{
			var delta = new ModDelta { ModId = mod.Id };

			if (mod.Keybinds?.Superactions is not null)
			{
				foreach (var sa in mod.Keybinds.Superactions)
				{
					if (!vanilla.SuperactionsByName.TryGetValue(sa.Name, out var baseSa) || !SuperactionEquals(baseSa, sa))
					{
						delta.Superactions[sa.Name] = sa;

						logger.LogDebug("Mod {ModId}: Superaction changed or new: {Name}", mod.Id, sa.Name);
					}
				}
			}

			if (mod.Profile?.ActionMaps is not null)
			{
				foreach (var map in mod.Profile.ActionMaps)
				{
					if (!vanilla.ActionMapsByName.TryGetValue(map.Name, out var baseMap) || !ActionMapEquals(baseMap, map))
					{
						delta.ActionMaps[map.Name] = map;

						logger.LogDebug("Mod {ModId}: ActionMap changed or new: {Name}", mod.Id, map.Name);
					}
				}
			}

			if (mod.Keybinds?.Conflicts is not null)
			{
				foreach (var conflict in mod.Keybinds.Conflicts)
				{
					if (!vanilla.ConflictsByName.TryGetValue(conflict.Id, out var baseConflict) || !ConflictEquals(baseConflict, conflict))
					{
						delta.Conflicts[conflict.Id] = conflict;

						logger.LogDebug($"Conflict: {conflict.Id}");
					}
				}
			}

			return delta;
		}



		private string GetModIdFromManifest(string manifestPath)
		{
			var doc = XDocument.Load(manifestPath);
			return doc.Root?.Element("info")?.Element("modid")?.Value ?? Path.GetFileNameWithoutExtension(manifestPath);
		}

		#endregion

		#region --- Helper Functions ---

		public void BuildDeltaViewModels()
		{
			DeltaViewModels.Clear();
			if (VanillaStore == null) return;

			foreach (var mod in Mods)
			{
				var delta = ExtractModDelta(mod, VanillaStore, logger);
				DeltaViewModels.Add(new ModDeltaViewModel
				{
					ModId = delta.ModId,
					ChangedSuperactions = delta.Superactions.Values.ToList(),
					ChangedActionMaps = delta.ActionMaps.Values.ToList(),
					ChangedConflicts = delta.Conflicts.Values.ToList(),
				});
			}
		}

		private static bool SuperactionEquals(Superaction a, Superaction b)
		{
			if (a.Name != b.Name) return false;

			if (a.Actions.Count != b.Actions.Count) return false;

			var aActions = a.Actions.OrderBy(x => x.Name).ThenBy(x => x.Map).ToList();

			var bActions = b.Actions.OrderBy(x => x.Name).ThenBy(x => x.Map).ToList();

			for (int i = 0; i < aActions.Count; i++)
			{
				if (aActions[i].Name != bActions[i].Name) return false;
				if (aActions[i].Map != bActions[i].Map) return false;
			}

			return true;
		}

		private static bool ActionMapEquals(ActionMap a, ActionMap b)
		{
			if (a.Name != b.Name) return false;

			if (a.Priority != b.Priority) return false;

			if (a.Exclusivity != b.Exclusivity) return false;

			if (!UnorderedEqual(a.Includes.Select(i => i.ActionMap), b.Includes.Select(i => i.ActionMap))) return false;

			if (a.Actions.Count != b.Actions.Count) return false;

			var aActions = a.Actions.OrderBy(x => x.Name).ToList();

			var bActions = b.Actions.OrderBy(x => x.Name).ToList();

			for (int i = 0; i < aActions.Count; i++)
			{
				if (!ActionElementEquals(aActions[i], bActions[i])) return false;
			}

			return true;
		}

		private static bool ConflictEquals(Conflict a, Conflict b)
		{
			if (a.Name != b.Name) return false;
			if (a.Id != b.Id) return false;

			if (!UnorderedEqual(
				a.Includes.Select(i => i.Conflict),
				b.Includes.Select(i => i.Conflict)))
				return false;

			if (!UnorderedEqual(
				a.Superactions,
				b.Superactions))
				return false;

			return true;
		}

		private static bool ActionElementEquals(ActionElement a, ActionElement b)
		{
			if (a.Name != b.Name) return false;

			return a.OnPress == b.OnPress
				&& a.OnRelease == b.OnRelease
				&& a.OnHold == b.OnHold
				&& a.NoModifiers == b.NoModifiers
				&& a.Retriggerable == b.Retriggerable
				&& a.HoldTriggerDelay == b.HoldTriggerDelay
				&& a.HoldRepeatDelay == b.HoldRepeatDelay
				&& a.Keyboard == b.Keyboard
				&& a.Xboxpad == b.Xboxpad
				&& a.Pspad == b.Pspad;
		}

		private static bool UnorderedEqual<T>(IEnumerable<T> a, IEnumerable<T> b)
		{
			var setA = a.ToHashSet();
			var setB = b.ToHashSet();
			return setA.SetEquals(setB);
		}
		#endregion
	}
}
