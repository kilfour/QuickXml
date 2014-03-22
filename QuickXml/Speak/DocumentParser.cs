using System.Collections.Generic;
using System.Linq;
using QuickXml.XmlStructure;
using Sprache;

namespace QuickXml.Speak
{
	public static class DocumentParser
	{
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
				from gt in Parse.Char('>').Token()
				select id;
		}

		private static readonly Parser<Content> Content =
			from chars in Parse.CharExcept('<').Many()
			select new Content { Text = new string(chars.ToArray()) };

		private static readonly Parser<Node> FullNode =
			from lt in Parse.Char('<')
			from tag in Identifier
			from attributes in Attributes
			from gt in Parse.Char('>').Token()
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
						Attributes = attributes
					};

		private static readonly Parser<Node> Node = ShortNode.Or(FullNode);

		private static readonly Parser<Item> Item = Node.XOr<Item>(Content);

		private static readonly Parser<string> Header =
			from begin in Parse.String("<?")
			from content in Parse.AnyChar.Except(Parse.String("?>")).Many().Text()
			from end in Parse.String("?>")
			select content;
		
		public static readonly Parser<Document> Document =
			from header in Header.Optional()
			from root in Node.Token().End()
			select
				new Document
					{
						Root = root
					};
	}
}