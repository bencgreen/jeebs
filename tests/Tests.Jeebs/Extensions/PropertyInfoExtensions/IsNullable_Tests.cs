﻿// Jeebs Unit Tests
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using System.Reflection;
using Xunit;

namespace Jeebs.Reflection.PropertyInfoExtensions_Tests
{
	public class IsNullable_Tests
	{
		[Fact]
		public void Nullable_ValueType_Property_Returns_True()
		{
			// Arrange
			var info = typeof(TestValue).GetProperty(nameof(TestValue.Foo));

			// Act
			var result = info?.IsNullable();

			// Assert
			Assert.True(result);
		}

		[Fact]
		public void Nullable_ReferenceType_Property_Returns_True()
		{
			// Arrange
			var info = typeof(TestReference).GetProperty(nameof(TestReference.Foo));

			// Act
			var result = info?.IsNullable();

			// Assert
			Assert.True(result);
		}

		[Fact]
		public void Not_Nullable_ValueType_Property_Returns_False()
		{
			// Arrange
			var info = typeof(TestValue).GetProperty(nameof(TestValue.Bar));

			// Act
			var result = info?.IsNullable();

			// Assert
			Assert.False(result);
		}

		[Fact]
		public void Not_Nullable_ReferenceType_Property_Returns_False()
		{
			// Arrange
			var info = typeof(TestReference).GetProperty(nameof(TestReference.Bar));

			// Act
			var result = info?.IsNullable();

			// Assert
			Assert.False(result);
		}

		public sealed class TestValue
		{
			public int? Foo { get; set; }

			public int Bar { get; set; }
		}

		public sealed class TestReference
		{
			public TestValue? Foo { get; set; }

			public TestValue Bar { get; set; } = new();
		}
	}
}