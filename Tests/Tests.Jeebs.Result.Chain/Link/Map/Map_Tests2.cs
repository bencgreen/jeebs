﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Jeebs.LinkTests
{
	public partial class Map_Tests
	{
		[Fact]
		public void IOk_ValueType_Input_When_IOk_Maps_To_Next_Type()
		{
			// Arrange
			var chain = Chain.Create();
			static IR<string> f(IOk<bool> r) => r.Ok<string>();

			// Act
			var next = chain.Link().Map(f);

			// Assert
			Assert.IsAssignableFrom<IOk<string>>(next);
		}

		[Fact]
		public void IOk_ValueType_Input_When_IOk_Catches_Exception()
		{
			// Arrange
			var chain = Chain.Create();
			const string error = "Error!";
			static IR<string> f(IOk<bool> _) => throw new Exception(error);

			// Act
			var next = chain.Link().Map(f);
			var msg = next.Messages.Get<Jm.Link.LinkExceptionMsg>();

			// Assert
			Assert.IsAssignableFrom<IError<string>>(next);
			Assert.NotEmpty(msg);
			Assert.Equal($"{typeof(Exception)}: {error}", msg.Single().ToString());
		}

		[Fact]
		public void IOk_ValueType_Input_When_IError_Returns_IError()
		{
			// Arrange
			var error = Chain.Create().Error();
			static IR<int> f(IOk<bool> _) => throw new Exception();

			// Act
			var next = error.Link().Map(f);

			// Assert
			Assert.IsAssignableFrom<IError<int>>(next);
			Assert.Same(error.Messages, next.Messages);
		}
	}
}
