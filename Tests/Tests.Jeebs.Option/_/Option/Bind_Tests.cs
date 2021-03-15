﻿// Jeebs Unit Tests
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using System;
using Jeebs.Exceptions;
using NSubstitute;
using Xunit;
using static F.OptionF;
using static F.OptionF.Msg;

namespace Jeebs.Option_Tests
{
	public class Bind_Tests
	{
		[Fact]
		public void If_Unknown_Option_Returns_None_With_UnhandledExceptionMsg()
		{
			// Arrange
			var option = new FakeOption();
			var bind = Substitute.For<Func<int, Option<string>>>();

			// Act
			var result = option.Bind(bind);

			// Assert
			var none = result.AssertNone();
			var msg = Assert.IsType<UnhandledExceptionMsg>(none);
			Assert.IsType<UnknownOptionException>(msg.Exception);
		}

		[Fact]
		public void Exception_Thrown_Returns_None_With_UnhandledExceptionMsg()
		{
			// Arrange
			var option = Return(F.Rnd.Str);
			var exception = new Exception();

			// Act
			var result = option.Bind<int>(_ => throw exception);

			// Assert
			var msg = result.AssertNone();
			Assert.IsType<UnhandledExceptionMsg>(msg);
		}

		[Fact]
		public void If_None_Gets_None()
		{
			// Arrange
			var option = None<int>(true);
			var bind = Substitute.For<Func<int, Option<string>>>();

			// Act
			var result = option.Bind(bind);

			// Assert
			result.AssertNone();
		}

		[Fact]
		public void If_None_With_Reason_Gets_None_With_Same_Reason()
		{
			// Arrange
			var msg = new TestMsg();
			var option = None<int>(msg);
			var bind = Substitute.For<Func<int, Option<string>>>();

			// Act
			var result = option.Bind(bind);

			// Assert
			var none = result.AssertNone();
			Assert.Same(msg, none);
		}

		[Fact]
		public void If_Some_Runs_Bind_Function()
		{
			// Arrange
			var value = F.Rnd.Int;
			var option = Return(value);
			var bind = Substitute.For<Func<int, Option<string>>>();

			// Act
			option.Bind(bind);

			// Assert
			bind.Received().Invoke(value);
		}

		public class FakeOption : Option<int> { }

		public record TestMsg : IMsg { }
	}
}
