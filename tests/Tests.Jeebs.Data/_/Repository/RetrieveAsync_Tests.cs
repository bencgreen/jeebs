﻿// Jeebs Unit Tests
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using System.Threading.Tasks;
using NSubstitute;
using Xunit;

namespace Jeebs.Data.Repository_Tests
{
	public class RetrieveAsync_Tests
	{
		[Fact]
		public async Task Calls_Client_GetRetrieveQuery()
		{
			// Arrange
			var (client, _, repo) = Repository_Setup.Get();
			var value = F.Rnd.Ulng;

			// Act
			await repo.RetrieveAsync<Repository_Setup.FooModel>(new Repository_Setup.FooId(value));

			// Assert
			client.Received().GetRetrieveQuery<Repository_Setup.Foo, Repository_Setup.FooModel>(value);
		}

		[Fact]
		public async Task Logs_Query_To_Debug()
		{
			// Arrange
			var (_, log, repo) = Repository_Setup.Get();
			var value = F.Rnd.Ulng;

			// Act
			await repo.RetrieveAsync<Repository_Setup.FooModel>(new Repository_Setup.FooId(value));

			// Assert
			log.ReceivedWithAnyArgs().Debug(Arg.Any<string>(), Arg.Any<object[]>());
		}
	}
}
