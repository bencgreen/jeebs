﻿using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Jeebs.StringExtensions_Tests
{
	public class ReplaceHtmlTags_Tests
	{
		[Theory]
		[InlineData(null)]
		[InlineData("")]
		public void NullOrEmpty_ReturnsOriginal(string input)
		{
			// Arrange

			// Act
			var result = input.ReplaceHtmlTags();

			// Assert
			Assert.Equal(input, result);
		}

		[Theory]
		[InlineData("<p>Ben</p>", "Ben")]
		[InlineData("<p class=\"attr\">Ben</p>", "Ben")]
		[InlineData("<p class=\"attr\">Ben <strong>Green</strong></p>", "Ben Green")]
		public void String_ReturnsValueWithHtmlTagsReplaced(string input, string expected)
		{
			// Arrange

			// Act
			var result = input.ReplaceHtmlTags();

			// Assert
			Assert.Equal(expected, result);
		}
	}
}
