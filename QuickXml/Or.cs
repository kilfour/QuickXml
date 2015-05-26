using QuickXml.UnderTheHood;

namespace QuickXml
{
	public static partial class XmlParse
	{
		public static XmlParser<T> Or<T>(this XmlParser<T> parser, T value)
		{
			return
				state =>
					{
                        var result = parser(state);
                        if (result.IsOption)
                            return Result.Success(value, state);
					    return result;
					};

		}

		public static XmlParser<T> Or<T>(this XmlParser<T> parser, XmlParser<T> otherParser)
		{
			return
				state =>
					{
						var result = parser(state);
						if (result.WasSuccessFull)
						{
							return result;
						}
						result = otherParser(state);
						if (result.WasSuccessFull)
						{
							return result;
						}
						return Result.Failure<T>(state);
					};
		}
	}
}
