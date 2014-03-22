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
				state.DontThrowFailures = true;
				state.UseNullNode = true;
				while (success)
				{
					var result = parser(state);
					success = result.WasSuccessFull; 
					if(success)
						list.Add(result.Value);
					state.DontThrowFailures = true;
					state.UseNullNode = true;
				}
				state.DontThrowFailures = false;
				state.UseNullNode = false;
				return Result.Success<IEnumerable<T>>(list, state);
			};
		}
	}
}
