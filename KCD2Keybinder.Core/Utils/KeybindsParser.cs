using KDC2Keybinder.Core.Models;
using KDC2Keybinder.Core.Models.Superactions;
using System.Xml.Linq;

namespace KDC2Keybinder.Core.Utils
{
	public static class KeybindsParser
	{
		public static Keybinds Parse(XDocument doc)
		{
			if (doc.Root == null)
				throw new InvalidOperationException("Missing root element in Keybinds XML.");

			var keybinds = new Keybinds
			{
				UIGroups = ParseUiGroups(doc.Root),
				Superactions = ParseSuperactions(doc.Root),
				Conflicts = ParseConflicts(doc.Root)
			};

			return keybinds;
		}

		#region UI-Groups
		private static List<UiGroup> ParseUiGroups(XElement root)
		{
			return root.Elements("ui_group")
					   .Select(e => new UiGroup
					   {
						   Name = (string)e.Attribute("name")!,
						   UiLabel = (string)e.Attribute("ui_label")!
					   })
					   .ToList();
		}
		#endregion

		#region Superactions
		private static List<Superaction> ParseSuperactions(XElement root)
		{
			return root.Elements("superaction")
					   .Select(ParseSuperaction)
					   .ToList();
		}

		private static Superaction ParseSuperaction(XElement saEl)
		{
			var sa = new Superaction
			{
				Name = (string)saEl.Attribute("name")!,
				UiGroup = (string?)saEl.Attribute("ui_group") ?? "",
				UiName = (string?)saEl.Attribute("ui_name") ?? "",
				UiTooltip = (string?)saEl.Attribute("ui_tooltip") ?? "",
				Keyboard = (string?)saEl.Attribute("keyboard") ?? ""
			};

			// Actions
			sa.Actions = saEl.Elements("action")
							 .Select(act => new SuperactionAction
							 {
								 Name = (string?)act.Attribute("name") ?? "",
								 Map = (string?)act.Attribute("map") ?? ""
							 })
							 .ToList();

			// Controls
			sa.Controls = saEl.Elements("control")
							  .Select(c => new Control
							  {
								  Input = (string?)c.Attribute("input") ?? "",
								  Controller = (string?)c.Attribute("controller") ?? ""
							  })
							  .ToList();

			return sa;
		}
		#endregion

		#region Conflicts
		private static List<Conflict> ParseConflicts(XElement root)
		{
			return root.Elements("conflict")
					   .Select(ParseConflict)
					   .ToList();
		}

		private static Conflict ParseConflict(XElement conflictEl)
		{
			var conflict = new Conflict
			{
				Name = (string?)conflictEl.Attribute("name") ?? ""
			};

			// Superactions in Konflikt
			conflict.Superactions = conflictEl.Elements("superaction")
											  .Select(saEl => new Superaction
											  {
												  Name = (string?)saEl.Attribute("name") ?? ""
											  })
											  .ToList();

			// Include-Referenzen
			conflict.Includes = conflictEl.Elements("include")
										  .Select(incEl => new IncludeConflict
										  {
											  Conflict = (string?)incEl.Attribute("conflict") ?? ""
										  })
										  .ToList();

			return conflict;
		}
		#endregion
	}
}
