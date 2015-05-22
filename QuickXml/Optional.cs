using QuickXml.UnderTheHood;

namespace QuickXml
{
    public static partial class XmlParse
    {
        public static XmlParser<T> Optional<T>(this XmlParser<T> parser)
        {
            return
                state =>
                {
                    var result = parser(state);
                    if (result.WasSuccessFull)
                    {
                        return result;
                    }
                    if (!result.NotFirstFailure)
                    {
                        if (typeof(T) == typeof(XmlParserNode))
                        {
                            var value = ((T)((object)new XmlParserOptionalNode()));
                            return Result.Success(value, state).WithOption(true);
                        }
                        return new XmlParserResult<T>(default(T), state, false, true) {IsOption = true};
                    }
                    return Result.NextFailure<T>(state);
                };

        }
    }
}
