﻿// Jeebs Unit Tests
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using Xunit;

namespace Jeebs.Constants.BibleBooks_Tests
{
	public class Law_Tests
	{
		[Fact]
		public void Returns_Law_Books()
		{
			// Arrange
			const string? law = "[\"Genesis\",\"Exodus\",\"Leviticus\",\"Numbers\",\"Deuteronomy\"]";

			// Act
			var result = F.JsonF.Serialise(BibleBooks.Law);

			// Assert
			Assert.Equal(law, result);
		}
	}
}
