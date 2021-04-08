﻿// Jeebs Unit Tests
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using System;
using Xunit;

namespace Jeebs.PropertyInfo_Tests
{
	public class Set_Tests
	{
		[Theory]
		[InlineData(null)]
		public void Set_WithNullObject_ThrowsArgumentNullException(Foo obj)
		{
			// Arrange
			var info = new PropertyInfo<Foo, string>(nameof(Foo.Bar));

			// Act
			void result() => info.Set(obj, F.Rnd.Str);

			// Assert
			Assert.Throws<ArgumentNullException>(result);
		}

		[Theory]
		[InlineData(null)]
		public void Set_WithNullValue_ThrowsArgumentNullException(string value)
		{
			// Arrange
			var foo = new Foo();
			var info = new PropertyInfo<Foo, string>(nameof(Foo.Bar));

			// Act
			void result() => info.Set(foo, value);

			// Assert
			Assert.Throws<ArgumentNullException>(result);
		}

		[Fact]
		public void Set_WithEverythingValid_ChangesValue()
		{
			// Arrange
			var foo = new Foo { Bar = F.Rnd.Str };
			var newValue = F.Rnd.Str;
			var info = new PropertyInfo<Foo, string>(nameof(Foo.Bar));

			// Act
			info.Set(foo, newValue);

			// Assert
			Assert.Equal(newValue, foo.Bar);
		}

		public sealed class Foo
		{
			public string Bar { get; set; } = string.Empty;
		}
	}
}