﻿using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Jeebs.R_Tests.WithState
{
	public class False_Tests : IR_False
	{
		[Fact]
		public void Returns_Error_Bool()
		{
			// Arrange
			const int state = 7;
			var r = Result.Ok(state);

			// Act
			var f = r.False();

			// Assert
			Assert.IsAssignableFrom<IError<bool, int>>(f);
		}

		[Fact]
		public void With_Message_Returns_Error_With_Msg()
		{
			// Arrange
			const int state = 7;
			var r = Result.Ok(state);
			var msg = new MsgTest();

			// Act
			var f = r.False(msg);

			// Assert
			Assert.IsAssignableFrom<IError<bool, int>>(f);
			Assert.True(f.Messages.Contains<MsgTest>());
		}

		public class MsgTest : IMsg { }
	}
}