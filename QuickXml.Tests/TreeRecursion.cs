using Xunit;

namespace QuickXml.Tests
{
	public class TreeRecursion
	{
		public class Folder
		{
			public Folder Child { get; set; }
			public int Number { get; set; }
		}

	    private static readonly XmlParser<Folder> FolderParser =
	        from folder in XmlParse.Child("folder")
	        from number in folder.Attribute("number").Int()
	        from child in folder.Apply(FolderParser.Optional())
	        select
	            new Folder
	            {
	                Number = number,
	                Child = child
	            };

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