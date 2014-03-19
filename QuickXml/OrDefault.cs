using QuickXml.UnderTheHood;

namespace QuickXml
{
	public static partial class XmlParse
	{
		public static XmlParser<T> OrDefault<T>(this XmlParser<T> parser)
		{
			return
				state =>
				{
					var result = parser(state);
					if (result.WasSuccessFull)
					{
						return result;
					}
					return Result.Success(default(T), state);
				};
		}
	}
}
