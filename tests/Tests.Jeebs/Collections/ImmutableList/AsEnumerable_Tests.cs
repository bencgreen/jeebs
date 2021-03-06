﻿// Jeebs Unit Tests
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using System.Collections.Generic;
using Xunit;

namespace Jeebs.ImmutableList_Tests
{
	public class AsEnumerable_Tests
	{
		[Fact]
		public void Returns_Enumerable()
		{
			// Arrange
			var i0 = F.Rnd.Int;
			var i1 = F.Rnd.Int;
			var list = new ImmutableList<int>(new[] { i0, i1 });

			// Act
			var result = list.AsEnumerable();

			// Assert
			Assert.IsAssignableFrom<IEnumerable<int>>(result);
			Assert.Collection(result,
				x => Assert.Equal(i0, x),
				x => Assert.Equal(i1, x)
			);
		}

		[Fact]
		public void Returns_Copy()
		{
			// Arrange
			var i0 = F.Rnd.Str;
			var i1 = F.Rnd.Str;
			var list = new ImmutableList<string>(new[] { i0, i1 });

			// Act
			var copy = list.AsEnumerable();
			i0 = F.Rnd.Str;
			i1 = F.Rnd.Str;

			// Assert
			Assert.Collection(copy,
				x => Assert.NotEqual(i0, x),
				x => Assert.NotEqual(i1, x)
			);
		}
	}
}
