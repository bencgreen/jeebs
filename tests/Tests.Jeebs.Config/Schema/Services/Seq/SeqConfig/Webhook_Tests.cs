﻿// Jeebs Unit Tests
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using Xunit;

namespace Jeebs.Config.SeqConfig_Tests
{
	public class Webhook_Tests
	{
		[Fact]
		public void Set_Does_Nothing()
		{
			// Arrange
			var config = new SeqConfig
			{
				Webhook = F.Rnd.Str
			};

			// Act
			var result = config.Webhook;

			// Assert
			Assert.Equal("/api/events/raw?clef", result);
		}

		[Fact]
		public void Returns_With_Server_Value()
		{
			// Arrange
			var server = F.Rnd.Str;
			var config = new SeqConfig { Server = server };

			// Act
			var result = config.Webhook;

			// Assert
			Assert.Equal($"{server}/api/events/raw?clef", result);
		}
	}
}
