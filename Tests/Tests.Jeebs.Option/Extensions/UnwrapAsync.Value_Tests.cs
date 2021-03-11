﻿// Jeebs Unit Tests
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using System;
using System.Threading.Tasks;
using NSubstitute;
using Xunit;

namespace Jeebs.OptionExtensions_Tests
{
	public class UnwrapAsync_Tests
	{
		[Fact]
		public async Task Some_Returns_Value()
		{
			// Arrange
			var value = F.Rnd.Int;
			var option = Option.Wrap(value);
			var task = Task.FromResult(option);

			// Act
			var r0 = await task.UnwrapAsync(x => x.Value(F.Rnd.Int));
			var r1 = await task.UnwrapAsync(x => x.Value(Substitute.For<Func<int>>()));
			var r2 = await task.UnwrapAsync(x => x.Value(Substitute.For<Func<IMsg?, int>>()));

			// Assert
			Assert.Equal(value, r0);
			Assert.Equal(value, r1);
			Assert.Equal(value, r2);
		}

		[Fact]
		public async Task None_Gets_IfNone()
		{
			// Arrange
			var value = F.Rnd.Int;
			var option = Option.None<int>(true);
			var task = Task.FromResult(option.AsOption);

			// Act
			var result = await task.UnwrapAsync(x => x.Value(value));

			// Assert
			Assert.Equal(value, result);
		}

		[Fact]
		public async Task None_Runs_IfNone()
		{
			// Arrange
			var option = Option.None<int>(true);
			var task = Task.FromResult(option.AsOption);
			var ifNone = Substitute.For<Func<IMsg?, int>>();

			// Act
			await task.UnwrapAsync(x => x.Value(() => ifNone(null)));
			await task.UnwrapAsync(x => x.Value(ifNone));

			// Assert
			ifNone.ReceivedWithAnyArgs(2).Invoke(Arg.Any<IMsg?>());
		}
	}
}
