using QuickXml.UnderTheHood;

namespace QuickXml
{
	public static partial class XmlParse
	{
		public static XmlParser<XmlParseNode> Root()
		{
			return
				state =>
					{
						state.Current = state.Document.Root;
						return Result.Success(new XmlParseNode(state.Current), state);
					};
		}
	}
}
