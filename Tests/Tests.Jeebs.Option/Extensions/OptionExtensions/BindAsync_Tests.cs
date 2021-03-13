﻿// Jeebs Unit Tests
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using System;
using System.Threading.Tasks;
using F.OptionFMsg;
using Jeebs.Option.Exceptions;
using NSubstitute;
using Xunit;
using static F.OptionF;

namespace Jeebs.OptionExtensions_Tests
{
	public class BindAsync_Tests
	{
		[Fact]
		public async Task If_Unknown_Option_Returns_None_With_UnhandledExceptionMsg()
		{
			// Arrange
			var option = new FakeOption();
			var task = option.AsTask;
			var bind = Substitute.For<Func<int, Task<Option<string>>>>();

			// Act
			var result = await OptionExtensions.DoBindAsync(task, bind, null);

			// Assert
			var none = Assert.IsType<None<string>>(result);
			var msg = Assert.IsType<UnhandledExceptionMsg>(none.Reason);
			Assert.IsType<UnknownOptionException>(msg.Exception);
		}

		[Fact]
		public async Task Exception_Thrown_Calls_Handler()
		{
			// Arrange
			var option = Return(F.Rnd.Int);
			var task = Task.FromResult(option);
			var handler = Substitute.For<Handler>();
			var exception = new Exception();

			Option<string> syncThrow(int _) => throw exception;
			Task<Option<string>> asyncThrow(int _) => throw exception;

			// Act
			var r0 = await OptionExtensions.DoBindAsync(task, asyncThrow, handler);
			var r1 = await task.BindAsync(syncThrow, handler);
			var r2 = await task.BindAsync(asyncThrow, handler);

			// Assert
			Assert.IsType<None<string>>(r0);
			Assert.IsType<None<string>>(r1);
			Assert.IsType<None<string>>(r2);
			handler.Received(3).Invoke(exception);
		}

		[Fact]
		public async Task If_None_Gets_None()
		{
			// Arrange
			var option = None<int>(true);
			var task = option.AsTask;
			var bind = Substitute.For<Func<int, Task<Option<string>>>>();

			// Act
			var r0 = await OptionExtensions.DoBindAsync(task, bind, null);
			var r1 = await task.BindAsync(v => bind(v).GetAwaiter().GetResult());
			var r2 = await task.BindAsync(bind);

			// Assert
			Assert.IsType<None<string>>(r0);
			Assert.IsType<None<string>>(r1);
			Assert.IsType<None<string>>(r2);
		}

		[Fact]
		public async Task If_None_With_Reason_Gets_None_With_Same_Reason()
		{
			// Arrange
			var msg = new TestMsg();
			var option = None<int>(msg);
			var task = option.AsTask;
			var bind = Substitute.For<Func<int, Task<Option<string>>>>();

			// Act
			var r0 = await OptionExtensions.DoBindAsync(task, bind, null);
			var r1 = await task.BindAsync(v => bind(v).GetAwaiter().GetResult());
			var r2 = await task.BindAsync(bind);

			// Assert
			var n0 = Assert.IsType<None<string>>(r0);
			Assert.Same(msg, n0.Reason);
			var n1 = Assert.IsType<None<string>>(r1);
			Assert.Same(msg, n1.Reason);
			var n2 = Assert.IsType<None<string>>(r2);
			Assert.Same(msg, n2.Reason);
		}

		[Fact]
		public async Task If_Some_Runs_Bind_Function()
		{
			// Arrange
			var value = F.Rnd.Int;
			var option = Return(value);
			var task = Task.FromResult(option);
			var bind = Substitute.For<Func<int, Task<Option<string>>>>();

			// Act
			await OptionExtensions.DoBindAsync(task, bind, null);
			await task.BindAsync(v => bind(v).GetAwaiter().GetResult(), null);
			await task.BindAsync(bind, null);

			// Assert
			await bind.Received(3).Invoke(value);
		}

		public class FakeOption : Option<int> { }

		public record TestMsg : IMsg { }
	}
}