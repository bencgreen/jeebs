﻿// Jeebs Unit Tests
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using System;
using Jeebs.Linq;
using NSubstitute;
using Xunit;
using static F.OptionF.Enumerable.Msg;

namespace Jeebs.EnumerableExtensions_Tests
{
	public class LastOrNone_Tests
	{
		[Fact]
		public void Empty_List_Returns_None_With_ListIsEmptyMsg()
		{
			// Arrange
			var list = Array.Empty<int>();

			// Act
			var result = list.LastOrNone();

			// Assert
			var none = result.AssertNone();
			Assert.IsType<ListIsEmptyMsg>(none);
		}

		[Fact]
		public void No_Matching_Items_Returns_None_With_LastItemIsNullMsg()
		{
			// Arrange
			var list = new int?[] { F.Rnd.Int, F.Rnd.Int, F.Rnd.Int };
			var predicate = Substitute.For<Func<int?, bool>>();
			predicate.Invoke(Arg.Any<int?>()).Returns(false);

			// Act
			var result = list.LastOrNone(predicate);

			// Assert
			var none = result.AssertNone();
			Assert.IsType<LastItemIsNullMsg>(none);
		}

		[Fact]
		public void Returns_Last_Element()
		{
			// Arrange
			var value = F.Rnd.Int;
			var list = new[] { F.Rnd.Int, F.Rnd.Int, value };

			// Act
			var result = list.LastOrNone();

			// Assert
			var some = result.AssertSome();
			Assert.Equal(value, some);
		}

		[Fact]
		public void Returns_Last_Matching_Element()
		{
			// Arrange
			var value = F.Rnd.Int;
			var list = new[] { F.Rnd.Int, value, F.Rnd.Int };
			var predicate = Substitute.For<Func<int, bool>>();
			predicate.Invoke(value).Returns(true);

			// Act
			var result = list.LastOrNone(predicate);

			// Assert
			var some = result.AssertSome();
			Assert.Equal(value, some);
		}
	}
}