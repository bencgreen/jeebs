﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace F
{
	public sealed class StringTests
	{
		[Fact]
		public void Random_ValidLength_ReturnsString()
		{
			// Arrange
			const int length = 10;

			// Act
			var result = StringF.Random(length);

			// Assert
			Assert.Equal(length, result.Length);
		}

		[Fact]
		public void Random_InvalidLength_ThrowsInvalidOperationException()
		{
			// Arrange
			const int length = 3;

			// Act
			Action result = () => StringF.Random(length, upper: true, numbers: true, special: true);

			// Assert
			Assert.Throws<InvalidOperationException>(result);
		}

		[Fact]
		public void Random_ReturnsOnlyLowercaseCharacters()
		{
			// Arrange

			// Act
			var result = StringF.Random(12, upper: false);

			// Assert
			Assert.True(result.All(c => StringF.LowercaseChars.Contains(c)));
		}

		[Fact]
		public void Random_ReturnsOnlyLowercaseAndUppercaseCharacters()
		{
			// Arrange

			// Act
			var result = StringF.Random(12, upper: true);

			// Assert
			Assert.True(result.All(c => StringF.LowercaseChars.Contains(c) || StringF.UppercaseChars.Contains(c)));
		}

		[Fact]
		public void Random_ReturnsOnlyLowercaseAndNumericCharacters()
		{
			// Arrange

			// Act
			var result = StringF.Random(12, upper: false, numbers: true);

			// Assert
			Assert.True(result.All(c => StringF.LowercaseChars.Contains(c) || StringF.NumberChars.Contains(c)));
		}

		[Fact]
		public void Random_ReturnsOnlyLowercaseAndSpecialCharacters()
		{
			// Arrange

			// Act
			var result = StringF.Random(12, upper: false, special: true);

			// Assert
			Assert.True(result.All(c => StringF.LowercaseChars.Contains(c) || StringF.SpecialChars.Contains(c)));
		}

		[Fact]
		public void Random_ReturnsStringWithAllCharacters()
		{
			// Arrange

			// Act
			var result = StringF.Random(12, upper: true, numbers: true, special: true);

			// Assert
			Assert.True(result.All(c => StringF.AllChars.Contains(c)));
		}
	}
}
