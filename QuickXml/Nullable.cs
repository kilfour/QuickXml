using QuickXml.UnderTheHood;

namespace QuickXml
{
	public static partial class XmlParse
	{
		public static XmlParser<T?> Nullable<T>(this XmlParser<T> parser) where T : struct
		{
			return state =>
			       	{
			       		var result = parser(state);
			       		if (result.WasSuccessFull)
			       			return Result.Success<T?>(result.Value, state);
			       		return Result.Success<T?>(null, state);
			       	};
		}
	}
}
