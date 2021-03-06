﻿// Jeebs Unit Tests
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using Xunit;

namespace Jeebs.StringExtensions_Tests
{
	public class ToSentenceCase_Tests
	{
		[Theory]
		[InlineData(null)]
		[InlineData("")]
		public void NullOrEmpty_ReturnsOriginal(string input)
		{
			// Arrange

			// Act
			var result = input.ToSentenceCase();

			// Assert
			Assert.Equal(input, result);
		}

		[Theory]
		[InlineData("this is a test sentence", "This is a test sentence")]
		[InlineData("testing The PHP acronym", "Testing the php acronym")]
		public void String_ReturnsValueInSentenceCase(string input, string expected)
		{
			// Arrange

			// Act
			var result = input.ToSentenceCase();

			// Assert
			Assert.Equal(expected, result);
		}
	}
}
