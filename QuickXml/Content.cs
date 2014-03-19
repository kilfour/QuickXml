using System.Linq;
using QuickXml.Speak;
using QuickXml.UnderTheHood;

namespace QuickXml
{
	public static partial class XmlParse
	{
		public static XmlParser<string> Content(this XmlParser<Node> parser)
		{
			return
				state =>
					{
						var result = parser(state);
						if(result.WasSuccessFull)
						{
							var content = ((Content)result.Value.Children.Single()).Text;
							return Result.Success(content, state);	
						}
						return Result.Failure<string>(state);
					};
		}
	}
}
