namespace QuickXml.UnderTheHood
{
	public class XmlParseNullNode : XmlParseNode
	{
		public XmlParseNullNode() 
			: base(null) { }

		public override XmlParser<string> Attribute(string attributeName)
		{
			return Result.Failure<string>;
		}

		public override XmlParser<XmlParseNode> Child(string tagName)
		{
			return state => Result.Success(this, state);
		}

		public override XmlParser<T> Apply<T>(XmlParser<T> parser)
		{
			return Result.Failure<T>;
		}

		public override XmlParserResult<string> GetContent(XmlParserState state)
		{
			return Result.Failure<string>(state);
		}
	}
}