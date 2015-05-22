using System;

namespace QuickXml.UnderTheHood
{
    public class XmlParserOptionalNode : XmlParserNode
    {
        public XmlParserOptionalNode()
            : base(null) { }

        public override XmlParser<string> Attribute(string attributeName)
        {
            return state => Result.Success<string>(null, state);
        }

        public override XmlParser<XmlParserNode> Child(string tagName)
        {
            return state => Result.Success<XmlParserNode>(new XmlParserOptionalNode(), state).WithOption(true);
        }

        public override XmlParser<T> Apply<T>(XmlParser<T> parser)
        {
            return state => Result.Success(default(T), state).WithOption(true);
        }

        public override XmlParserResult<string> GetContent(XmlParserState state)
        {
            var result = Result.Success<string>(null, state);
            result.IsOption = true;
            return result;
        }
    }
}