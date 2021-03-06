﻿// Jeebs Unit Tests
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using System;
using System.Collections.Generic;
using Jeebs;
using NSubstitute;
using Xunit;
using static F.OptionF;

namespace Jeebs_Tests.Enumerable
{
	public abstract class Filter_Tests
	{
		public abstract void Test00_Maps_And_Returns_Only_Some_From_List();

		protected static void Test00(Func<IEnumerable<Option<int>>, IEnumerable<int>> act)
		{
			// Arrange
			var v0 = F.Rnd.Int;
			var v1 = F.Rnd.Int;
			var o0 = Return(v0);
			var o1 = Return(v1);
			var o2 = Create.None<int>();
			var o3 = Create.None<int>();
			var list = new[] { o0, o1, o2, o3 };

			// Act
			var result = act(list);

			// Assert
			Assert.Collection(result,
				x => Assert.Equal(v0, x),
				x => Assert.Equal(v1, x)
			);
		}

		public abstract void Test01_Maps_And_Returns_Matching_Some_From_List();

		protected static void Test01(Func<IEnumerable<Option<int>>, Func<int, bool>, IEnumerable<int>> act)
		{
			// Arrange
			var v0 = F.Rnd.Int;
			var v1 = F.Rnd.Int;
			var o0 = Return(v0);
			var o1 = Return(v1);
			var o2 = Create.None<int>();
			var o3 = Create.None<int>();
			var list = new[] { o0, o1, o2, o3 };
			var predicate = Substitute.For<Func<int, bool>>();
			predicate.Invoke(v1).Returns(true);

			// Act
			var result = act(list, predicate);

			// Assert
			Assert.Collection(result,
				x => Assert.Equal(v1, x)
			);
		}
	}
}
