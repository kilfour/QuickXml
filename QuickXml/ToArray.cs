using System.Collections.Generic;
using System.Linq;
using QuickXml.UnderTheHood;

namespace QuickXml
{
    public static partial class XmlParse
	{
        public static XmlParser<T[]> ToArray<T>(this XmlParser<IEnumerable<T>> parser)
		{
            return state => parser(state).IfSuccessfull(result => Result.Success(result.Value.ToArray(), state));
		}
	}
}
