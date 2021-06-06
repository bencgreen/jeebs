﻿// Jeebs Unit Tests
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using System.Linq;
using Xunit;
using static Jeebs.WordPress.Data.Adapter_Tests.Adapter;

namespace Jeebs.WordPress.Data.Adapter_Tests
{
	public class Join_Tests
	{
		[Fact]
		public void No_Columns_Returns_Empty()
		{
			// Arrange
			var adapter = GetAdapter();

			// Act
			var result = adapter.Join();

			// Assert
			Assert.Equal(string.Empty, result);
		}

		[Fact]
		public void Removes_Empty_Joins_Remaining()
		{
			// Arrange
			var adapter = GetAdapter();
			var input = new[] { " one ", "  two", "", " ", "three " }.ToList();

			// Act
			var result = adapter.Join(input);

			// Assert
			Assert.Equal(" one |   two| three ", result);
		}
	}
}
