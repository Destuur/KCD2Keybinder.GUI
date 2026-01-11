using KDC2Keybinder.Core.Models;
using KDC2Keybinder.Core.Models.DefaultProfile.ActionMaps;
using KDC2Keybinder.Core.Models.Superactions;
using System.Text.Json;

namespace KDC2Keybinder.Core
{
	public static class ModComparer
	{
		public static ResolvableKeybindsChange ToResolvable(KeybindsChange changes)
		{
			var resolvable = new ResolvableKeybindsChange();

			resolvable.Superactions.AddRange(changes.Superactions.Select(c => new ResolvableModChange<Superaction>
			{
				Change = c,
				SelectedValue = c.Original  // initial Basegame wählen
			}));

			resolvable.ActionMaps.AddRange(changes.ActionMaps.Select(c => new ResolvableModChange<ActionMap>
			{
				Change = c,
				SelectedValue = c.Original
			}));

			return resolvable;
		}

		public static void CompareModToBase(ModMergeContext context)
		{
			var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			var filePath = Path.Combine(appData, "KCD2Keybinder", "IgnoredDifferences.json");
			var json = File.ReadAllText(filePath);
			var ignoreConfig = JsonSerializer.Deserialize<IgnoreConfig>(json);

			if (context.BaseProfile == null || context.BaseKeybinds == null)
			{
				throw new InvalidOperationException("Base game data must be loaded.");
			}

			foreach (var mod in context.Mods)
			{
				var changes = new KeybindsChange { };

				// --- Superactions vergleichen ---
				if (mod.Keybinds?.Superactions != null)
				{
					foreach (var sa in mod.Keybinds.Superactions)
					{
						if (ignoreConfig?.IgnoredObjects.Any(x =>
								x.ModId == mod.ModId &&
								x.ObjectType == nameof(Superaction) &&
								x.ObjectName == sa.Name) == true)
						{
							continue; // ignorieren
						}

						var baseSa = context.BaseKeybinds.Superactions.FirstOrDefault(x => x.Name == sa.Name);

						if (baseSa == null || !SuperactionEquals(baseSa, sa))
						{
							changes.Superactions.Add(new ModChange<Superaction>
							{
								ModId = mod.ModId,
								Original = baseSa ?? new Superaction { Name = sa.Name },
								Modified = sa
							});
						}
					}
				}

				// --- ActionMaps vergleichen ---
				if (mod.Profile?.ActionMaps != null)
				{
					foreach (var map in mod.Profile.ActionMaps)
					{
						if (ignoreConfig?.IgnoredObjects.Any(x =>
							x.ModId == mod.ModId &&
							x.ObjectType == nameof(ActionMap) &&
							x.ObjectName == map.Name) == true)
						{
							// Objekt ignorieren, wird nicht als Änderung gemeldet
							continue;
						}

						var baseMap = context.BaseProfile.ActionMaps.FirstOrDefault(x => x.Name == map.Name);

						if (baseMap == null || !ActionMapEquals(baseMap, map))
						{
							changes.ActionMaps.Add(new ModChange<ActionMap>
							{
								ModId = mod.ModId,
								Original = baseMap ?? new ActionMap { Name = map.Name },
								Modified = map
							});
						}
					}
				}

				// Änderungen für diese Mod speichern
				context.ChangesPerMod[mod.ModId] = changes;
			}
		}

		private static bool SuperactionEquals(Superaction a, Superaction b)
		{
			if (a.Name != b.Name || a.UiGroup != b.UiGroup || a.UiName != b.UiName)
				return false;

			if (a.Actions.Count != b.Actions.Count) return false;
			for (int i = 0; i < a.Actions.Count; i++)
			{
				if (!SuperactionActionEquals(a.Actions[i], b.Actions[i])) return false;
			}

			return true;
		}

		private static bool ActionMapEquals(ActionMap a, ActionMap b)
		{
			if (a.Name != b.Name || a.Priority != b.Priority || a.Exclusivity != b.Exclusivity)
				return false;

			if (a.Actions.Count != b.Actions.Count) return false;
			for (int i = 0; i < a.Actions.Count; i++)
			{
				if (!ActionElementEquals(a.Actions[i], b.Actions[i])) return false;
			}

			return true;
		}

		private static bool SuperactionActionEquals(SuperactionAction a, SuperactionAction b)
		{
			return a.Name == b.Name
				   && a.Map == b.Map;
			// Optional: xboxpad / pspad / keyboard vergleichen
		}

		private static bool ActionElementEquals(ActionElement a, ActionElement b)
		{
			return a.Name == b.Name
				   && a.OnPress == b.OnPress
				   && a.OnRelease == b.OnRelease
				   && a.OnHold == b.OnHold
				   && a.Retriggerable == b.Retriggerable
				   && a.HoldTriggerDelay == b.HoldTriggerDelay
				   && a.HoldRepeatDelay == b.HoldRepeatDelay;
			// Optional: xboxpad / pspad / keyboard vergleichen
		}
	}
}
