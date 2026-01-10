using KDC2Keybinder.Core.Models.DefaultProfile;
using KDC2Keybinder.Core.Models.DefaultProfile.ActionMaps;
using KDC2Keybinder.Core.Models.Superactions;

namespace KDC2Keybinder.Core
{
	public class ModData
	{
		public string ModId { get; set; } = string.Empty;
		public string ModPath { get; set; } = string.Empty;
		public Profile? Profile { get; set; }
		public Keybinds? Keybinds { get; set; }
	}

	public class ModChange<T>
	{
		public string ModId { get; set; } = string.Empty;
		public T Original { get; set; } // Wert aus BaseGame
		public T Modified { get; set; } // Wert aus Mod
	}

	public class KeybindsChange
	{
		public List<ModChange<Superaction>> Superactions { get; } = new();
		public List<ModChange<ActionMap>> ActionMaps { get; } = new();
	}

	public class ModMergeContext
	{
		public Profile BaseProfile { get; set; } = null!;
		public Keybinds BaseKeybinds { get; set; } = null!;

		public List<ModData> Mods { get; set; } = new();

		// Änderungen pro Mod
		public Dictionary<string, KeybindsChange> ChangesPerMod { get; } = new();
	}

	public static class ModComparer
	{
		public static void CompareModToBase(ModMergeContext context)
		{
			if (context.BaseProfile == null || context.BaseKeybinds == null)
				throw new InvalidOperationException("Base game data must be loaded.");

			foreach (var mod in context.Mods)
			{
				var changes = new KeybindsChange { };

				// --- Superactions vergleichen ---
				if (mod.Keybinds?.Superactions != null)
				{
					foreach (var sa in mod.Keybinds.Superactions)
					{
						// BaseGame Superaction mit gleichem Namen suchen
						var baseSa = context.BaseKeybinds.Superactions
							.FirstOrDefault(x => x.Name == sa.Name);

						// Nur aufnehmen, wenn Unterschiede existieren
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
						var baseMap = context.BaseProfile.ActionMaps
							.FirstOrDefault(x => x.Name == map.Name);

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
