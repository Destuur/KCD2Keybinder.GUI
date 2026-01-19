using KDC2Keybinder.Core.Models;
using KDC2Keybinder.Core.Models.DefaultProfile.Platform;
using KDC2Keybinder.Core.Models.Superactions;
using System.Xml;
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
					   }).ToList();
		}
		#endregion

		#region Superactions
		private static List<Superaction> ParseSuperactions(XElement root)
		{
			return root.Elements("superaction").Select(ParseSuperaction).ToList();
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

			sa.Actions = saEl.Elements("action")
							 .Select(act => new SuperactionAction
							 {
								 Name = (string?)act.Attribute("name") ?? "",
								 Map = (string?)act.Attribute("map") ?? ""
							 }).ToList();

			sa.Controls = saEl.Elements("control")
							  .Select(c => new Control
							  {
								  Input = (string?)c.Attribute("input") ?? "",
								  Controller = (string?)c.Attribute("controller") ?? ""
							  }).ToList();

			return sa;
		}
		#endregion

		#region Conflicts
		private static List<Conflict> ParseConflicts(XElement root)
		{
			return root.Elements("conflict").Select(ParseConflict).ToList();
		}

		private static Conflict ParseConflict(XElement conflictEl)
		{
			var nameAttr = (string?)conflictEl.Attribute("name");
			var comment = GetPrecedingComment(conflictEl);
			var line = GetLineNumber(conflictEl);

			var conflict = new Conflict
			{
				Name = nameAttr ?? "",
				Id = nameAttr ?? comment ?? (line != null ? $"conflict@line:{line}" : Guid.NewGuid().ToString())
			};

			conflict.Superactions = conflictEl.Elements("superaction").Select(saEl => new Superaction
			{
				Name = (string?)saEl.Attribute("name") ?? ""
			}).ToList();

			conflict.Includes = conflictEl.Elements("include").Select(incEl => new IncludeConflict
			{
				Conflict = (string?)incEl.Attribute("conflict") ?? ""
			}).ToList();

			return conflict;
		}

		#endregion

		private static string? GetPrecedingComment(XElement element)
		{
			var node = element.PreviousNode;

			while (node != null)
			{
				if (node is XComment comment) return comment.Value.Trim();
				if (node is XElement) break;
				node = node.PreviousNode;
			}

			return null;
		}

		private static int? GetLineNumber(XElement el)
		{
			return el is IXmlLineInfo li && li.HasLineInfo() ? li.LineNumber : null;
		}

		public static XDocument BuildKeybindSuperactionsXml(MergedKeybindStore mergedStore)
		{
			var root = new XElement("keybinds");

			// --- UI Groups ---
			foreach (var ug in mergedStore.VanillaKeybinds.UIGroups)
			{
				root.Add(new XElement("ui_group",
					new XAttribute("name", ug.Name),
					new XAttribute("ui_label", ug.UiLabel)
				));
			}

			// --- Superactions ---
			foreach (var sa in mergedStore.Superactions)
			{
				var saEl = new XElement("superaction",
					new XAttribute("name", sa.Name),
					new XAttribute("ui_group", sa.UiGroup ?? ""),
					new XAttribute("ui_name", sa.UiName ?? ""),
					new XAttribute("ui_tooltip", sa.UiTooltip ?? ""),
					new XAttribute("keyboard", sa.Keyboard ?? "")
				);

				// Actions
				foreach (var a in sa.Actions)
				{
					var actEl = new XElement("action",
						new XAttribute("name", a.Name),
						new XAttribute("map", a.Map)
					);

					saEl.Add(actEl);
				}

				// Controls
				foreach (var c in sa.Controls)
				{
					saEl.Add(new XElement("control",
						new XAttribute("input", c.Input),
						new XAttribute("controller", c.Controller)
					));
				}

				root.Add(saEl);
			}

			// --- Conflicts ---
			foreach (var conflict in mergedStore.Conflicts)
			{
				var conflictEl = new XElement("conflict");
				if (!string.IsNullOrEmpty(conflict.Name))
					conflictEl.SetAttributeValue("name", conflict.Name);

				foreach (var sa in conflict.Superactions)
					conflictEl.Add(new XElement("superaction", new XAttribute("name", sa.Name)));

				foreach (var inc in conflict.Includes)
					conflictEl.Add(new XElement("include", new XAttribute("conflict", inc.Conflict)));

				root.Add(conflictEl);
			}

			return new XDocument(new XDeclaration("1.0", "utf-8", "yes"), root);
		}
	}
}
