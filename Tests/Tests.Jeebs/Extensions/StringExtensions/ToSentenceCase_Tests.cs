﻿// Jeebs Unit Tests
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

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
