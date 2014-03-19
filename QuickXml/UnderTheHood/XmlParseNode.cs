using System.Linq;
using QuickXml.XmlStructure;

namespace QuickXml.UnderTheHood
{
	public class XmlParseNode 
	{
		private readonly Node node;

		public XmlParseNode(Node node)
		{
			this.node = node;
		}

		public virtual XmlParser<string> Attribute(string attributeName)
		{
			return
				state =>
				{
					if (node.Attributes.ContainsKey(attributeName))
						return Result.Success(node.Attributes[attributeName].ToString(), state);
					return Result.Failure<string>(state);
				};
		}

		public virtual XmlParser<XmlParseNode> Child(string tagName)
		{
			return Wrap(XmlParse.Child(tagName));
		}

		public virtual XmlParserResult<string> GetContent(XmlParserState state)
		{
			var content = ((Content) node.Children.Single()).Text;
			return Result.Success(content, state);
		}

		public virtual XmlParser<T> Apply<T>(XmlParser<T> parser)
		{
			return
				state =>
				{
					state.Current = node;
					return parser(state);
				};
		}

		private XmlParser<T> Wrap<T>(XmlParser<T> parser)
		{
			return
				state =>
				{
					state.Current = node;
					return parser(state);
				};
		}
	}
}