using System;
using System.Collections.Generic;
using System.Linq;
using QuickXml.XmlStructure;
using Sprache;

namespace QuickXml.Speak
{
	public static class DocumentParser
	{
	    private static readonly Parser<string> WhiteSpace =
	        from ws in Parse.Char(Char.IsWhiteSpace, "").Many()
            select ws.ToString();

        private static readonly Parser<string> Identifier =
			from first in Parse.Letter.Once()
			from rest in Parse.LetterOrDigit.XOr(Parse.Char('-')).XOr(Parse.Char('_')).XOr(Parse.Char(':')).Many()
			select new string(first.Concat(rest).ToArray());

		private static Parser<KeyValuePair<string, string>> Attribute(char quote)
		{
			return
 				from attr in Identifier.Token()
				from eq in Parse.Char('=').Token()
				from lq in Parse.Char(quote)
				from value in Parse.AnyChar.Except(Parse.Char(quote)).Many().Text()
				from rq in Parse.Char(quote)
				select new KeyValuePair<string, string>(attr, value);
		}

	    private static Parser<string> NamedAttributeWithQuote(string name, char quote)
	    {
	        return
	            from attr in Identifier.Token()
                from eq in Parse.Char('=').Token()
	            from lq in Parse.Char(quote)
	            from value in Parse.AnyChar.Except(Parse.Char(quote)).Many().Text()
	            from rq in Parse.Char(quote)
	            select value;
	    }

        private static Parser<string> NamedAttribute(string name)
        {
            return NamedAttributeWithQuote(name, '"');//.Or(NamedAttributeWithQuote(name, '\''));
        }

        private static readonly Parser<KeyValuePair<string, string>> DoubleQuotedAttribute = Attribute('"');
		private static readonly Parser<KeyValuePair<string, string>> SingleQuotedAttribute = Attribute('\'');

		private static readonly Parser<Dictionary<string, string>> Attributes =
			from attributes in DoubleQuotedAttribute.Or(SingleQuotedAttribute).Many()
			select attributes.ToDictionary(kv => kv.Key, kv => kv.Value);

		private static Parser<string> EndTag(string name)
		{
			return
				from lt in Parse.Char('<').Token()
				from slash in Parse.Char('/').Token()
				from id in Identifier where id == name
				from chars in WhiteSpace
                from gt in Parse.Char('>')
				select id;
		}

        private static readonly Parser<string> Comment =
            from left in Parse.String("<!--")
            from chars in Parse.AnyChar.Except(Parse.String("-->")).Many()
            from right in Parse.String("-->")
            select string.Empty;

        private static readonly Parser<Content> CDataContent =
            from left in Parse.String("<![CDATA[")
            from chars in Parse.AnyChar.Except(Parse.String("]]>")).Many()
            from right in Parse.String("]]>")
            select new Content { Text = new string(chars.ToArray()), IsCData = true };

		private static readonly Parser<Content> Content =
			from chars in Parse.CharExcept('<').Many()
			select new Content { Text = new string(chars.ToArray()), IsCData = false };

		private static readonly Parser<Node> FullNode =
            from lt in Parse.Char('<').Token()
			from tag in Identifier
			from attributes in Attributes
            from chars in WhiteSpace
            from gt in Parse.Char('>')
			from children in Parse.Ref(() => Item).Many()
			from end in EndTag(tag)
			select
				new Node
					{
						Name = tag,
						Attributes = attributes,
						Children = children
					};

		private static readonly Parser<Node> ShortNode =
			from lt in Parse.Char('<').Token()
			from tag in Identifier
			from attributes in Attributes
			from slash in Parse.Char('/').Token()
			from gt in Parse.Char('>').Token()
			select
				new Node
					{
						Name = tag,
						Attributes = attributes,
                        Children = new Item[0]
					};

	    private static readonly Parser<Node> Node =
            from comment in Comment.Optional()
	        from node in ShortNode.Or(FullNode)
	        select node;

        private static readonly Parser<Item> Item = CDataContent.Or<Item>(Node).XOr<Item>(Content);

		private static readonly Parser<Header> Header =
			from begin in Parse.String("<?xml")
            from version in NamedAttribute("version")
			from encoding in NamedAttribute("encoding").Optional()
            from standalone in NamedAttribute("standalone").Optional()
            from content in Parse.AnyChar.Except(Parse.String("?>")).Many().Text()
			from end in Parse.String("?>")
			select new Header
			{
                Version = version,
                Encoding = encoding.GetOrDefault(),
                StandAlone = standalone.GetOrDefault()
            };
		
		public static readonly Parser<Document> Document =
			from header in Header.Optional()
			from root in Node.End()
			select
				new Document
					{
                        Header = header.GetOrDefault(),
						Root = root
					};
	}
}