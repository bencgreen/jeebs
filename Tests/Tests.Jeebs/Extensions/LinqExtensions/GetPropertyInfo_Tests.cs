﻿// Jeebs Unit Tests
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using System;
using System.Linq.Expressions;
using Xunit;

namespace Jeebs.LinqExtensions_Tests
{
	public class GetPropertyInfo_Tests
	{
		[Fact]
		public void Returns_PropertyInfo()
		{
			// Arrange
			Expression<Func<Test, int>> expr = t => t.MyProperty;

			// Act
			var result = expr.GetPropertyInfo();

			// Assert
			Assert.IsType<PropertyInfo<Test, int>>(result);
		}

		public class Test
		{
			public int MyProperty { get; set; }
		}
	}
}
