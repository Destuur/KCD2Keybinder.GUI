using KDC2Keybinder.Core.Models.DefaultProfile;
using KDC2Keybinder.Core.Models.DefaultProfile.ActionMaps;
using KDC2Keybinder.Core.Models.Superactions;

namespace KDC2Keybinder.Core.Models
{
	public class MergedKeybindStore
	{
		public Keybinds VanillaKeybinds { get; private set; }
		public Profile VanillaProfile { get; private set; }

		public List<Superaction> Superactions { get; private set; } = [];
		public List<ActionMap> ActionMaps { get; private set; } = [];
		public List<Conflict> Conflicts { get; private set; } = [];

		public MergedKeybindStore(Keybinds vanillaKeybinds, Profile vanillaProfile)
		{
			VanillaKeybinds = vanillaKeybinds;
			VanillaProfile = vanillaProfile;

			Superactions = vanillaKeybinds.Superactions.Select(CloneSuperaction).ToList();
			ActionMaps = vanillaProfile.ActionMaps.Select(CloneActionMap).ToList();
			Conflicts = vanillaKeybinds.Conflicts.Select(CloneConflict).ToList();
		}

		private static Conflict CloneConflict(Conflict conflict)
		{
			return new Conflict
			{
				Id = conflict.Id,
				Name = conflict.Name,
				Includes = conflict.Includes.Select(x => new IncludeConflict
				{
					Conflict = x.Conflict,
				}).ToList(),
				Superactions = conflict.Superactions.Select(CloneSuperaction).ToList(),
			};
		}

		private static Superaction CloneSuperaction(Superaction sa)
		{
			return new Superaction
			{
				Name = sa.Name,
				UiGroup = sa.UiGroup,
				UiName = sa.UiName,
				UiTooltip = sa.UiTooltip,
				Keyboard = sa.Keyboard,
				Actions = sa.Actions.Select(a => new SuperactionAction
				{
					Name = a.Name,
					Map = a.Map
				}).ToList(),
				Controls = sa.Controls.Select(c => new Control
				{
					Controller = c.Controller,
					Input = c.Input
				}).ToList()
			};
		}

		private static ActionMap CloneActionMap(ActionMap map)
		{
			return new ActionMap
			{
				Name = map.Name,
				Priority = map.Priority,
				Exclusivity = map.Exclusivity,
				Actions = map.Actions.Select(a => new ActionElement
				{
					Name = a.Name,
					OnPress = a.OnPress,
					OnRelease = a.OnRelease,
					OnHold = a.OnHold,
					NoModifiers = a.NoModifiers,
					Retriggerable = a.Retriggerable,
					HoldTriggerDelay = a.HoldTriggerDelay,
					HoldRepeatDelay = a.HoldRepeatDelay,
					Keyboard = a.Keyboard,
					Xboxpad = a.Xboxpad,
					Pspad = a.Pspad
				}).ToList(),
				Includes = map.Includes.Select(i => new IncludeActionMap
				{
					ActionMap = i.ActionMap
				}).ToList()
			};
		}

		public void ApplyModDelta(ModDelta delta)
		{
			foreach (var sa in delta.Superactions)
			{
				var index = Superactions.FindIndex(s => s.Name == sa.Value.Name);
				if (index >= 0)
					Superactions[index] = CloneSuperaction(sa.Value);
				else
					Superactions.Add(CloneSuperaction(sa.Value));
			}

			foreach (var map in delta.ActionMaps)
			{
				var index = ActionMaps.FindIndex(m => m.Name == map.Value.Name);
				if (index >= 0)
					ActionMaps[index] = CloneActionMap(map.Value);
				else
					ActionMaps.Add(CloneActionMap(map.Value));
			}

			foreach (var conflict in delta.Conflicts)
			{
				var index = Conflicts.FindIndex(m => m.Id == conflict.Value.Id);
				if (index >= 0)
					Conflicts[index] = CloneConflict(conflict.Value);
				else
					Conflicts.Add(CloneConflict(conflict.Value));
			}
		}
	}
}
