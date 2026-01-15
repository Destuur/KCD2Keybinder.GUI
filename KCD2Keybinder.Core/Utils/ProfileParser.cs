using KDC2Keybinder.Core.Models;
using KDC2Keybinder.Core.Models.DefaultProfile;
using KDC2Keybinder.Core.Models.DefaultProfile.ActionMaps;
using KDC2Keybinder.Core.Models.DefaultProfile.Controller;
using KDC2Keybinder.Core.Models.DefaultProfile.Filter;
using KDC2Keybinder.Core.Models.DefaultProfile.Platform;
using KDC2Keybinder.Core.Models.DefaultProfile.Prios;
using System.Xml.Linq;

namespace KDC2Keybinder.Core.Utils
{
	public static class ProfileParser
	{
		public static Profile Parse(XDocument doc)
		{
			var root = doc.Root ?? throw new InvalidOperationException("Missing root");

			return new Profile
			{
				Priorities = ParsePriorities(root),
				Platforms = ParsePlatforms(root),
				ActionMaps = ParseActionMaps(root),
				ActionFilter = ParseActionFilters(root),
				Controllerlayouts = ParseControllerLayouts(root)
			};
		}

		private static Priorities? ParsePriorities(XElement root)
		{
			var prioritiesNode = root.Element("priorities");
			if (prioritiesNode == null)
				return null;

			return new Priorities
			{
				PriorityList = prioritiesNode.Elements("priority")
					.Select(p => new Priority
					{
						Name = (string)p.Attribute("name")!,
						Value = (string)p.Attribute("value")!
					})
					.ToList()
			};
		}

		private static Controllerlayouts? ParseControllerLayouts(XElement root)
		{
			var layoutsNode = root.Element("controllerlayouts");
			if (layoutsNode == null)
				return null;

			return new Controllerlayouts
			{
				Layout = layoutsNode.Elements("layout")
					.Select(l => new Layout
					{
						Name = (string)l.Attribute("name")!,
						File = (string)l.Attribute("file")!
					})
					.ToList()
			};
		}


		private static Platforms ParsePlatforms(XElement root)
		{
			var platformsNode = root.Element("platforms");
			if (platformsNode == null)
			{
				return null!;
			}

			var platforms = new List<PlatformBase>();

			foreach (var node in platformsNode.Elements())
			{
				PlatformBase platform = node.Name.LocalName switch
				{
					"PC" => new PC
					{
						Keyboard = (string?)node.Attribute("keyboard") ?? "0",
						Xboxpad = (string?)node.Attribute("xboxpad") ?? "0",
						Pspad = (string?)node.Attribute("pspad") ?? "0"
					},
					"XboxOne" => new Xbox
					{
						Keyboard = (string?)node.Attribute("keyboard") ?? "0",
						Xboxpad = (string?)node.Attribute("xboxpad") ?? "0",
						Pspad = (string?)node.Attribute("pspad") ?? "0"
					},
					"PS4" => new PS4
					{
						Keyboard = (string?)node.Attribute("keyboard") ?? "0",
						Xboxpad = (string?)node.Attribute("xboxpad") ?? "0",
						Pspad = (string?)node.Attribute("pspad") ?? "0"
					},
					_ => throw new InvalidOperationException($"Unknown platform: {node.Name}")
				};

				platforms.Add(platform);
			}

			return new Platforms() { PlatformList = platforms };
		}


		private static List<ActionMap> ParseActionMaps(XElement root)
		{
			return root.Elements("actionmap")
				.Select(ParseActionMap)
				.ToList();
		}

		private static ActionMap ParseActionMap(XElement map)
		{
			return new ActionMap
			{
				Name = (string)map.Attribute("name")!,
				Priority = (string?)map.Attribute("priority") ?? "default",
				Exclusivity = (string?)map.Attribute("exclusivity") ?? "0",

				Actions = map.Elements("action")
					.Select(ParseAction)
					.ToList(),

				Includes = map.Elements("include")
					.Select(i => new IncludeActionMap
					{
						ActionMap = (string)i.Attribute("actionmap")!
					})
					.ToList()
			};
		}

		private static ActionElement ParseAction(XElement action)
		{
			return new ActionElement
			{
				Name = (string)action.Attribute("name")!,

				OnPress = (string?)action.Attribute("onPress"),
				OnRelease = (string?)action.Attribute("onRelease"),
				OnHold = (string?)action.Attribute("onHold"),
				NoModifiers = (string?)action.Attribute("noModifiers"),
				Retriggerable = (string?)action.Attribute("retriggerable"),

				Keyboard = (string?)action.Attribute("keyboard"),
				Xboxpad = (string?)action.Attribute("xboxpad"),
				Pspad = (string?)action.Attribute("pspad"),

				HoldTriggerDelay = (string?)action.Attribute("holdTriggerDelay"),
				HoldRepeatDelay = (string?)action.Attribute("holdRepeatDelay"),

				XBoxPad = ParseXBoxpad(action.Element("xboxpad")),
				PspPad = ParsePspPad(action.Element("pspad"))
			};
		}

		private static XBoxPad? ParseXBoxpad(XElement? node)
		{
			if (node == null)
			{
				return null;
			}

			return new XBoxPad
			{
				InputData = node.Elements("inputdata")
					.Select(d => new InputData
					{
						Input = (string)d.Attribute("input")!,
						UseAnalogCompare = (string?)d.Attribute("useAnalogCompare"),
						AnalogCompareVal = (string?)d.Attribute("analogCompareVal"),
						AnalogCompareOp = (string?)d.Attribute("analogCompareOp")
					})
					.ToList()
			};
		}

		private static PspPad? ParsePspPad(XElement? node)
		{
			if (node == null)
			{
				return null;
			}

			return new PspPad
			{
				InputData = node.Elements("inputdata")
					.Select(d => new InputData
					{
						Input = (string)d.Attribute("input")!,
						UseAnalogCompare = (string?)d.Attribute("useAnalogCompare"),
						AnalogCompareVal = (string?)d.Attribute("analogCompareVal"),
						AnalogCompareOp = (string?)d.Attribute("analogCompareOp")
					})
					.ToList()
			};
		}

		private static List<ActionFilter> ParseActionFilters(XElement root)
		{
			return root.Elements("actionfilter")
				.Select(f => new ActionFilter
				{
					Name = (string)f.Attribute("name")!,
					Actions = f.Elements("action")
						.Select(a => new FilterAction
						{
							Name = (string)a.Attribute("name")!
						})
						.ToList()
				})
				.ToList();
		}


		public static XDocument BuildDefaultProfileXml(MergedKeybindStore mergedStore)
		{
			var profile = mergedStore.VanillaProfile;
			var root = new XElement("profile", new XAttribute("version", "0"));

			if (profile.Priorities != null)
			{
				var prioritiesEl = new XElement("priorities");
				foreach (var p in profile.Priorities.PriorityList)
				{
					prioritiesEl.Add(new XElement("priority",
						new XAttribute("name", p.Name),
						new XAttribute("value", p.Value)
					));
				}
				root.Add(prioritiesEl);
			}

			if (profile.Platforms != null)
			{
				var platformsEl = new XElement("platforms");
				foreach (var pf in profile.Platforms.PlatformList)
				{
					string nodeName = pf switch
					{
						PC => "PC",
						Xbox => "XboxOne",
						PS4 => "PS4",
						_ => "Unknown"
					};

					platformsEl.Add(new XElement(nodeName,
						new XAttribute("keyboard", pf.Keyboard),
						new XAttribute("xboxpad", pf.Xboxpad),
						new XAttribute("pspad", pf.Pspad)
					));
				}
				root.Add(platformsEl);
			}

			foreach (var am in mergedStore.ActionMaps)
			{
				var mapEl = new XElement("actionmap",
					new XAttribute("name", am.Name),
					new XAttribute("priority", am.Priority),
					new XAttribute("exclusivity", am.Exclusivity)
				);

				foreach (var a in am.Actions)
				{
					var actionEl = new XElement("action",
						new XAttribute("name", a.Name)
					);

					if (!string.IsNullOrEmpty(a.OnPress)) actionEl.SetAttributeValue("onPress", a.OnPress);
					if (!string.IsNullOrEmpty(a.OnRelease)) actionEl.SetAttributeValue("onRelease", a.OnRelease);
					if (!string.IsNullOrEmpty(a.OnHold)) actionEl.SetAttributeValue("onHold", a.OnHold);
					if (!string.IsNullOrEmpty(a.Retriggerable)) actionEl.SetAttributeValue("retriggerable", a.Retriggerable);
					if (!string.IsNullOrEmpty(a.NoModifiers)) actionEl.SetAttributeValue("noModifiers", a.NoModifiers);
					if (!string.IsNullOrEmpty(a.HoldTriggerDelay)) actionEl.SetAttributeValue("holdTriggerDelay", a.HoldTriggerDelay);
					if (!string.IsNullOrEmpty(a.HoldRepeatDelay)) actionEl.SetAttributeValue("holdRepeatDelay", a.HoldRepeatDelay);
					if (!string.IsNullOrEmpty(a.Keyboard)) actionEl.SetAttributeValue("keyboard", a.Keyboard);
					if (!string.IsNullOrEmpty(a.Xboxpad)) actionEl.SetAttributeValue("xboxpad", a.Xboxpad);
					if (!string.IsNullOrEmpty(a.Pspad)) actionEl.SetAttributeValue("pspad", a.Pspad);

					if (a.XBoxPad?.InputData != null && a.XBoxPad.InputData.Any())
					{
						var xboxEl = new XElement("xboxpad");
						foreach (var input in a.XBoxPad.InputData)
						{
							var inputEl = new XElement("inputdata", new XAttribute("input", input.Input));
							if (!string.IsNullOrEmpty(input.UseAnalogCompare)) inputEl.SetAttributeValue("useAnalogCompare", input.UseAnalogCompare);
							if (!string.IsNullOrEmpty(input.AnalogCompareVal)) inputEl.SetAttributeValue("analogCompareVal", input.AnalogCompareVal);
							if (!string.IsNullOrEmpty(input.AnalogCompareOp)) inputEl.SetAttributeValue("analogCompareOp", input.AnalogCompareOp);
							xboxEl.Add(inputEl);
						}
						actionEl.Add(xboxEl);
					}

					if (a.PspPad?.InputData != null && a.PspPad.InputData.Any())
					{
						var pspEl = new XElement("pspad");
						foreach (var input in a.PspPad.InputData)
						{
							var inputEl = new XElement("inputdata", new XAttribute("input", input.Input));
							if (!string.IsNullOrEmpty(input.UseAnalogCompare)) inputEl.SetAttributeValue("useAnalogCompare", input.UseAnalogCompare);
							if (!string.IsNullOrEmpty(input.AnalogCompareVal)) inputEl.SetAttributeValue("analogCompareVal", input.AnalogCompareVal);
							if (!string.IsNullOrEmpty(input.AnalogCompareOp)) inputEl.SetAttributeValue("analogCompareOp", input.AnalogCompareOp);
							pspEl.Add(inputEl);
						}
						actionEl.Add(pspEl);
					}

					mapEl.Add(actionEl);
				}

				foreach (var inc in am.Includes)
					mapEl.Add(new XElement("include", new XAttribute("actionmap", inc.ActionMap)));

				root.Add(mapEl);
			}

			foreach (var af in profile.ActionFilter)
			{
				var filterEl = new XElement("actionfilter", new XAttribute("name", af.Name));
				foreach (var a in af.Actions)
					filterEl.Add(new XElement("action", new XAttribute("name", a.Name)));
				root.Add(filterEl);
			}

			if (profile.Controllerlayouts != null)
			{
				var layoutsEl = new XElement("controllerlayouts");
				foreach (var l in profile.Controllerlayouts.Layout)
					layoutsEl.Add(new XElement("layout",
						new XAttribute("name", l.Name),
						new XAttribute("file", l.File)
					));
				root.Add(layoutsEl);
			}

			return new XDocument(new XDeclaration("1.0", "utf-8", "yes"), root);
		}

	}
}
