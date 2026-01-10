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
	}
}
