using System.Collections.Generic;
using QuickXml.UnderTheHood;

namespace QuickXml
{
	public static partial class XmlParse
	{
		public static XmlParser<IEnumerable<T>> Many<T>(this XmlParser<T> parser)
		{
			return state =>
			{
				var list = new List<T>();
				var success = true;
				while (success)
				{
					var result = parser(state);
					list.Add(result.Value);
					success = result.WasSuccessFull;
				}
				return Result.Success<IEnumerable<T>>(list, state);
			};
		}
	}
}
