using System.Collections.Generic;
using System.Linq;
using Sprache;

namespace QuickXml.Speak
{
	public static class DocumentParser
	{
		static readonly Parser<string> Identifier =
			from first in Parse.Letter.Once()
			from rest in Parse.LetterOrDigit.XOr(Parse.Char('-')).XOr(Parse.Char('_')).Many()
			select new string(first.Concat(rest).ToArray());

		public static Parser<KeyValuePair<string, string>> Attribute(char quote)
		{
			return
 				from attr in Identifier.Token()
				from eq in Parse.Char('=').Token()
				from lq in Parse.Char(quote)
				from value in Parse.AnyChar.Except(Parse.Char(quote)).Many().Text()
				from rq in Parse.Char(quote)
				select new KeyValuePair<string, string>(attr, value);
		}

		public static Parser<KeyValuePair<string, string>> DoubleQuotedAttribute = Attribute('"');
		public static Parser<KeyValuePair<string, string>> SingleQuotedAttribute = Attribute('\'');

		private static readonly Parser<Dictionary<string, string>> Attributes =
			from attributes in DoubleQuotedAttribute.Or(SingleQuotedAttribute).Many()
			select attributes.ToDictionary(kv => kv.Key, kv => kv.Value);

		static Parser<string> EndTag(string name)
		{
			return 
				from lt in Parse.Char('<')
				from slash in Parse.Char('/')
				from id in Identifier where id == name
				from gt in Parse.Char('>').Token()
					   select id;
		}

		static readonly Parser<Content> Content =
			from chars in Parse.CharExcept('<').Many()
			select new Content { Text = new string(chars.ToArray()) };

		static readonly Parser<Node> FullNode =
			from lt in Parse.Char('<')
			from tag in Identifier
			from attributes in Attributes
			from gt in Parse.Char('>').Token()
			from nodes in Parse.Ref(() => Item).Many()
			from end in EndTag(tag)
			select new Node(attributes)
			       	{
			       		Name = tag, Children = nodes
			       	};

		private static readonly Parser<Node> ShortNode =
			from lt in Parse.Char('<')
			from tag in Identifier
			from attributes in Attributes
			from slash in Parse.Char('/').Token()
			from gt in Parse.Char('>').Token()
			select new Node(attributes) { Name = tag };

		static readonly Parser<Node> Node = ShortNode.Or(FullNode);

		static readonly Parser<Item> Item = Node.Select(n => (Item)n).XOr(Content);

		public static readonly Parser<Document> Document =
			from leading in Parse.WhiteSpace.Many()
			from doc in Node.Select(n => new Document { Root = n }).End()
			select doc;
	}
}