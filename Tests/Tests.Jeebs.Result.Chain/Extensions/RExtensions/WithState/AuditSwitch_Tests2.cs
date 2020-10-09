﻿using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Jeebs.RExtensions_Tests.WithState
{
	public partial class AuditSwitch_Tests
	{
		[Fact]
		public void IOkV_Input_When_IOk_Does_Nothing()
		{
			// Arrange
			var state = F.Rand.Integer;
			var chain = Chain.Create(state);
			int sideEffect = 1;
			void a(IOkV<bool, int> _) => sideEffect++;

			// Act
			chain.AuditSwitch(isOkV: a);

			// Assert
			Assert.Equal(1, sideEffect);
		}

		[Fact]
		public void IOkV_Input_When_IOkV_Runs_Func()
		{
			// Arrange
			var value = F.Rand.Integer;
			var state = F.Rand.Integer;
			var chain = Chain.CreateV(value, state);
			int sideEffect = 1;
			void a(IOkV<int, int> _) => sideEffect++;

			// Act
			chain.AuditSwitch(isOkV: a);

			// Assert
			Assert.Equal(2, sideEffect);
		}

		[Fact]
		public void IOkV_Input_When_IError_Does_Nothing()
		{
			// Arrange
			var state = F.Rand.Integer;
			var chain = Chain.Create(state).Error();
			int sideEffect = 1;
			void a(IOkV<bool, int> _) => sideEffect++;

			// Act
			chain.AuditSwitch(isOkV: a);

			// Assert
			Assert.Equal(1, sideEffect);
		}

		[Fact]
		public void IOkV_Input_Catches_Exception()
		{
			// Arrange
			var value = F.Rand.Integer;
			var state = F.Rand.Integer;
			var chain = Chain.CreateV(value, state);
			static void a(IOkV<int, int> _) => throw new Exception();

			// Act
			var next = chain.AuditSwitch(isOkV: a);

			// Assert
			Assert.Equal(1, next.Messages.Count);
			Assert.True(next.Messages.Contains<Jm.Audit.AuditSwitchExceptionMsg>());
		}
	}
}
