﻿using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Jeebs.OptionExtensions_Tests
{
	public class SelectMany_Tests
	{
		[Fact]
		public void Linq_SelectMany_With_Some_Returns_Some()
		{
			// Arrange
			const int v0 = 18;
			const int v1 = 7;
			var o0 = Option.Some(v0);
			var o1 = Option.Some(v1);

			// Act
			var result = from a in o0
						 from b in o1
						 select a + b;

			// Assert
			var some = Assert.IsType<Some<int>>(result);
			Assert.Equal(v0 + v1, some.Value);
		}

		[Fact]
		public void Linq_SelectMany_With_None_Returns_None()
		{
			// Arrange
			const int v0 = 18;
			const int v1 = 7;
			var o0 = Option.Some(v0);
			var o1 = Option.Some(v1);
			var o2 = Option.None<int>().AddReason<InvalidIntegerMsg>();

			// Act
			var result = from a in o0
						 from b in o1
						 from c in o2
						 select a + b + c;

			// Assert
			var none = Assert.IsType<None<int>>(result);
			Assert.True(none.Reason is InvalidIntegerMsg);
		}

		public class InvalidIntegerMsg : IMsg { }
	}
}