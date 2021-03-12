﻿// Jeebs Unit Tests
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using Xunit;

namespace JeebsF.OptionStatic_Tests
{
	public class False_Tests
	{
		[Fact]
		public void Returns_Some_With_Value_False()
		{
			// Arrange

			// Act
			var result = OptionF.False;

			// Assert
			var some = Assert.IsType<Some<bool>>(result);
			Assert.False(some.Value);
		}
	}
}
