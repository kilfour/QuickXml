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
						var old = state.Current.SnapShot();
						state.UseNullNode = true;
						var result = parser(state);
						if (result.WasSuccessFull)
						{
							state.UseNullNode = false;
							return result;
						}
						state.Current = old.Reset();
						state.UseNullNode = false;
						return Result.Success(value, state);
					};

		}

		public static XmlParser<T> Or<T>(this XmlParser<T> parser, XmlParser<T> otherParser)
		{
			return
				state =>
					{
						var old = state.Current.SnapShot();
						state.UseNullNode = true;
						var result = parser(state);
						if (result.WasSuccessFull)
						{
							state.UseNullNode = false;
							return result;
						}
						state.Current = old.Reset();
						result = otherParser(state);
						if (result.WasSuccessFull)
						{
							state.UseNullNode = false;
							return result;
						}
						state.Current = old.Reset();
						state.UseNullNode = false;
						return Result.Failure<T>(state);
					};
		}
	}
}
