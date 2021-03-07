﻿// Jeebs Unit Tests
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using Xunit;

namespace Jeebs.Enumerated_Tests
{
	public partial class Operator_Tests
	{
		[Fact]
		public void DoesNotEqual_When_Equal_Returns_False()
		{
			// Arrange
			var value = F.Rnd.Str;
			var foo = new Foo(value);

			// Act
			var result = foo != value;

			// Assert
			Assert.False(result);
		}

		[Fact]
		public void DoesNotEqual_When_Not_Equal_Returns_True()
		{
			// Arrange
			var value = F.Rnd.Str;
			var foo = new Foo(value);
			var bar = new Bar(value);

			// Act
			var r0 = foo != string.Empty;
			var r1 = foo != bar;

			// Assert
			Assert.True(r0);
			Assert.True(r1);
		}
	}
}
