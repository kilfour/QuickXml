using QuickXml.UnderTheHood;

namespace QuickXml.Speak
{
	public class NullNode : Node
	{
		public NullNode() 
			: base(null) { }

		public override XmlParser<string> Attribute(string attributeName)
		{
			return Result.Failure<string>;
		}

		public override XmlParser<Node> Child(string tagName)
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