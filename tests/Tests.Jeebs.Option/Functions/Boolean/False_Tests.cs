﻿// Jeebs Unit Tests
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using Jeebs;
using Xunit;
using static F.OptionF;

namespace F.OptionF_Tests
{
	public class False_Tests
	{
		[Fact]
		public void Returns_Some_With_Value_False()
		{
			// Arrange

			// Act
			var result = False;

			// Assert
			result.AssertFalse();
		}
	}
}
