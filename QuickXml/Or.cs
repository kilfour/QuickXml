﻿using QuickXml.UnderTheHood;

namespace QuickXml
{
	public static partial class XmlParse
	{
		public static XmlParser<T> Or<T>(this XmlParser<T> parser, T value)
		{
			return
				state =>
				{
					var result = parser(state);
					if (result.WasSuccessFull)
					{
						return result;
					}
					return Result.Success(value, state);
				};
		}
	}
}