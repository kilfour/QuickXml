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
						state.Current = new XmlParserNode(state.Document.Root);
						return Result.Success(state.Current, state);
					};
		}
	}
}
