﻿// Jeebs Unit Tests
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using NSubstitute;
using Xunit;

namespace Jeebs.Data.DbFunc_Tests
{
	public class WriteToLog_Tests
	{
		[Fact]
		public void Sends_Message_And_Args_To_Debug_Log()
		{
			// Arrange
			var (_, log, func) = DbFunc_Setup.Get();
			var message = F.Rnd.Str;
			var args = new object[] { F.Rnd.Int, F.Rnd.Int };

			// Act
			func.WriteToLogTest(message, args);

			// Assert
			log.Received().Debug(message, args);
		}
	}
}