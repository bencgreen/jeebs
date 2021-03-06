﻿// Jeebs Unit Tests
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using System;
using Xunit;

namespace Jeebs.DateTimeInt_Tests
{
	public class Constructor_Tests
	{
		[Fact]
		public void FromIntegers_CreatesObject()
		{
			// Arrange
			const string expected = "200001020304";

			// Act
			var result = new DateTimeInt(2000, 1, 2, 3, 4).ToString();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void FromDateTime_CreatesObject()
		{
			// Arrange
			const string expected = "200001020304";
			var input = new DateTime(2000, 1, 2, 3, 4, 5);

			// Act
			var result = new DateTimeInt(input).ToString();

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void FromValidString_CreatesObject()
		{
			// Arrange
			const string input = "200001020304";

			// Act
			var result = new DateTimeInt(input).ToString();

			// Assert
			Assert.Equal(input, result);
		}

		[Theory]
		[InlineData("2000")]
		[InlineData("20000102030405")]
		[InlineData("invalid")]
		public void FromInvalidString_ThrowsArgumentException(string input)
		{
			// Arrange

			// Act
			DateTimeInt result() => new(input);

			// Assert
			Assert.Throws<ArgumentException>(result);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		public void FromNullString_ReturnsZeroes(string input)
		{
			// Arrange

			// Act
			var result = new DateTimeInt(input);

			// Assert
			Assert.Equal("000000000000", result.ToString());
		}

		[Fact]
		public void FromValidLong_CreatesObject()
		{
			// Arrange
			const long input = 200001020304;

			// Act
			var result = new DateTimeInt(input).ToString();

			// Assert
			Assert.Equal(input.ToString(), result);
		}

		[Theory]
		[InlineData(2000)]
		[InlineData(20000102030405)]
		public void FromInvalidLong_ThrowsArgumentException(long input)
		{
			// Arrange

			// Act
			DateTimeInt result() => new(input);

			// Assert
			Assert.Throws<ArgumentException>(result);
		}
	}
}
