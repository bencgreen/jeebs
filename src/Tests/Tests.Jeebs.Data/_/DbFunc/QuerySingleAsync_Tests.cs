﻿// Jeebs Unit Tests
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Jeebs.Data.Enums;
using NSubstitute;
using Xunit;

namespace Jeebs.Data.DbFunc_Tests
{
	public class QuerySingleAsync_Tests
	{
		[Fact]
		public async Task Calls_Client_GetQuery()
		{
			// Arrange
			var (client, _, func) = DbFunc_Setup.Get();
			var predicates = new List<(Expression<Func<DbFunc_Setup.Foo, object>>, SearchOperator, object)>
			{
				(f => f.Id, SearchOperator.NotEqual, F.Rnd.Int)
			}.ToArray();

			// Act
			await func.QuerySingleAsync<DbFunc_Setup.FooModel>(predicates);

			// Assert
			client.Received().GetQuery<DbFunc_Setup.Foo, DbFunc_Setup.FooModel>(predicates);
		}

		[Fact]
		public async Task Logs_Query_To_Debug()
		{
			// Arrange
			var (_, log, func) = DbFunc_Setup.Get();
			var predicates = new List<(Expression<Func<DbFunc_Setup.Foo, object>>, SearchOperator, object)>
			{
				(f => f.Id, SearchOperator.NotEqual, F.Rnd.Int)
			}.ToArray();

			// Act
			await func.QuerySingleAsync<DbFunc_Setup.FooModel>(predicates);

			// Assert
			log.ReceivedWithAnyArgs().Debug(Arg.Any<string>(), Arg.Any<object[]>());
		}
	}
}