using QuickXml.UnderTheHood;

namespace QuickXml
{
	public static partial class XmlParse
	{
		public static XmlParser<decimal> Decimal(this XmlParser<string> parser)
		{
			return state => parser(state).IfSuccessfull(result => TryParseDecimal(result, state));
		}

		private static IXmlParserResult<decimal> TryParseDecimal(IXmlParserResult<string> result, XmlParserState state)
		{
			decimal value;
			return decimal.TryParse(result.Value, out value) ? Result.Success(value, state) : Result.Failure<decimal>(state);
		}
	}
}
