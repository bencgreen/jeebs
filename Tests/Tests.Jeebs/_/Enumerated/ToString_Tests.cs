using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Jeebs.Enumerated_Tests
{
	public class ToString_Tests
	{
		[Fact]
		public void ReturnsName()
		{
			// Arrange
			const string input = "test";
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
