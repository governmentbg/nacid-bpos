using System.Collections.Generic;
using System.Linq;
using MetadataHarvesting.Models;

namespace MetadataHarvesting.Core.Services
{
	public class SetSpecService
	{
		private const char SetSpecSeparator = ':';

		public IEnumerable<string> GetSets(string setSpec)
		{
			var sets = setSpec.Split(SetSpecSeparator)
				.Select(spec => spec.Trim())
				.ToList();

			return sets;
		}

		public IList<string> GetSetNamesFromSetSpecs(Header header, IDictionary<string, string> setSpecNameMap)
		{
			return header.SetSpecs
				.Select(spec => GetSetNameFromSetSpec(spec, setSpecNameMap))
				.Distinct()
				.ToList();
		}

		private string GetSetNameFromSetSpec(string setSpec, IDictionary<string, string> setSpecNameMap)
		{
			var setSpecs = GetSets(setSpec);

			if (setSpecs.All(setSpecNameMap.ContainsKey))
			{
				var setNames = setSpecs
					.Select(s => setSpecNameMap[s])
					.ToList();

				return string.Join(SetSpecSeparator.ToString(), setNames);
			}

			if (setSpecNameMap.ContainsKey(setSpec))
			{
				var setName = setSpecNameMap[setSpec];
				if (setName.Count(c => c == SetSpecSeparator) == setSpec.Count(c => c == SetSpecSeparator))
				{
					return setName;
				}
			}

			return setSpec;
		}
	}
}
