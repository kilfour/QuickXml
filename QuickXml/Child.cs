using QuickXml.UnderTheHood;
using QuickXml.XmlStructure;

namespace QuickXml
{
	public static partial class XmlParse
	{
		public static XmlParser<XmlParserNode> Child(string tagName)
		{
			return
				state =>
					{
						Node child;
						var hasChild = state.NextChild(tagName, out child);
						return hasChild ? Result.Success(new XmlParserNode(child), state) : Result.Failure<XmlParserNode>(state);
					};
		}

		public static XmlParser<XmlParserNode> Child(this XmlParser<XmlParserNode> parser, string tagName)
		{
			return state => parser(state).IfSuccessfull(result => result.Value.Child(tagName)(state));
		}
	}
}
