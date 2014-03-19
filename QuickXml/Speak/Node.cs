using System;
using System.Collections.Generic;
using System.Linq;
using QuickXml.UnderTheHood;

namespace QuickXml.Speak
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

		public virtual XmlParser<string> Attribute(string attributeName)
		{
			return
				state =>
				{
					if (attributes.ContainsKey(attributeName))
						return Result.Success(attributes[attributeName].ToString(), state);
					return Result.Failure<string>(state);
				};
		}

		public virtual XmlParser<Node> Child(string tagName)
		{
			return Wrap(XmlParse.Child(tagName));
		}

		public virtual XmlParser<T> Apply<T>(XmlParser<T> parser)
		{
			return
				state =>
					{
						state.Current = this;
						return parser(state);
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

		public virtual XmlParserResult<string> GetContent(XmlParserState state)
		{
			var content = ((Content) Children.Single()).Text;
			return Result.Success(content, state);
		}
	}
}