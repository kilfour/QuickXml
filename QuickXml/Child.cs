using QuickXml.UnderTheHood;
using QuickXml.XmlStructure;

namespace QuickXml
{
	public static partial class XmlParse
	{
		public static XmlParser<XmlParseNode> Child(string tagName)
		{
			return
				state =>
					{
						Node child;
						var hasChild = state.NextChild(tagName, out child);
						return hasChild ? Result.Success(new XmlParseNode(child), state) : Result.Failure<XmlParseNode>(state);
					};
		}

		public static XmlParser<XmlParseNode> Child(this XmlParser<XmlParseNode> parser, string tagName)
		{
			return state => parser(state).IfSuccessfull(result => result.Value.Child(tagName)(state));
		}
	}
}
