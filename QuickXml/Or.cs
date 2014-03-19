using QuickXml.UnderTheHood;

namespace QuickXml
{
	public static partial class XmlParse
	{
		public static XmlParser<T> Or<T>(this XmlParser<T> parser, T value)
		{
			return state => parser(state).WhenFailed(result => Result.Success(value, state));
		}

		public static XmlParser<T> Or<T>(this XmlParser<T> parser, XmlParser<T> otherParser)
		{
			return
				state =>
					{
						state.UseNullNode = true;
						var result = parser(state);
						if (result.WasSuccessFull)
						{
							state.UseNullNode = false;
							return result;
						}

						result = otherParser(state);
						if (result.WasSuccessFull)
						{
							state.UseNullNode = false;
							return result;
						}
						state.UseNullNode = false;
						return Result.Failure<T>(state);
					};
		}
	}
}
