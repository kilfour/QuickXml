using System.Collections.Generic;
using System.Linq;
using QuickXml.UnderTheHood;

namespace QuickXml
{
    public static partial class XmlParse
	{
        public static XmlParser<T[]> ToArray<T>(this XmlParser<IEnumerable<T>> parser)
        {
            return
                state =>
                {
                    var result = parser(state);
                    if (result.WasSuccessFull)
                        return Result.Success(result.Value.ToArray(), state);
                    return Result.Success(new T[0], state);
                };
        }
	}
}
