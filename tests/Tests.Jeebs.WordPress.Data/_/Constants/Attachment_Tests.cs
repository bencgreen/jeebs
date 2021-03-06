﻿// Jeebs Unit Tests
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using Xunit;

namespace Jeebs.WordPress.Data.Constants_Tests
{
	public class Attachment_Tests
	{
		[Fact]
		public void Returns_Correct_Value()
		{
			// Arrange
			var expected = "_wp_attached_file";

			// Act
			var result = Constants.Attachment;

			// Assert
			Assert.Equal(expected, result);
		}
	}
}
