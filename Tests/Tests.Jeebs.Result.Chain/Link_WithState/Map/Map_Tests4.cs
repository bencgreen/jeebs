﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Jeebs.LinkTests.WithState
{
	public partial class Map_Tests
	{
		[Fact]
		public void IOk_ValueType_WithState_Input_When_IOk_Maps_To_Next_Type()
		{
			// Arrange
			const int state = 7;
			var chain = Chain.Create(state);
			static IR<string, int> f(IOk<bool, int> r) => r.Ok<string>();

			// Act
			var next = chain.Link().Map(f);

			// Assert
			var ok = Assert.IsAssignableFrom<IOk<string, int>>(next);
			Assert.Equal(state, ok.State);
		}

		[Fact]
		public void IOk_ValueType_WithState_Input_When_IOk_Catches_Exception()
		{
			// Arrange
			const int state = 7;
			var chain = Chain.Create(state);
			const string error = "Error!";
			static IR<string, int> f(IOk<bool, int> _) => throw new Exception(error);

			// Act
			var next = chain.Link().Map(f);
			var msg = next.Messages.Get<Jm.Link.LinkExceptionMsg>();

			// Assert
			var e = Assert.IsAssignableFrom<IError<string, int>>(next);
			Assert.Equal(state, e.State);
			Assert.NotEmpty(msg);
			Assert.Equal($"{typeof(Exception)}: {error}", msg.Single().ToString());
		}

		[Fact]
		public void IOk_ValueType_WithState_Input_When_IError_Returns_IError()
		{
			// Arrange
			const int state = 7;
			var error = Chain.Create(state).Error();
			static IR<int, int> f(IOk<bool, int> _) => throw new Exception();

			// Act
			var next = error.Link().Map(f);

			// Assert
			var e = Assert.IsAssignableFrom<IError<int, int>>(next);
			Assert.Equal(state, e.State);
			Assert.Same(error.Messages, next.Messages);
		}
	}
}