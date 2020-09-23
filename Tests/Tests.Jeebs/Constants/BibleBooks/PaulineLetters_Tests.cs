﻿using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Jeebs.Constants.BibleBooks_Tests
{
	public class PaulineLetters_Tests
	{
		[Fact]
		public void Returns_PaulineLetters()
		{
			// Arrange
			var pauline = "[\"Romans\",\"1 Corinthians\",\"2 Corinthians\",\"Galatians\",\"Ephesians\",\"Philippians\",\"Colossians\",\"1 Thessalonians\",\"2 Thessalonians\",\"1 Timothy\",\"2 Timothy\",\"Titus\",\"Philemon\"]";

			// Act
			var result = F.JsonF.Serialise(BibleBooks.PaulineLetters);

			// Assert
			Assert.Equal(pauline, result);
		}
	}
}