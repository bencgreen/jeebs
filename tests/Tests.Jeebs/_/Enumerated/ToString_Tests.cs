// Jeebs Unit Tests
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using Xunit;

namespace Jeebs.Enumerated_Tests
{
	public class ToString_Tests
	{
		[Fact]
		public void ReturnsName()
		{
			// Arrange
			var input = F.Rnd.Str;
			var test = new Foo(input);

			// Act
			var result = test.ToString();

			// Assert
			Assert.Equal(input, result);
		}

		public class Foo : Enumerated
		{
			public Foo(string name) : base(name) { }
		}
	}
}
