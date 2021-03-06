﻿// Jeebs Unit Tests
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using Xunit;
using static Jeebs.Reflection.ObjectExtensions.Msg;

namespace Jeebs.Reflection.ObjectExtensions_Tests
{
	public class GetPropertyValue_Tests
	{
		[Fact]
		public void Property_Does_Not_Exist_Returns_None_With_PropertyNotFoundMsg()
		{
			// Arrange
			var test = new Test(F.Rnd.Str);

			// Act
			var r0 = test.GetPropertyValue(F.Rnd.Str);
			var r1 = test.GetPropertyValue<string>(F.Rnd.Str);

			// Assert
			var n0 = r0.AssertNone();
			Assert.IsType<PropertyNotFoundMsg>(n0);
			var n1 = r1.AssertNone();
			Assert.IsType<PropertyNotFoundMsg>(n1);
		}

		[Fact]
		public void TypeParam_Is_Wrong_Type_Returns_None_With_UnexpectedPropertyTypeMsg()
		{
			// Arrange
			var test = new Test(F.Rnd.Str);

			// Act
			var result = test.GetPropertyValue<int>(nameof(Test.Foo));

			// Assert
			var none = result.AssertNone();
			Assert.IsType<UnexpectedPropertyTypeMsg<int>>(none);
		}

		[Theory]
		[InlineData(null)]
		public void Value_Is_Null_Returns_None_With_NullValueMsg(string input)
		{
			// Arrange
			var test = new Test(input);

			// Act
			var r0 = test.GetPropertyValue(nameof(Test.Foo));
			var r1 = test.GetPropertyValue<string>(nameof(Test.Foo));

			// Assert
			var n0 = r0.AssertNone();
			Assert.IsType<NullValueMsg<object>>(n0);
			var n1 = r1.AssertNone();
			Assert.IsType<NullValueMsg<string>>(n1);
		}

		[Fact]
		public void Returns_Property_Value()
		{
			// Arrange
			var value = F.Rnd.Str;
			var test = new Test(value);

			// Act
			var r0 = test.GetPropertyValue(nameof(Test.Foo));
			var r1 = test.GetPropertyValue<string>(nameof(Test.Foo));

			// Assert
			var s0 = r0.AssertSome();
			Assert.Equal(value, s0);
			var s1 = r1.AssertSome();
			Assert.Equal(value, s1);
		}

		public sealed record Test(string Foo);
	}
}
