﻿using System;
using System.Collections.Generic;
using System.Text;
using Jeebs;
using Xunit;

namespace Tests.Jeebs.Result
{
	public class ROk_WithState_True_Tests
	{
		[Fact]
		public void True_Returns_IOk_Bool()
		{
			// Arrange
			const int state = 18;
			var r = R.Ok(state);

			// Act
			var f = r.True();

			// Assert
			Assert.IsAssignableFrom<IOk<bool, int>>(f);
		}

		[Fact]
		public void False_With_Message_Returns_Error_With_Msg()
		{
			// Arrange
			const int state = 18;
			var r = R.Ok(state);
			var msg = new MsgTest();

			// Act
			var f = r.True(msg);

			// Assert
			Assert.IsAssignableFrom<IOk<bool, int>>(f);
			Assert.True(f.Messages.Contains<MsgTest>());
		}

		public class MsgTest : IMsg { }
	}
}
