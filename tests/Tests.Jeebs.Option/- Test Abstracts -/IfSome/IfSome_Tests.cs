﻿// Jeebs Unit Tests
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using System;
using Jeebs;
using NSubstitute;
using Xunit;
using static F.OptionF;

namespace Jeebs_Tests
{
	public abstract class IfSome_Tests
	{
		public abstract void Test00_Exception_In_IfSome_Action_Returns_None_With_UnhandledExceptionMsg();

		protected static void Test00(Func<Option<int>, Action<int>, Option<int>> act)
		{
			// Arrange
			var option = Return(F.Rnd.Int);
			static void ifSome(int _) => throw new Exception();

			// Act
			var result = act(option, ifSome);

			// Assert
			var none = result.AssertNone();
			Assert.IsType<Msg.UnhandledExceptionMsg>(none);
		}

		public abstract void Test01_None_Returns_Original_Option();

		protected static void Test01(Func<Option<int>, Action<int>, Option<int>> act)
		{
			// Arrange
			var option = Create.None<int>();
			var ifSome = Substitute.For<Action<int>>();

			// Act
			var result = act(option, ifSome);

			// Assert
			Assert.Same(option, result);
			ifSome.DidNotReceiveWithAnyArgs().Invoke(default);
		}

		public abstract void Test02_Some_Runs_IfSome_Action_And_Returns_Original_Option();

		protected static void Test02(Func<Option<int>, Action<int>, Option<int>> act)
		{
			// Arrange
			var value = F.Rnd.Int;
			var option = Return(value);
			var ifSome = Substitute.For<Action<int>>();

			// Act
			var result = act(option, ifSome);

			// Assert
			Assert.Same(option, result);
			ifSome.Received().Invoke(value);
		}
	}
}
