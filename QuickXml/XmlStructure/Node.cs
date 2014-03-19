using System.Collections.Generic;
using System.Linq;
using QuickXml.UnderTheHood;

namespace QuickXml.XmlStructure
{
	public class Node : Item
	{
		public string Name;
		public IEnumerable<Item> Children;
		public Dictionary<string, string> Attributes;
	}
}