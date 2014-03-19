using System;
using System.Collections.Generic;
using System.Linq;

namespace QuickXml
{
	public class Node : Item
	{
		public string Name;
		public IEnumerable<Item> Children;
		private readonly Dictionary<string, string> attributes;

		public Node(Dictionary<string, string> attributes)
		{
			this.attributes = attributes;
		}

		public XmlParser<Node> Child(string tagName)
		{
			return Wrap(XmlParse.Child(tagName));
		}

		public XmlParser<string> Content(string tagName)
		{
			return
				state =>
				{
					state.Current = this;
					Node child;
					var hasChild = state.NextChild(tagName, out child);
					if (hasChild)
						return Result.Success(((Content)child.Children.Single()).Text, state);
					return Result.Failure<string>(state);
				};
		}

		private XmlParser<T> Wrap<T>(XmlParser<T> parser)
		{
			return
				state =>
				{
					state.Current = this;
					return parser(state);
				};
		}

		public XmlParser<T> Apply<T>(XmlParser<T> parser)
		{
			return
				state =>
					{
						state.Current = this;
						return parser(state);
					};
		}

		public XmlParser<string> Attribute(string attributeName)
		{
			return
				state =>
					{
						if (attributes.ContainsKey(attributeName))
							return Result.Success(attributes[attributeName].ToString(), state);
						return Result.Failure<string>(state);
					};
		}
	}
}