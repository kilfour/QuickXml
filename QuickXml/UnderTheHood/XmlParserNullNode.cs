namespace QuickXml.UnderTheHood
{
	public class XmlParserNullNode : XmlParserNode
	{
		public XmlParserNullNode() 
			: base(null) { }

		public override XmlParser<string> Attribute(string attributeName)
		{
			return Result.Failure<string>;
		}

		public override XmlParser<XmlParserNode> Child(string tagName)
		{
			return Result.Failure<XmlParserNode>;
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