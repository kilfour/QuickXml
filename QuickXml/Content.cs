using System.Linq;
using QuickXml.Speak;
using QuickXml.UnderTheHood;
using QuickXml.XmlStructure;

namespace QuickXml
{
	public static partial class XmlParse
	{
		public static XmlParser<string> Content(this XmlParser<XmlParseNode> parser)
		{
			return
				state =>
					{
						var result = parser(state);
						if(result.WasSuccessFull)
						{
							return result.Value.GetContent(state);
						}
						return Result.Failure<string>(state);
					};
		}
	}
}
