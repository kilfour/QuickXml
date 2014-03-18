using System.Collections.Generic;
using System.Linq;
using Sprache;

namespace QuickXml
{
	public static class TagParser
	{
		private static readonly Parser<string> Identifier =
			from first in Parse.Letter.Once()
			from rest in Parse.LetterOrDigit.XOr(Parse.Char('-')).XOr(Parse.Char('_')).Many()
			select new string(first.Concat(rest).ToArray());

		public static Parser<KeyValuePair<string, string>> Attribute =
				from attr in Identifier
				from eq in Parse.Char('=').Token()
				from lq in Parse.Char('"')
				from value in Parse.AnyChar.Except(Parse.Char('"')).Many().Text()
				from rq in Parse.Char('"')
				select new KeyValuePair<string, string>(attr, value);

		public static Parser<string> Tag =
			from tag in Parse.AnyChar.Except(Parse.Char('>').Or(Parse.WhiteSpace)).Many().Text().Token()
			from rest in Parse.AnyChar.Except(Parse.Char('>').Or(Parse.Char('/'))).Many()
			select tag;

		public static Parser<Node> AttributedTag =
			from tag in Parse.AnyChar.Except(Parse.Char('>').Or(Parse.WhiteSpace)).Many().Text().Token()
			from attributes in Attribute.Many()
			from rest in Parse.AnyChar.Except(Parse.Char('>').Or(Parse.Char('/'))).Many()
			select new Node(tag, attributes.ToDictionary(kv => kv.Key, kv => kv.Value));

		public static Parser<Node> Start =
			from left in Parse.Char('<').Token()
			from tag in AttributedTag
			from right in Parse.Char('>').Token()
			select tag;

		public static Parser<string> End =
			from left in Parse.Char('<').Token()
			from slash in Parse.Char('/').Token()
			from tag in Tag
			from right in Parse.Char('>').Token()
			select tag;

		public static Parser<Node> StartTag(string tagName)
		{
			return from start in Start.Where(t => t.Tag == tagName).Token() select start;
		}

		public static Parser<string> EndTag(string tagName)
		{
			return from start in End.Where(t => t == tagName).Token() select start;
		}

		public static Parser<string> Content(string tagName)
		{
			return
				from start in StartTag(tagName).Token()
				from content in Parse.AnyChar.Except(EndTag(tagName)).Many().Text().Token()
				from end in EndTag(tagName).Token()
				select content;
		}
	}
}