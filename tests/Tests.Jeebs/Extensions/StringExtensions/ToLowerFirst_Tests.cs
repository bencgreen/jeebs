﻿// Jeebs Unit Tests
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using Xunit;

namespace Jeebs.StringExtensions_Tests
{
	public class ToLowerFirst_Tests
	{
		[Theory]
		[InlineData(null)]
		[InlineData("")]
		public void NullOrEmpty_ReturnsOriginal(string input)
		{
			// Arrange

			// Act
			var result = input.ToLowerFirst();

			// Assert
			Assert.Equal(input, result);
		}

		[Theory]
		[InlineData("Ben", "ben")]
		[InlineData("bEN", "bEN")]
		public void String_ReturnsValueWithLowercaseFirstLetter(string input, string expected)
		{
			// Arrange

			// Act
			var result = input.ToLowerFirst();

			// Assert
			Assert.Equal(expected, result);
		}
	}
}
