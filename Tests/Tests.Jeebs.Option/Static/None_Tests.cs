﻿// Jeebs Unit Tests
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using Jeebs;
using NSubstitute;
using Xunit;

namespace JeebsF.OptionStatic_Tests
{
	public class None_Tests
	{
		[Fact]
		public void Returns_None()
		{
			// Arrange

			// Act
			var result = OptionF.None<int>(true);

			// Assert
			Assert.IsType<None<int>>(result);
		}

		[Fact]
		public void Returns_None_With_Reason()
		{
			// Arrange
			var reason = Substitute.For<IMsg>();

			// Act
			var result = OptionF.None<int>(reason);

			// Assert
			var none = Assert.IsType<None<int>>(result);
			Assert.Same(reason, none.Reason);
		}
	}
}
