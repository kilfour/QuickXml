using System;
using System.Linq;
using QuickXml.Speak;

namespace QuickXml.UnderTheHood
{
	public class XmlParserState
	{
		public Document Document { get; set; }
		
		public Node Current { get; set; }

		public string CurrentChildTag { get; set; }
		public int CurrentChildIndex { get; set; }

		public bool UseNullNode { get; set; }
		public bool DontThrowFailures { get; set; }

		public bool NextChild(string tagName, out Node node)
		{
			if (tagName != CurrentChildTag)
				CurrentChildIndex = 0;

			CurrentChildTag = tagName;

			var children =
				Current
					.Children
					.Where(c => c is Node)
					.Cast<Node>()
					.Where(n => n.Name == tagName);

			if (children.Count() <= CurrentChildIndex)
			{
				node = null;
				return false;
			}

			node = children.ElementAt(CurrentChildIndex);
			CurrentChildIndex++;
			return true;
		}
	}
}