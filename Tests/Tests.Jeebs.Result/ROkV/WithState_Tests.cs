﻿using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Jeebs.ROkV_Tests
{
	public class WithState_Tests : IOkV_WithState
	{
		[Fact]
		public void Returns_OkV_With_State()
		{
			// Arrange
			const int value = 18;
			const int state = 7;
			var r = Result.OkV(value);

			// Act
			var next = r.WithState(state);

			// Assert
			Assert.IsAssignableFrom<IOk<int, int>>(next);
			Assert.Equal(value, next.Value);
			Assert.Equal(state, next.State);
		}

		[Fact]
		public void Returns_OkV_With_State_And_Keeps_Messages()
		{
			// Arrange
			const int value = 18;
			const int state = 7;
			var r = Result.OkV(value);
			r.AddMsg(new StringMsg("Test message."));

			// Act
			var next = r.WithState(state);

			// Assert
			Assert.True(next.Messages.Contains<StringMsg>());
			Assert.Equal(value, next.Value);
		}

		public class StringMsg : Jm.WithValueMsg<string>
		{
			public StringMsg(string value) : base(value) { }
		}
	}
}