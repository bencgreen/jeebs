using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Jeebs
{
	public sealed class EnumTests
	{
		[Fact]
		public void ToString_ReturnsName()
		{
			// Arrange
			const string input = "test";
			var test = new EnumTest(input);

			// Act
			var result = test.ToString();

			// Assert
			Assert.Equal(input, result);
		}

		[Fact]
		public void Parse_ValidString_Returns_Some()
		{
			// Arrange
			var input = EnumTest.Test1.ToString();

			// Act
			var result = EnumTest.Parse(input);

			// Assert
			var success = Assert.IsType<Some<EnumTest>>(result);
			Assert.Equal(EnumTest.Test1, success.Value);
		}

		[Fact]
		public void Parse_InvalidString_Returns_None()
		{
			// Arrange
			const string input = "test3";

			// Act
			var result = EnumTest.Parse(input);

			// Assert
			Assert.IsType<None<EnumTest>>(result);
		}
	}

	public class EnumTest : Enum
	{
		public EnumTest(string name) : base(name) { }

		public static readonly EnumTest Test1 = new EnumTest("test1");
		public static readonly EnumTest Test2 = new EnumTest("test2");

		public static Option<EnumTest> Parse(string value)
		{
			return Parse(value, new[] { Test1, Test2 });
		}
	}
}
