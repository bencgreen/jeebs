﻿// Jeebs Unit Tests
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using Xunit;
using static F.OptionF;

namespace Jeebs.OptionExtensions_Tests
{
	public class UnsafeUnwrap_Tests
	{
		[Fact]
		public void Some_Returns_Value()
		{
			// Arrange
			var value = F.Rnd.Int;
			var option = Return(value);

			// Act
			var result = option.UnsafeUnwrap();

			// Assert
			Assert.Equal(value, result);
		}

		[Fact]
		public void None_Throws_UnsafeUnwrapException()
		{
			// Arrange
			var option = Create.None<int>();

			// Act
			void action() => option.UnsafeUnwrap();

			// Assert
			Assert.Throws<UnsafeUnwrapException>(action);
		}
	}
}