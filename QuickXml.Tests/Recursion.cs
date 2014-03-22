using Xunit;

namespace QuickXml.Tests
{
	public class Recursion
	{
		public class Folder
		{
			public Folder Child { get; set; }
			public int Number { get; set; }
		}

		private static readonly XmlParser<Folder> FolderParser =
			from f in XmlParse.Child("folder")
			from n in f.Attribute("number").Int()
			from c in f.Apply(FolderParser.OrNull())
			select new Folder { Number = n, Child = c };

		[Fact]
		public void OneLevel()
		{
			const string input = "<root><folder number=\"1\"></folder></root>";
			var result = FolderParser.Parse(input);

			Assert.Equal(1, result.Number);
			Assert.Null(result.Child);
		}

		[Fact]
		public void TwoLevels()
		{
			const string input = "<root><folder number=\"1\"><folder number=\"2\"></folder></folder></root>";
			var result = FolderParser.Parse(input);

			Assert.Equal(1, result.Number);
			Assert.NotNull(result.Child);

			Assert.Equal(2, result.Child.Number);
			Assert.Null(result.Child.Child);
		}

		[Fact]
		public void ThreeLevels()
		{
			const string input = "<root><folder number=\"1\"><folder number=\"2\"><folder number=\"3\"></folder></folder></folder></root>";
			var result = FolderParser.Parse(input);

			Assert.Equal(1, result.Number);
			Assert.NotNull(result.Child);

			Assert.Equal(2, result.Child.Number);
			Assert.NotNull(result.Child.Child);

			Assert.Equal(3, result.Child.Child.Number);
			Assert.Null(result.Child.Child.Child);
		}
	}
}