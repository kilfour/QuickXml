using QuickXml.UnderTheHood;

namespace QuickXml
{
	public static partial class XmlParse
	{
		public static XmlParser<T> OrNull<T>(this XmlParser<T> parser) where T: class
		{
			return
				state =>
					{
						var old = state.Current.SnapShot();
						state.UseNullNode = true;
						var result = parser(state);
						if (result.WasSuccessFull)
						{
							state.UseNullNode = false;
							return result;
						}
						state.Current = old.Reset();
						state.UseNullNode = false;
						return Result.Success((T)null, state);
					};

		}
	}
}
