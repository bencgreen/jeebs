﻿// Jeebs Unit Tests
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using Xunit;

namespace Jeebs.Constants.BibleBooks_Tests
{
	public class History_Tests
	{
		[Fact]
		public void Returns_History_Books()
		{
			// Arrange
			const string? history = "[\"Joshua\",\"Judges\",\"Ruth\",\"1 Samuel\",\"2 Samuel\",\"1 Kings\",\"2 Kings\",\"1 Chronicles\",\"2 Chronicles\",\"Ezra\",\"Nehemiah\",\"Esther\"]";

			// Act
			var result = JeebsF.JsonF.Serialise(BibleBooks.History);

			// Assert
			Assert.Equal(history, result);
		}
	}
}
