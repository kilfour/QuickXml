using System.Collections.Generic;

namespace QuickXml
{
	public class Node
	{
		public string Tag { get; private set; }

		private readonly Dictionary<string, string> attributes;

		public Node(string tag, Dictionary<string, string> attributes)
		{
			Tag = tag;
			this.attributes = attributes;
		}

		public string StringOrDefault(string attributeName)
		{
			if (attributes.ContainsKey(attributeName))
				return attributes[attributeName];
			return string.Empty;
		}
	}
}