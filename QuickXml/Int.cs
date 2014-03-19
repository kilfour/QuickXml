using QuickXml.UnderTheHood;

namespace QuickXml
{
	public static partial class XmlParse
	{
		public static XmlParser<int> Int(this XmlParser<string> parser)
		{
			return
				state =>
				{
					var result = parser(state);
					if (result.WasSuccessFull)
					{
						int value;
						if (int.TryParse(result.Value, out value))
							return Result.Success(value, state);
					}
					return Result.Failure<int>(state);
				};
		}
	}
}
