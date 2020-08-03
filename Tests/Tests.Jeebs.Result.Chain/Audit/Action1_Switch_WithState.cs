﻿using System;
using System.Collections.Generic;
using System.Text;
using Jeebs;
using Xunit;

namespace Tests.Jeebs.Result.Audit
{
	public class Action1_Switch_WithState : IAudit_Action1_Switch
	{
		[Fact]
		public void AuditSwitch_Returns_Original_Object_Without_Parameters()
		{
			// Arrange
			var chain = Chain.Create(false);

			// Act
			var result = chain.AuditSwitch();

			// Assert
			Assert.StrictEqual(chain, result);
		}

		[Fact]
		public void AuditSwitch_Returns_Original_Object_With_Parameters()
		{
			// Arrange
			var chain = Chain.Create(false);
			static void ok<TResult, TState>(IOk<TResult, TState> _) { }
			static void okV<TResult, TState>(IOkV<TResult, TState> _) { }
			static void error<TResult, TState>(IError<TResult, TState> _) { }

			// Act
			var result = chain.AuditSwitch(ok, okV, error);

			// Assert
			Assert.StrictEqual(chain, result);
		}

		[Fact]
		public void AuditSwitch_Catches_Exception_Adds_Message()
		{
			// Arrange
			var chain = Chain.Create(false);
			static void fail<TResult, TState>(IOk<TResult, TState> _) => throw new Exception();

			// Act
			var result = chain.AuditSwitch(isOk: fail);

			// Assert
			Assert.True(result.Messages.Contains<Jm.AuditExceptionMsg>());
		}

		[Fact]
		public void AuditSwitch_Runs_Ok_When_Ok()
		{
			// Arrange
			var chain = Chain.Create(false);
			var run = false;
			void ok<TResult, TState>(IOk<TResult, TState> _) { run = true; }

			// Act
			chain.AuditSwitch(isOk: ok);

			// Assert
			Assert.True(run);
		}

		[Fact]
		public void AuditSwitch_DoesNot_Run_OkV_When_Ok()
		{
			// Arrange
			var chain = Chain.Create(false);
			var run = false;
			void okV<TResult, TState>(IOkV<TResult, TState> _) { run = true; }

			// Act
			chain.AuditSwitch(isOkV: okV);

			// Assert
			Assert.False(run);
		}

		[Fact]
		public void AuditSwitch_DoesNot_Run_Error_When_Ok()
		{
			// Arrange
			var chain = Chain.Create(false);
			var run = false;
			void error<TResult, TState>(IError<TResult, TState> _) { run = true; }

			// Act
			chain.AuditSwitch(isError: error);

			// Assert
			Assert.False(run);
		}

		[Fact]
		public void AuditSwitch_Runs_OkV_When_OkV()
		{
			// Arrange
			var chain = Chain.CreateV(18, false);
			var run = false;
			void okV<TResult, TState>(IOkV<TResult, TState> _) { run = true; }

			// Act
			chain.AuditSwitch(isOkV: okV);

			// Assert
			Assert.True(run);
		}

		[Fact]
		public void AuditSwitch_DoesNot_Run_Ok_When_OkV()
		{
			// Arrange
			var chain = Chain.CreateV(18, false);
			var run = false;
			void ok<TResult, TState>(IOk<TResult, TState> _) { run = true; }

			// Act
			chain.AuditSwitch(isOk: ok);

			// Assert
			Assert.False(run);
		}

		[Fact]
		public void AuditSwitch_DoesNot_Run_Error_When_OkV()
		{
			// Arrange
			var chain = Chain.CreateV(18, false);
			var run = false;
			void error<TResult, TState>(IError<TResult, TState> _) { run = true; }

			// Act
			chain.AuditSwitch(isError: error);

			// Assert
			Assert.False(run);
		}

		[Fact]
		public void AuditSwitch_Runs_Error_When_Error()
		{
			// Arrange
			var chain = Chain.Create(false).Error();
			var run = false;
			void error<TResult, TState>(IError<TResult, TState> _) { run = true; }

			// Act
			chain.AuditSwitch(isError: error);

			// Assert
			Assert.True(run);
		}

		[Fact]
		public void AuditSwitch_DoesNot_Run_Ok_When_Error()
		{
			// Arrange
			var chain = Chain.Create(false).Error();
			var run = false;
			void ok<TResult, TState>(IOk<TResult, TState> _) { run = true; }

			// Act
			chain.AuditSwitch(isOk: ok);

			// Assert
			Assert.False(run);
		}

		[Fact]
		public void AuditSwitch_DoesNot_Run_OkV_When_Error()
		{
			// Arrange
			var chain = Chain.Create(false).Error();
			var run = false;
			void okV<TResult, TState>(IOkV<TResult, TState> _) { run = true; }

			// Act
			chain.AuditSwitch(isOkV: okV);

			// Assert
			Assert.False(run);
		}
	}
}