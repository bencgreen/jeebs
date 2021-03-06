﻿// Jeebs Unit Tests
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using System.Threading.Tasks;
using NSubstitute;
using Xunit;

namespace Jeebs.Data.Repository_Tests
{
	public class CreateAsync_Tests
	{
		[Fact]
		public async Task Calls_Client_GetCreateQuery()
		{
			// Arrange
			var (client, _, repo) = Repository_Setup.Get();
			var foo = new Repository_Setup.Foo { Id = new(F.Rnd.Ulng) };

			// Act
			await repo.CreateAsync(foo);

			// Assert
			client.Received().GetCreateQuery<Repository_Setup.Foo>();
		}

		[Fact]
		public async Task Logs_Query_To_Debug()
		{
			// Arrange
			var (_, log, repo) = Repository_Setup.Get();
			var foo = new Repository_Setup.Foo { Id = new(F.Rnd.Ulng) };

			// Act
			await repo.CreateAsync(foo);

			// Assert
			log.ReceivedWithAnyArgs().Debug(Arg.Any<string>(), Arg.Any<object[]>());
		}
	}
}
