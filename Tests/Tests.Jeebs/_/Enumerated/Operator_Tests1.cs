﻿using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Jeebs.Enumerated_Tests
{
	public partial class Operator_Tests
	{
		[Fact]
		public void Equals_When_Equal_Returns_True()
		{
			// Arrange
			const string value = "foo";
			var foo = new Foo(value);

			// Act
			var result = foo == value;

			// Assert
			Assert.True(result);
		}

		[Fact]
		public void Equals_When_Not_Equal_Returns_False()
		{
			// Arrange
			const string value = "foo";
			var foo = new Foo(value);
			var bar = new Bar(value);

			// Act
			var r0 = foo == string.Empty;
			var r1 = foo == bar;

			// Assert
			Assert.False(r0);
			Assert.False(r1);
		}

		public class Bar : Enumerated
		{
			public Bar(string name) : base(name) { }
		}
	}
}