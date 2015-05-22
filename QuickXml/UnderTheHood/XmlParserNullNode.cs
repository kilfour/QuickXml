namespace QuickXml.UnderTheHood
{
	public class XmlParserNullNode : XmlParserNode
	{
		public XmlParserNullNode() 
			: base(null) { }

		public override XmlParser<string> Attribute(string attributeName)
		{
            return Result.NextFailure<string>;
		}

		public override XmlParser<XmlParserNode> Child(string tagName)
		{
            return Result.NextFailure<XmlParserNode>;
		}

		public override XmlParser<T> Apply<T>(XmlParser<T> parser)
		{
            return Result.NextFailure<T>;
		}

		public override XmlParserResult<string> GetContent(XmlParserState state)
		{
            return Result.NextFailure<string>(state);
		}
	}
}