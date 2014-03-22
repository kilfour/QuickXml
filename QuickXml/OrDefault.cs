﻿using QuickXml.UnderTheHood;

namespace QuickXml
{
	public static partial class XmlParse
	{
		public static XmlParser<T> OrDefault<T>(this XmlParser<T> parser)
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
						return Result.Success(default(T), state);
					};
		}
	}
}
