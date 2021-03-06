﻿// Jeebs Unit Tests
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using System;
using System.Threading.Tasks;
using Jeebs;
using NSubstitute;
using Xunit;
using static F.OptionF;

namespace Jeebs_Tests
{
	public abstract class IfSomeAsync_Tests
	{
		public abstract Task Test00_Exception_In_IfSome_Func_Returns_None_With_UnhandledExceptionMsg();

		protected static async Task Test00(Func<Option<int>, Func<int, Task>, Task<Option<int>>> act)
		{
			// Arrange
			var option = Return(F.Rnd.Int);
			static Task ifSome(int _) => throw new Exception();

			// Act
			var result = await act(option, ifSome);

			// Assert
			var none = result.AssertNone();
			Assert.IsType<Msg.UnhandledExceptionMsg>(none);
		}

		public abstract Task Test01_None_Returns_Original_Option();

		protected static async Task Test01(Func<Option<int>, Func<int, Task>, Task<Option<int>>> act)
		{
			// Arrange
			var option = Create.None<int>();
			var ifSome = Substitute.For<Func<int, Task>>();

			// Act
			var result = await act(option, ifSome);

			// Assert
			Assert.Same(option, result);
			await ifSome.DidNotReceiveWithAnyArgs().Invoke(default);
		}

		public abstract Task Test02_Some_Runs_IfSome_Func_And_Returns_Original_Option();

		protected static async Task Test02(Func<Option<int>, Func<int, Task>, Task<Option<int>>> act)
		{
			// Arrange
			var value = F.Rnd.Int;
			var option = Return(value);
			var ifSome = Substitute.For<Func<int, Task>>();

			// Act
			var result = await act(option, ifSome);

			// Assert
			Assert.Same(option, result);
			await ifSome.Received().Invoke(value);
		}
	}
}
