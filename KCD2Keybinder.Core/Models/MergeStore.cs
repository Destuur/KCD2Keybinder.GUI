using KDC2Keybinder.Core.Models.DefaultProfile.ActionMaps;
using KDC2Keybinder.Core.Models.Superactions;

namespace KDC2Keybinder.Core.Models
{
	public class MergeStore
	{
		public Dictionary<string, List<(string ModId, object Delta)>> Changes { get; } = new();

		public void AddSuperaction(string name, string modId, Superaction delta)
		{
			if (!Changes.TryGetValue(name, out var list))
			{
				list = new List<(string, object)>();
				Changes[name] = list;
			}
			list.Add((modId, delta));
		}

		public void AddActionMap(string name, string modId, ActionMap delta)
		{
			if (!Changes.TryGetValue(name, out var list))
			{
				list = new List<(string, object)>();
				Changes[name] = list;
			}
			list.Add((modId, delta));
		}

		public bool HasConflict(string key) => Changes.TryGetValue(key, out var list) && list.Count > 1;

		public IEnumerable<string> GetConflictKeys() => Changes.Where(kvp => kvp.Value.Count > 1).Select(kvp => kvp.Key);
	}

}
