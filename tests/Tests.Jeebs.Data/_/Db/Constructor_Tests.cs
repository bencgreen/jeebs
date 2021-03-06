﻿// Jeebs Unit Tests
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using Xunit;

namespace Jeebs.Data.Db_Tests
{
	public class Constructor_Tests
	{
		[Fact]
		public void Sets_Properties()
		{
			// Arrange

			// Act
			var (config, log, client, _, db) = Db_Setup.Get();

			// Assert
			Assert.Same(client, db.Client);
			Assert.Same(config, db.Config);
			Assert.Same(log, db.LogTest);
		}
	}
}
