﻿// Jeebs Unit Tests
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using Xunit;

namespace Jeebs.WordPress.Data.Constants_Tests
{
	public class FeaturedImageId_Tests
	{
		[Fact]
		public void Returns_Correct_Value()
		{
			// Arrange
			var expected = "_thumbnail_id";

			// Act
			var result = Constants.FeaturedImageId;

			// Assert
			Assert.Equal(expected, result);
		}
	}
}
