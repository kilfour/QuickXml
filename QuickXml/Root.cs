using QuickXml.UnderTheHood;

namespace QuickXml
{
	public static partial class XmlParse
	{
		public static XmlParser<XmlParserNode> Root()
		{
			return
				state =>
					{
						state.Current = state.Document.Root;
						return Result.Success(new XmlParserNode(state.Current), state);
					};
		}
	}
}
