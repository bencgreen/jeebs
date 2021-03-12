﻿// Jeebs Unit Tests
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jeebs;
using NSubstitute;
using Xunit;

namespace JeebsF.OptionExtensions_Tests
{
	public class UnwrapSingleAsync_Tests
	{
		[Fact]
		public async Task If_Unknown_Option_Returns_None_With_UnhandledExceptionMsg()
		{
			// Arrange
			var option = new FakeOption();
			var task = Task.FromResult(option.AsOption);

			// Act
			var result = await task.UnwrapAsync(x => x.Single<int>());

			// Assert
			var none = Assert.IsType<None<int>>(result);
			var msg = Assert.IsType<Jm.Option.UnhandledExceptionMsg>(none.Reason);
			Assert.IsType<Exceptions.UnknownOptionException>(msg.Exception);
		}

		[Fact]
		public async Task None_Returns_None()
		{
			// Arrange
			var option = OptionF.None<int>(true);
			var task = Task.FromResult(option.AsOption);

			// Act
			var result = await task.UnwrapAsync(x => x.Single<int>());

			// Assert
			Assert.IsType<None<int>>(result);
		}

		[Fact]
		public async Task None_With_Reason_Returns_None_With_Reason()
		{
			// Arrange
			var reason = new TestMsg();
			var option = OptionF.None<int>(reason);
			var task = Task.FromResult(option.AsOption);

			// Act
			var result = await task.UnwrapAsync(x => x.Single<int>());

			// Assert
			var none = Assert.IsType<None<int>>(result);
			Assert.Same(reason, none.Reason);
		}

		[Fact]
		public async Task No_Items_Returns_None_With_UnwrapSingleNoItemsMsg()
		{
			// Arrange
			var empty = (IEnumerable<int>)Array.Empty<int>();
			var option = OptionF.Return(empty);
			var task = Task.FromResult(option);

			// Act
			var result = await task.UnwrapAsync(x => x.Single<int>());

			// Assert
			var none = Assert.IsType<None<int>>(result);
			Assert.IsType<Jm.Option.UnwrapSingleNoItemsMsg>(none.Reason);
		}

		[Fact]
		public async Task No_Items_Runs_NoItems()
		{
			// Arrange
			var empty = (IEnumerable<int>)Array.Empty<int>();
			var option = OptionF.Return(empty);
			var task = Task.FromResult(option);
			var noItems = Substitute.For<Func<IMsg>>();

			// Act
			await task.UnwrapAsync(x => x.Single<int>(noItems: noItems));

			// Assert
			noItems.Received().Invoke();
		}

		[Fact]
		public async Task Too_Many_Items_Returns_None_With_UnwrapSingleTooManyItemsErrorMsg()
		{
			// Arrange
			var list = (IEnumerable<int>)new[] { JeebsF.Rnd.Int, JeebsF.Rnd.Int };
			var option = OptionF.Return(list);
			var task = Task.FromResult(option);

			// Act
			var result = await task.UnwrapAsync(x => x.Single<int>());

			// Assert
			var none = Assert.IsType<None<int>>(result);
			Assert.IsType<Jm.Option.UnwrapSingleTooManyItemsErrorMsg>(none.Reason);
		}

		[Fact]
		public async Task Too_Many_Items_Runs_TooMany()
		{
			// Arrange
			var list = (IEnumerable<int>)new[] { JeebsF.Rnd.Int, JeebsF.Rnd.Int };
			var option = OptionF.Return(list);
			var task = Task.FromResult(option);
			var tooMany = Substitute.For<Func<IMsg>>();

			// Act
			await task.UnwrapAsync(x => x.Single<int>(tooMany: tooMany));

			// Assert
			tooMany.Received().Invoke();
		}

		[Fact]
		public async Task Not_A_List_Returns_None_With_UnwrapSingleNotAListMsg()
		{
			// Arrange
			var value = JeebsF.Rnd.Int;
			var option = OptionF.Return(value);
			var task = Task.FromResult(option);

			// Act
			var result = await task.UnwrapAsync(x => x.Single<int>());

			// Assert
			var none = Assert.IsType<None<int>>(result);
			Assert.IsType<Jm.Option.UnwrapSingleNotAListMsg>(none.Reason);
		}

		[Fact]
		public async Task Not_A_List_Runs_NotAList()
		{
			// Arrange
			var value = JeebsF.Rnd.Int;
			var option = OptionF.Return(value);
			var task = Task.FromResult(option);
			var notAList = Substitute.For<Func<IMsg>>();

			// Act
			await task.UnwrapAsync(x => x.Single<int>(notAList: notAList));

			// Assert
			notAList.Received().Invoke();
		}

		[Fact]
		public async Task List_With_Single_Item_Returns_Single()
		{
			// Arrange
			var value = JeebsF.Rnd.Int;
			var list = (IEnumerable<int>)new[] { value };
			var option = OptionF.Return(list);
			var task = Task.FromResult(option);

			// Act
			var result = await task.UnwrapAsync(x => x.Single<int>());

			// Assert
			Assert.Equal(value, result);
		}

		public class FakeOption : Option<int>
		{
			public Option<int> AsOption => this;
		}

		public record TestMsg : IMsg { }
	}
}
