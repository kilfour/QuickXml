﻿using QuickXml.UnderTheHood;

namespace QuickXml
{
	public delegate XmlParserResult<T> XmlParser<T>(XmlParserState state);
}