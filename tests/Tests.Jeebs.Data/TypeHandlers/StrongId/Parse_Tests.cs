﻿// Jeebs Unit Tests
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using Xunit;

namespace Jeebs.Data.TypeHandlers.StrongId_Tests
{
	public class Parse_Tests
	{
		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData(true)]
		[InlineData("something wrong")]
		public void Null_Or_Empty_Or_Invalid_Returns_Default(object input)
		{
			// Arrange
			var handler = new StrongIdTypeHandler<TestId>();

			// Act
			var result = handler.Parse(input);

			// Assert
			Assert.True(result == 0);
		}

		[Theory]
		[InlineData(42)]
		[InlineData("42")]
		[InlineData(" 42 ")]
		public void Valid_Number_Returns_StrongId(object input)
		{
			// Arrange
			var handler = new StrongIdTypeHandler<TestId>();

			// Act
			var result = handler.Parse(input);

			// Assert
			Assert.True(result == 42);
		}

		public sealed record TestId() : StrongId(0);
	}
}
