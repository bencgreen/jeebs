﻿// Jeebs Unit Tests
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using Xunit;

namespace Jeebs.Config.SlackConfig_Tests
{
	public class Constructor_Tests
	{
		[Fact]
		public void Implements_ServiceConfig()
		{
			// Arrange
			var config = new SlackConfig();

			// Act

			// Assert
			Assert.IsAssignableFrom<IServiceConfig>(config);
		}

		[Fact]
		public void Implements_WebhookServiceConfig()
		{
			// Arrange
			var config = new SlackConfig();

			// Act

			// Assert
			Assert.IsAssignableFrom<WebhookServiceConfig>(config);
		}
	}
}
