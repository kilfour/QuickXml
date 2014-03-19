using QuickXml.Speak;
using QuickXml.UnderTheHood;

namespace QuickXml
{
	public static partial class XmlParse
	{
		public static XmlParser<Node> Root()
		{
			return
				state =>
					{
						state.Current = state.Document.Root;
						return Result.Success(state.Current, state);
					};
		}
	}
}
