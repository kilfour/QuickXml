using QuickXml.UnderTheHood;

namespace QuickXml
{
	public static partial class XmlParse
	{
		public static XmlParser<decimal> Decimal(this XmlParser<string> parser)
		{
			return
				state =>
				{
					var result = parser(state);
					if (result.WasSuccessFull)
					{
						decimal value;
						if (decimal.TryParse(result.Value, out value))
							return Result.Success(value, state);
					}
					return Result.Failure<decimal>(state);
				};
		}
	}
}
