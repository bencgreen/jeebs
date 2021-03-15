﻿// Jeebs Unit Tests
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using Jeebs;
using Xunit;
using static F.OptionF;
using static F.OptionF.Msg;

namespace F.OptionF_Tests
{
	public class Return_Tests
	{
		[Theory]
		[InlineData(18)]
		[InlineData("foo")]
		public void Value_Input_Returns_Some<T>(T input)
		{
			// Arrange

			// Act
			var result = Return(input);

			// Assert
			var some = result.AssertSome();
			Assert.Equal(input, some);
		}

		[Fact]
		public void Null_Input_Returns_None()
		{
			// Arrange
			int? value = null;

			// Act
			var result = Return(value);

			// Assert
			var none = result.AssertNone();
			Assert.IsType<AllowNullWasFalseMsg>(none);
		}

		[Fact]
		public void Null_Input_Returns_Some_If_AllowNull_Is_True()
		{
			// Arrange
			int? value = null;

			// Act
			var result = Return(value, true);

			// Assert
			var some = result.AssertSome();
			Assert.Null(some);
		}

		[Fact]
		public void Null_Input_Allow_Nulls_Returns_Some_With_Null_Value()
		{
			// Arrange
			int? value = null;

			// Act
			var result = Return(value, true);

			// Assert
			var some = result.AssertSome();
			Assert.Null(some);
		}
	}
}
